using MvvmHelpers;
using ShopT.StaticValues;
using System;
using Xamarin.Forms;

namespace ShopT.Models.LocalModels
{
    public class OrderDetailLocal : ObservableObject
    {
        public OrderDetailLocal(OrderDetail _orderDetail, Command _saveChanges, Command _selfDestructCommand)
        {
            OrderDetail = _orderDetail;
            selfDestructCommand = _selfDestructCommand;
            saveChanges = _saveChanges;

            Logo = OrderDetail.Product.Image != null ? new UriImageSource
            {
                Uri = new Uri(ApiStrings.HOST_ADMIN + ApiStrings.IMAGES_FOLDER + OrderDetail.Product.Image),
                CachingEnabled = true,
                CacheValidity = Caches.IMAGE_CACHE.lifeTime
            } : null;
            AddCount = new Command(() =>
            {
                if (Count < maxCount) 
                {
                    Count++;
                    saveChanges.Execute(null);
                } 
            });
            SubCount = new Command(() =>
            {
                Count--;
                saveChanges.Execute(null);
            });
        }

        public OrderDetailLocal(OrderDetail _orderDetail)
        {
            OrderDetail = _orderDetail;

            Logo = OrderDetail.Product.Image != null ? new UriImageSource
            {
                Uri = new Uri($"{ApiStrings.HOST_ADMIN}{ApiStrings.IMAGES_FOLDER}{OrderDetail.Product.Image}"),
                CachingEnabled = true,
                CacheValidity = Caches.IMAGE_CACHE.lifeTime
            } : null;
        }

        public void InjectSumCommand(Command _updateSum) => updateSum = _updateSum;

        private Command saveChanges;
        private Command selfDestructCommand;
        private Command updateSum;
        public Command AddCount { get; private set; }
        public Command SubCount { get; private set; }

        private int maxCount = 99;

        public OrderDetail OrderDetail { get; set; }
        public UriImageSource Logo { get; private set; }
        public decimal SumPrice { get => OrderDetail.Price * OrderDetail.Count; }
        public int Count 
        {
            get => OrderDetail.Count;
            set 
            {
                if (Count == value) return;
                if (value <= 0)
                {
                    selfDestructCommand.Execute(this);
                }
                else
                {
                    OrderDetail.Count = value;
                    OnPropertyChanged();
                    OnPropertyChanged("SumPrice");
                    updateSum?.Execute(null);
                }
            }
        }

        public bool Discount { get => OrderDetail.Discount != null; }
        public decimal OldPrice { get => OrderDetail.Price; }
        public decimal Price { get => OrderDetail.Discount != null ? OrderDetail.Price - (OrderDetail.Price / 100 * OrderDetail.Discount.Value) : OrderDetail.Price; }
    }
}
