using Newtonsoft.Json;
using ShopT.Models.HubModels;
using ShopT.StaticValues;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ShopT.ViewModels
{
    public class ShopConfigViewModel : ViewModel
    {
        public async Task<HttpResponseMessage> RefreshConfiguration() 
        {
            HttpClient client = HttpClientSingleton.Instance;

            var response = await client.GetAsync($"{ApiStrings.SHOPT_HUB}{ApiStrings.HUB_SHOP_CONFIG_GET}{ShopInfoStatic.currentShopId}/{ShopInfoStatic.shopConfiguration.Version}");
            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                ShopConfiguration temp = JsonConvert.DeserializeObject<ShopConfiguration>(result);

                //Если текущая конфигурация не null - перезаписать
                if (temp != null) ShopInfoStatic.shopConfiguration = temp;
            }
            return response;
        }
    }
}
