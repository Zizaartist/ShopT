using Akavache;
using ShopT.Models;
using ShopT.StaticValues;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopT.ViewModels
{
    public class DataUserCacheViewModels : ViewModel
    {

        private string name;
        public string Name
        {
            get => name;
            set
            {
                SetProperty(ref name, value);
            }
        }

        private string street;
        public string Street
        {
            get => street;
            set
            {
                SetProperty(ref street, value);
            }
        }

        private string house;
        public string House
        {
            get => house;
            set
            {
                SetProperty(ref house, value);
            }
        }

        //nullable
        private int? entrance;
        public int? Entrance
        {
            get => entrance;
            set
            {
                SetProperty(ref entrance, value);
            }
        }

        //nullable
        private int? floor;
        public int? Floor
        {
            get => floor;
            set
            {
                SetProperty(ref floor, value);
            }
        }

        //nullable
        private int? apartment;
        public int? Apartment
        {
            get => apartment;
            set
            {
                SetProperty(ref apartment, value);
            }
        }


        public DataUserCacheViewModels()
        {
           
        }


        public async Task Autofill()
        {
            var orderData = await new CacheFunctions().tryToGet<Order>(Caches.AUTOFILL_CACHE.key, CacheFunctions.BlobCaches.UserAccount);

            Name = orderData.OrdererName;
            Street = orderData.OrderInfo.Street;
            House = orderData.OrderInfo.House;
            Apartment = orderData.OrderInfo.Apartment;
            Entrance = orderData.OrderInfo.Entrance;
            Floor = orderData.OrderInfo.Floor;
        }

        public async Task SaveAutofill()
        {
            var orderData = new Order()
            {
                OrdererName = Name,
                OrderInfo = new OrderInfo()
                {
                    Street = Street,
                    House = House,
                    Apartment = Apartment,
                    Entrance = Entrance,
                    Floor = Floor
                }
            };

            await BlobCache.UserAccount.InsertObject(Caches.AUTOFILL_CACHE.key, orderData);
        }



    }
}
