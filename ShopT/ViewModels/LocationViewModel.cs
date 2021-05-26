using Akavache;
using Newtonsoft.Json;
using ShopT.Models.HubModels;
using ShopT.StaticValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace ShopT.ViewModels
{
    public class LocationViewModel : ViewModel
    {
        private static object syncRoot = new Object();
        private static LocationViewModel instance;
        public static LocationViewModel Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new LocationViewModel();
                    }
                }
                return instance;
            }
        }

        public List<Location> Locations { get; private set; }

        private Location selectedLocation;
        public Location SelectedLocation
        {
            get => selectedLocation;
            set 
            {
                if (selectedLocation == value) return;
                selectedLocation = value;
                OnPropertyChanged();

                Task.Run(async () => 
                { 
                    await BlobCache.UserAccount.InsertObject($"{Caches.LOCATION_SELECTED.key}", value.LocationId); 
                    await shopVM.GetInitialData.ExecuteAsync();
                }); 
            }
        }

        public ShopViewModel shopVM { get; private set; }

        public LocationViewModel() 
        {
            shopVM = new ShopViewModel();
            Locations = new List<Location>();
        }

        public async Task ReplaceLocations(IEnumerable<Location> locations) 
        {
            Locations.Clear();
            Locations.AddRange(locations);
            OnPropertyChanged(nameof(Locations));

            var defaultId = Locations.First().LocationId;

            var lastSelectedId = await new CacheFunctions().tryToGet<int>($"{Caches.LOCATION_SELECTED.key}", CacheFunctions.BlobCaches.UserAccount);
            SelectedLocation = Locations.First(loc => loc.LocationId == (lastSelectedId == default ? defaultId : lastSelectedId));
        }

        public async Task GetRemoteData() 
        {
            try
            {
                HttpClient client = new HttpClient();

                var response = await client.GetAsync($"{ApiStrings.HUB}{ApiStrings.HUB_LOCATIONS_ALL}");
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    List<Location> tempList = JsonConvert.DeserializeObject<List<Location>>(result);

                    await BlobCache.LocalMachine.InsertObject($"{Caches.LOCATIONS_CACHE.key}", tempList, Caches.LOCATIONS_CACHE.lifeTime);
                    await ReplaceLocations(tempList);
                }
            }
            catch (Exception _ex) 
            {
            
            }
        }


        public async Task GetCachedData()
        {
            //Пытаемся вытащить данные из кэша, при неудаче создаем пустую ячейку для предотвращения KeyNotFoundException
            List<Location> cachedLocations = await new CacheFunctions().tryToGet<List<Location>>($"{Caches.LOCATIONS_CACHE.key}", CacheFunctions.BlobCaches.LocalMachine);

            //В случае если кэш не пуст
            if (cachedLocations != default)
            {
                await ReplaceLocations(cachedLocations);
            }
            //В случае если он пуст
            else
            {
                try
                {
                    await GetRemoteData();
                }
                catch (Exception e)
                {
                    //idk
                }
            }
        }
    }
}
