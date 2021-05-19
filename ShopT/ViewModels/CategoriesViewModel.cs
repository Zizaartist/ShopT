using Akavache;
using MvvmHelpers;
using Newtonsoft.Json;
using ShopT.Models;
using ShopT.Models.HubModels;
using ShopT.Models.LocalModels;
using ShopT.Models.Statistics;
using ShopT.StaticValues;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace ShopT.ViewModels
{
    public class CategoryViewModel : CollectionViewModel
    {
        public ShopLocal _shopLocal { get; set; }

        public ObservableRangeCollection<CategoryLocal> Categories { get; } = new ObservableRangeCollection<CategoryLocal>();

        private int? parentCategoryId;

        public CategoryViewModel(int? _parentCategoryId = null)
        {
            parentCategoryId = _parentCategoryId;
            _shopLocal = new ShopLocal(new Shop{ 
            
                ShopInfo = ShopInfoStatis.shopInfo,
                ShopConfiguration = ShopInfoStatis.shopConfiguration
            });

            GetInitialData = NewAsyncCommand(GetInitial);
        }

        public async Task GetInitial()
        {
            try
            {
                HttpClient client = await createUserClient();

                //Получение всех меню по id бренда
                HttpResponseMessage response = await client.GetAsync($"{ApiStrings.HOST}{ApiStrings.CATEGORIES_CONTROLLER}{parentCategoryId}");
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    List<Category> tempList = JsonConvert.DeserializeObject<List<Category>>(result);

                    Categories.Clear();

                    var localizedList = new List<CategoryLocal>();
                    foreach (var item in tempList)
                    {
                        localizedList.Add(new CategoryLocal(item));
                    }
                    Categories.AddRange(localizedList);

                    await BlobCache.LocalMachine.InsertObject($"{ShopInfoStatis.shopInfo.ShopId}_{Caches.CATEGORIES_CACHE.key}_{parentCategoryId}", tempList, Caches.CATEGORIES_CACHE.lifeTime);
                }
            }
            catch (Exception e)
            {
                throw (e);
            }
            finally 
            {
                IsWorking = false;
            }
        }

        public async Task GetCachedData() 
        {
            //Пытаемся вытащить данные из кэша, при неудаче создаем пустую ячейку для предотвращения KeyNotFoundException
            List<Category> cachedCategories = await new CacheFunctions().tryToGet<List<Category>>($"{ShopInfoStatis.shopInfo.ShopId}_{Caches.CATEGORIES_CACHE.key}_{parentCategoryId}", CacheFunctions.BlobCaches.LocalMachine);

            Categories.Clear();

            //В случае если кэш не пуст
            if (cachedCategories != null)
            {
                var localizedList = new List<CategoryLocal>();
                foreach (var item in cachedCategories)
                {
                    localizedList.Add(new CategoryLocal(item));
                }
                Categories.AddRange(localizedList);
            }
            //В случае если он пуст
            else
            {
                try
                {
                    await GetInitialData.ExecuteAsync();
                }
                catch (Exception e)
                {
                    throw (e);
                }
            }
        }
    }
}
