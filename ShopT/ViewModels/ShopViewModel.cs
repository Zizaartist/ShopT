
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
        private ObservableRangeCollection<ShopLocal> shops;
        public ObservableRangeCollection<ShopLocal> Shops
        {
            get => shops;
            set
            {
                if (value == shops) return;
                shops = value;
                OnPropertyChanged();
            }
        }
        public ShopViewModel()
        {
            Shops = new ObservableRangeCollection<ShopLocal>();

            GetInitial();
        }

        public async void GetInitial()
        {
            try
            {
                HttpClient client = new HttpClient();

                HttpResponseMessage response = await client.GetAsync($"{ApiStrings.SHOPT_HUB}{ApiStrings.HUB_SHOPS_BY_LOCATION}1");
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    List<Shop> tempList = JsonConvert.DeserializeObject<List<Shop>>(result);
                    Shops.Clear();

                    var localizedList = new List<ShopLocal>();
                    foreach (var item in tempList)
                    {
                        localizedList.Add(new ShopLocal(item));
                    }

                    StaticShopLocals.shopLocals = localizedList;
                    Shops.AddRange(localizedList);

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

    }
}
