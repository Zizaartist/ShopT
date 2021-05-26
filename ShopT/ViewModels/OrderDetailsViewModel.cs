using MvvmHelpers;
using Newtonsoft.Json;
using ShopT.Models;
using ShopT.Models.LocalModels;
using ShopT.StaticValues;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ShopT.ViewModels
{
    public class OrderDetailsViewModel : ViewModel
    {
        #region properties

        public ObservableRangeCollection<OrderDetailLocal> Details { get; } = new ObservableRangeCollection<OrderDetailLocal>();

        #endregion

        public async Task GetRemoteData(int orderId) 
        {
            try
            {
                HttpClient client = await createUserClient();

                var response = await client.GetAsync($"{ApiStrings.HOST}/{ApiStrings.ORDERS_CONTROLLER}{orderId}");
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    List<OrderDetail> tempList = JsonConvert.DeserializeObject<List<OrderDetail>>(result);

                    Details.Clear();

                    var localizedList = new List<OrderDetailLocal>();
                    foreach (var item in tempList)
                    {
                        localizedList.Add(new OrderDetailLocal(item));
                    }
                    Details.AddRange(localizedList);
                }
            }
            catch(Exception _ex)
            {
                //idk
            }
        }
    }
}
