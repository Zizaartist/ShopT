using Akavache;
using Newtonsoft.Json;
using ShopT.Models.HubModels;
using ShopT.Models.Maps;
using ShopT.StaticValues;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

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

        public ObservableRangeCollection<Location> Locations { get; private set; }

        private Location selectedLocation;
        public Location SelectedLocation 
        {
            get => selectedLocation;
            set { SetProperty(ref selectedLocation, value); }
        }

        public ObservableRangeCollection<Location> SearchLocations { get; }

        private string searchCriteria;
        public string SearchCriteria 
        {
            get => searchCriteria;
            set { SetProperty(ref searchCriteria, value); }
        }

        public Command SearchCommand { get; }

        public ShopViewModel shopVM { get; private set; }

        public LocationViewModel() 
        {
            shopVM = new ShopViewModel();
            Locations = new ObservableRangeCollection<Location>();
            SearchLocations = new ObservableRangeCollection<Location>();

            SearchCommand = new Command(() =>
            {
                var criteriaCaps = SearchCriteria?.ToUpper();

                SearchLocations.ReplaceRange(Locations.Where(loc =>
                {
                    if (string.IsNullOrEmpty(criteriaCaps)) return true;
                    return loc.LocationName.ToUpper().Contains(criteriaCaps);
                }));
            });

            Task.Run(() => GetCachedData());
        }

        public async Task InitLocations(IEnumerable<Location> locations) 
        {
            Locations.ReplaceRange(locations);
            SearchLocations.ReplaceRange(Locations);

            var defaultId = Locations.First().LocationId;

            var lastSelectedId = await new CacheFunctions().tryToGet<int>($"{Caches.LOCATION_SELECTED.key}", CacheFunctions.BlobCaches.UserAccount);
            SelectedLocation = Locations.First(loc => loc.LocationId == (lastSelectedId == default ? defaultId : lastSelectedId));

            await shopVM.GetInitialData.ExecuteAsync();
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
                    
                    await InitLocations(tempList);
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
                await InitLocations(cachedLocations);
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

        public async Task LocationSelected()
        {
            await BlobCache.UserAccount.InsertObject($"{Caches.LOCATION_SELECTED.key}", SelectedLocation.LocationId);
            await shopVM.GetInitialData.ExecuteAsync();
        }
    }
}
