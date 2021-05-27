
using MvvmHelpers;
using Newtonsoft.Json;
using ShopT.Models.HubModels;
using ShopT.Models.LocalModels;
using ShopT.StaticValues;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ShopT.ViewModels
{
    public class ShopViewModel : CollectionViewModel
    {
        public ObservableRangeCollection<ShopLocal> Shops { get; }

        public ShopViewModel()
        {
            Shops = new ObservableRangeCollection<ShopLocal>();

            GetInitialData = NewAsyncCommand(GetInitial);
        }

        public async Task GetInitial()
        {
            try
            {
                var lastSelectedId = await new CacheFunctions().tryToGet<int>($"{Caches.LOCATION_SELECTED.key}", CacheFunctions.BlobCaches.UserAccount);

                HttpClient client = new HttpClient();

                HttpResponseMessage response = await client.GetAsync($"{ApiStrings.HUB}{ApiStrings.HUB_SHOPS_BY_LOCATION}{lastSelectedId}");

                Shops.Clear();

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    List<Shop> tempList = JsonConvert.DeserializeObject<List<Shop>>(result);

                    var localizedList = new List<ShopLocal>();
                    foreach (var item in tempList)
                    {
                        localizedList.Add(new ShopLocal(item));
                    }

                    Shops.AddRange(localizedList);
                }
            }
            catch (Exception e)
            {
                //
            }
            finally
            {
                IsWorking = false;
            }
        }
    }
}
