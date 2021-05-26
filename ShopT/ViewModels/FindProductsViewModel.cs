using System;
using Akavache;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using System.Reactive.Linq;
using Xamarin.Forms;
using MvvmHelpers;
using ShopT.Models.LocalModels;
using ShopT.Models;
using ShopT.StaticValues;
using ShopT.Views.UserPages.Main;

namespace ShopT.ViewModels
{
    public class FindProductsViewModel : ViewModel
    {
        #region properties



        private ObservableCollection<string> filters;
        public ObservableCollection<string> Filters
        {
            get => filters;
            set
            {
                if (filters != value)
                {
                    filters = value;
                    OnPropertyChanged("");
                }
                OnPropertyChanged();
            }
        }

        public ObservableRangeCollection<ProductLocal> ProductListsTemp { get; } = new ObservableRangeCollection<ProductLocal>();
        public ObservableRangeCollection<ProductLocal> ProductLists { get; } = new ObservableRangeCollection<ProductLocal>();

        public ObservableCollection<ProductLocal> SelectedProducts { get; } = new ObservableCollection<ProductLocal>();

        public ProductLocal SelectedProduct { get; set; }
        public ProductLocal ProductSetter
        {
            get => null;
            set 
            {
                if (SelectedProduct != value)
                {
                    SelectedProduct = value;
                    OnPropertyChanged("SelectedProduct");
                } 
                OnPropertyChanged(); //Всегда сбрасывает обратно на null
            }
        }

        public int AddToCartCount { get => ProductLists.Sum(e => e.Count); }

        public bool AddToCartNotEmpty { get => SelectedProducts.Any(); }

        public decimal SumTotal { get => ProductLists.Sum(e => e.SumPrice); }

        private string ByName;

        public Command AddToBasket { get; }
        public Command AddToSelected { get; }
        public Command RemoveFromSelected { get; }

        #endregion

        #region methods

        public FindProductsViewModel(Command _addToBasket = null)
        {
            Filters = new ObservableCollection<string>();

            ProductLists.CollectionChanged += (sender, e) => UpdateBindings();
            SelectedProducts.CollectionChanged += (sender, e) => 
            {
                OnPropertyChanged("AddToCartNotEmpty");
                if (!AddToCartNotEmpty)
                {
                    foreach (var product in ProductLists)
                    {
                        product.Count = 0;
                    }
                }
            };

            AddToBasket = _addToBasket;

            AddToSelected = new Command((param) => SelectedProducts.Add(param as ProductLocal));
            RemoveFromSelected = new Command((param) => SelectedProducts.Remove(param as ProductLocal));
        }

     

        public async Task GetRemoteData(string _NAME)
        {
            ProductListsTemp.Clear();
            ProductLists.Clear();
            Filters.Clear();

            try
            {
                ByName = _NAME;

                HttpClient client = await createUserClient();

                //Получение всех продуктов по ByName меню
                HttpResponseMessage response = await client.GetAsync($"{ApiStrings.HOST}{ApiStrings.PRODUCTS_GET_BY_NAME}{ByName}");

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    List<Product> tempList = JsonConvert.DeserializeObject<List<Product>>(result);

                    List<string> tempListCategory = new List<string>();

                    foreach (var item in tempList)
                    {
                        ProductListsTemp.Add(new ProductLocal(item, UpdateBindings, AddToSelected, RemoveFromSelected, AddToBasket));
                        ProductLists.Add(new ProductLocal(item, UpdateBindings, AddToSelected, RemoveFromSelected, AddToBasket));
                        tempListCategory.Add(item.CategoryName);
                    }

                    var tempFilters = tempListCategory.Distinct().ToList();

                    foreach (var item in tempFilters)
                    {
                        Filters.Add(item);
                    }
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
        }

        public void SortFilters(string _sort)
        {
            if(_sort == "По возрастанию цены")
            {
                var tempList = ProductLists.ToList();


                var sortedUsers = from u in tempList
                                  orderby u.Price
                                  select u;

                ProductLists.ReplaceRange(sortedUsers);
            }
            if(_sort == "По убыванию цены")
            {
                var tempList = ProductLists.ToList();


                var sortedUsers = from u in tempList
                                  orderby u.Price descending
                                  select u;

                ProductLists.ReplaceRange(sortedUsers);

            }
            if (_sort == "Скидка")
            {
                var tempList = ProductLists.ToList();

                ProductLists.Clear();
                Filters.Clear();

                List<string> tempListCategory = new List<string>();

                foreach (var item in tempList)
                {
                    if (item.BoolDiscount)
                    {
                        ProductLists.Add(item);
                        tempListCategory.Add(item.Product.CategoryName);
                    }
                }
                var tempFilters = tempListCategory.Distinct().ToList();

                foreach (var item in tempFilters)
                {
                    Filters.Add(item);
                }
            }
        }

        public void FilterPickerFilters(string _sort)
        {
            List<ProductLocal> tempList;

            if (ProductListsTemp.Count > ProductLists.Count)
            {
                tempList = ProductListsTemp.ToList();
            }
            else
            {
                tempList = ProductLists.ToList();
            }

            ProductLists.Clear();

            foreach (var item in tempList)
            {
                if(item.Product.CategoryName == _sort)
                {
                    ProductLists.Add(item);
                }
            }
        }
        public void UpdateBindings()
        {
            OnPropertyChanged("AddToCartCount");
            OnPropertyChanged("AddToCartNotEmpty");
            OnPropertyChanged("SumTotal");
        }

        #endregion
    }
}
