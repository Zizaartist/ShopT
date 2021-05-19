using MvvmHelpers;
using ShopT.StaticValues;
using System;
using Xamarin.Forms;

namespace ShopT.Models.LocalModels
{
    public class ProductLocal : ObservableObject
    {
        #region properties

        public Product Product { get; private set; }
        public UriImageSource Image { get; private set; }
        private Action refreshAllData; //Делегат для обновления внешних данных

        private int count = 1;
        public int Count
        {
            get => count;
            set
            {
                if (count == value) return;
                var initialValue = count;
                count = value;
                if (Count <= 0 && initialValue > 0) //from 1 to 0
                {
                    removeFromSelected.Execute(this);
                }
                else if (Count > 0 && initialValue <= 0) //from 0 to 1
                {
                    addToSelected.Execute(this);
                }
                OnPropertyChanged();
                OnPropertyChanged("Price");
                OnPropertyChanged("CountView");
                OnPropertyChanged("SumPrice");
                refreshAllData();
            }
        }

        public string CountView
        {
            get 
            {
                if (Count > 0) return Count.ToString();
                return Product.Price.ToString();
            }
        }

        public bool OneOrMore { get => Count > 0; }
        public bool BoolDiscount { get => Product.Discount != null; }

        public decimal SumPrice { get => Count * Price; }

        public decimal OldPrice { get => Product.Price; }

        public decimal Price { get => Product.Discount != null ? Product.Price - (Product.Price / 100 * Product.Discount.Value) : Product.Price; }

        public Command AddCount { get; set; }
        public Command AddToBasket { get; set; }

        public Command SubCount { get; set; }

        private Command addToSelected;
        private Command removeFromSelected;

        private int minCount = 1;
        private int maxCount = 99;

        #endregion

        public ProductLocal(Product _product, Action _refreshAllData, Command _addToSelected, Command _removeFromSelected, Command _addToBasket)
        {
            Product = _product;
            AddToBasket = _addToBasket;
            refreshAllData = _refreshAllData;
            Image = new UriImageSource
            {
                Uri = new Uri(ApiStrings.HOST_ADMIN + ApiStrings.IMAGES_FOLDER + Product.Image),
                CachingEnabled = true,
                CacheValidity = Caches.IMAGE_CACHE.lifeTime
            };
            AddCount = new Command(() => 
            {
                if (Count < maxCount) Count++;
            });
            SubCount = new Command(() =>
            {
                if (Count > minCount) Count--;
            });

            addToSelected = _addToSelected;
            removeFromSelected = _removeFromSelected;
        }
    }
}
