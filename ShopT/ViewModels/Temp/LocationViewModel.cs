using ShopT.Models.HubModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace ShopT.ViewModels.Temp
{
    class LocationViewModel : BindableObject
    {
        private ObservableCollection<Location> locations;
        public ObservableCollection<Location> Locations
        {
            get => locations;
            set
            {
                if (value == locations) return;
                locations = value;
                OnPropertyChanged();
            }
        }
        public LocationViewModel()
        {
            Locations = new ObservableCollection<Location>()
            {
                new Location()
                {
                    LocationName = "Якутск",
                },
                new Location()
                {
                    LocationName = "Москва",
                },
            };
        }
    }
}
