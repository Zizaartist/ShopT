using Akavache;
using MvvmHelpers;
using ShopT.Models;
using ShopT.Models.LocalModels;
using ShopT.StaticValues;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ShopT.ViewModels
{
    public class BasketViewModel : ViewModel
    {
        #region properties

        private static BasketViewModel instance = null;

        //brandId & product pair
        public ObservableRangeCollection<OrderDetailLocal> OrderDetails { get; } = new ObservableRangeCollection<OrderDetailLocal>();

        public decimal TotalSum { get => OrderDetails.Sum(detail => detail.SumPrice); }



        public Command OrderDetailSelfDestruct { get; }

        public Command AddToBasket { get; } 

        public Command SaveChangesCommand { get; }

        #endregion

        private BasketViewModel()
        {
            //any просто для итерации, не результата
            OrderDetailSelfDestruct = new Command(param =>
            {
                var detail = param as OrderDetailLocal;
                OrderDetails.Remove(detail);
            });

            OrderDetails.CollectionChanged += (sender, e) => OnPropertyChanged("TotalSum");

            //Fire and forget
            SaveChangesCommand = new Command(async () => 
            {
                await SaveChangesToCache();
                OnPropertyChanged(nameof (TotalSum));
            });

            AddToBasket = new Command(async param =>
            {
                var product = param as ProductLocal;

                //var toAdd = new List<OrderDetailLocal>();
                //foreach (var product in products)
                //{
                //    toAdd.Add(new OrderDetailLocal(new OrderDetail()
                //    {
                //        Product = product.Product,
                //        Count = product.Count,
                //        Price = product.Product.Price,
                //        ProductId = product.Product.ProductId
                //    }
                //    , SaveChangesCommand, OrderDetailSelfDestruct));
                //}

                var eee = new OrderDetailLocal(new OrderDetail()
                {
                    Product = product.Product,
                    Count = product.Count,
                    Price = product.Price,
                    ProductId = product.Product.ProductId

                }, SaveChangesCommand, OrderDetailSelfDestruct);

                await AddDetails(eee);

                //products.Clear();
            });


        }

        public static BasketViewModel getInstance()
        {
            if (instance == null)
                instance = new BasketViewModel();
            return instance;
        }

        #region methods

        public async Task GetData()
        {
            OrderDetails.Clear();

            //Пытаемся вытащить данные из кэша, при неудаче создаем пустую ячейку для предотвращения KeyNotFoundException
            List<OrderDetail> cachedShopcart = await new CacheFunctions().tryToGet<List<OrderDetail>>(Caches.SHOPCART_CACHE.key, CacheFunctions.BlobCaches.UserAccount);

            //В случае если кэш не пуст
            if (cachedShopcart != null)
            {
                var temp = new List<OrderDetailLocal>();
                cachedShopcart.ForEach(detail => temp.Add(new OrderDetailLocal(detail, SaveChangesCommand, OrderDetailSelfDestruct)));

                OrderDetails.AddRange(temp);
            }
        }

        /// <summary>
        /// Добавляет новую деталь заказа и сохраняет изменения в кэше
        /// </summary>
        public async Task AddDetails(OrderDetailLocal newDetails)
        {
            //var toAdd = new List<OrderDetailLocal>();

            //foreach (var detail in newDetails)
            //{
            //    var found = OrderDetails?.FirstOrDefault(d => d.OrderDetail.ProductId == detail.OrderDetail.ProductId);
            //    if (found != null)
            //    {
            //        found.Count += detail.Count;
            //    }
            //    else
            //    {
            //        toAdd.Add(detail);
            //    }
            //}

            var found = OrderDetails?.FirstOrDefault(d => d.OrderDetail.ProductId == newDetails.OrderDetail.ProductId);
            if (found != null)
            {
                found.Count += newDetails.Count;
            }
            else
            {
                OrderDetails.Add(newDetails);
            }

            await SaveChangesToCache();
        }

        /// <summary>
        /// Удаляет деталь из корзины и сохраняет изменения в кэше
        /// </summary>
        public async Task RemoveDetail(IEnumerable<OrderDetailLocal> delDetails)
        {
            var toRemove = new List<OrderDetailLocal>();

            foreach (var detail in delDetails)
            {
                var found = OrderDetails.FirstOrDefault(d => d.OrderDetail.ProductId == detail.OrderDetail.ProductId);
                if (found != null)
                {
                    toRemove.Add(found);
                }
            }

            OrderDetails.RemoveRange(toRemove);
            await SaveChangesToCache();
        }

        /// <summary>
        /// Удаляет все детали из корзины и сохраняет изменения в кэш
        /// </summary>
        public async Task Clear()
        {
            OrderDetails.Clear();
            await SaveChangesToCache();
        }

        public async Task SaveChangesToCache()
        {
            if (IsBusy) return; //preventing parallel calls, is it though?
            IsBusy = true;
            await BlobCache.UserAccount.InsertObject(Caches.SHOPCART_CACHE.key, OrderDetails.Select(detail => detail.OrderDetail));
            IsBusy = false;
        }

        #endregion
    }
}
