using System;
using Akavache;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using System.Reactive.Linq;
using Xamarin.Forms;
using MvvmHelpers;
using ShopT.Models.LocalModels;
using ShopT.Models;
using ShopT.StaticValues;
using ShopT.Models.Statistics;

namespace ShopT.ViewModels
{
    public class ProductsViewModel : CollectionViewModel
    {
        #region properties

        public ObservableRangeCollection<ProductLocal> ProductLists { get; } = new ObservableRangeCollection<ProductLocal>();

        public ObservableCollection<ProductLocal> SelectedProducts { get; } = new ObservableCollection<ProductLocal>();

        public ProductLocal SelectedProduct { get; set; }
        public ProductLocal ProductSetter
        {
            get => null;
            set 
            {
                if (SelectedProduct != value)
                {
                    SelectedProduct = value;
                    OnPropertyChanged("SelectedProduct");
                } 
                OnPropertyChanged(); //Всегда сбрасывает обратно на null
            }
        }

        public int AddToCartCount { get => ProductLists.Sum(e => e.Count); }

        public bool AddToCartNotEmpty { get => SelectedProducts.Any(); }

        public decimal SumTotal { get => ProductLists.Sum(e => e.SumPrice); }

        private Category menu;

        public Command AddToBasket { get; }
        public Command AddToSelected { get; }
        public Command RemoveFromSelected { get; }

        #endregion

        #region methods

        public ProductsViewModel(Category _menu, Command _addToBasket = null)
        {
            menu = _menu;

            GetInitialData = NewAsyncCommand(GetInitial);
            GetMoreData = NewAsyncCommand(GetRemoteData);

            ProductLists.CollectionChanged += (sender, e) => UpdateBindings();
            SelectedProducts.CollectionChanged += (sender, e) => 
            {
                OnPropertyChanged("AddToCartNotEmpty");
                if (!AddToCartNotEmpty)
                {
                    foreach (var product in ProductLists)
                    {
                        product.Count = 0;
                    }
                }
            };

            AddToBasket = _addToBasket;

            AddToSelected = new Command((param) => SelectedProducts.Add(param as ProductLocal));
            RemoveFromSelected = new Command((param) => SelectedProducts.Remove(param as ProductLocal));
        }

        public async Task GetInitial()
        {
            ProductLists.Clear();
            NextPage = 0;

            try
            {
                await GetMoreData.ExecuteAsync();
            }
            catch (NoConnectionException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw CheckIfConnectionException(e);
            }
        }

        public async Task GetRemoteData()
        {
            if (IsLastPageReached) return;
            try
            {
                HttpClient client = await createUserClient();

                //Получение всех продуктов по id меню
                HttpResponseMessage response = await client.GetAsync(ApiStrings.HOST + ApiStrings.PRODUCTS_GET_BY_MENU + menu.CategoryId + "/" + NextPage);
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    List<Product> tempList = JsonConvert.DeserializeObject<List<Product>>(result);

                    if (tempList.Count != 0)
                    {
                        NextPage++;
                    }

                    tempList.ForEach(e => e.Category = menu);

                    foreach (var item in tempList)
                    {
                        ProductLists.Add(new ProductLocal(item, UpdateBindings, AddToSelected, RemoveFromSelected, AddToBasket));
                    }

                    await BlobCache.LocalMachine.InsertObject($"{ShopInfoStatic.shopInfo.ShopId}_{Caches.PRODUCTS_CACHE.key}" + "_" + menu.CategoryId.ToString(), (NextPage, ProductLists.Select(e => e.Product)), Caches.PRODUCTS_CACHE.lifeTime);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    NextPage = -1;
                }
            }
            catch (NoConnectionException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw CheckIfConnectionException(e);
            }
            finally 
            {
                IsWorking = false;
            }
        }

        public async Task GetCachedData()
        {
            //Пытаемся вытащить данные из кэша, при неудаче создаем пустую ячейку для предотвращения KeyNotFoundException
            (int, List<Product>) cachedProducts = await new CacheFunctions().tryToGet<(int, List<Product>)>($"{ShopInfoStatic.shopInfo.ShopId}_{Caches.PRODUCTS_CACHE.key}" + "_" + menu.CategoryId.ToString(), CacheFunctions.BlobCaches.LocalMachine);

            ProductLists.Clear();

            //В случае если кэш не пуст
            if (cachedProducts != default)
            {
                var localizedList = new List<ProductLocal>();
                foreach (Product product in cachedProducts.Item2)
                {
                    localizedList.Add(new ProductLocal(product, UpdateBindings, AddToSelected, RemoveFromSelected, AddToBasket));
                }
                ProductLists.AddRange(localizedList);

                NextPage = cachedProducts.Item1;
            }
            //В случае если он пуст
            else
            {
                try
                {
                    await GetInitialData.ExecuteAsync();
                }
                catch (NoConnectionException)
                {
                    throw;
                }
                catch (Exception e)
                {
                    throw CheckIfConnectionException(e);
                }
            }
        }

        public void UpdateBindings()
        {
            OnPropertyChanged("AddToCartCount");
            OnPropertyChanged("AddToCartNotEmpty");
            OnPropertyChanged("SumTotal");
        }

        #endregion
    }
}
