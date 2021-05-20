using Akavache;
using MvvmHelpers;
using ShopT.Models;
using ShopT.Models.EnumModels;
using ShopT.Models.LocalModels;
using ShopT.Models.Statistics;
using ShopT.StaticValues;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using static ShopT.Models.EnumModels.ChangeBanknoteDictionaries;

namespace ShopT.ViewModels
{
    class OrderViewModel : ViewModel
    {
        #region properties

        private string name;
        public string Name
        {
            get => name;
            set
            {
                SetProperty(ref name, value);
                OnPropertyChanged("ValidTakeaway");
                OnPropertyChanged("ValidDelivery");
            }
        }

        //nullable
        private string commentary;
        public string Commentary
        {
            get => commentary;
            set { SetProperty(ref commentary, value); }
        }

        private PaymentMethod paymentMethod;
        public PaymentMethod PaymentMethod
        {
            get => paymentMethod;
            set
            {
                SetProperty(ref paymentMethod, value);
                OnPropertyChanged("ValidTakeaway");
                OnPropertyChanged("ValidDelivery");
            }
        }

        public ToggleableModel<PaymentMethod> PaymentMethods { get; } = new ToggleableModel<PaymentMethod>();

        private bool usePoints = false;
        public bool UsePoints 
        {
            get => usePoints;
            set 
            {
                SetProperty(ref usePoints, value);
                OnPropertyChanged("ValidTakeaway");
                OnPropertyChanged("ValidDelivery");
                OnPropertyChanged("TotalSum");
            }
        }

        private string street;
        public string Street 
        {
            get => street;
            set 
            {
                SetProperty(ref street, value);
                OnPropertyChanged("ValidDelivery");
            }
        }

        private string house;
        public string House 
        {
            get => house;
            set 
            {
                SetProperty(ref house, value);
                OnPropertyChanged("ValidDelivery");
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

        public bool ValidTakeaway 
        {
            get 
            {
                if (!string.IsNullOrEmpty(Name))
                {
                    return true;
                }
                return false;
            }
        }

        public bool ValidDelivery 
        {
            get 
            {
                if (ValidTakeaway &&
                    !string.IsNullOrEmpty(Street) &&
                    !string.IsNullOrEmpty(House)) 
                {
                    return true;
                }
                return false;
            }
        }

        private BanknotePair selectedBanknote;
        public BanknotePair SelectedBanknote 
        {
            get => selectedBanknote;
            set { SetProperty(ref selectedBanknote, value); }
        }

        public decimal InitialSum { get; }

        public int Percent { get; }

        public decimal Saving { get; }

        public decimal TotalSum { get => UsePoints ? InitialSum - Saving : InitialSum; }

        private List<OrderDetail> orderDetails;
        private bool delivery;

        public Command ChangePaymentMethod { get; }

        #endregion

        public OrderViewModel(IEnumerable<OrderDetailLocal> _orderDetails, bool _delivery)
        {

            orderDetails = new List<OrderDetail>();
            _orderDetails.ForEach(detail => 
            {
                detail.OrderDetail.Product = null;
                orderDetails.Add(detail.OrderDetail);
            });
            delivery = _delivery;

            InitialSum = _orderDetails.Sum(detail => detail.SumPrice);

            Percent = ShopInfoStatic.shopConfiguration.MaxPoints;

            var saved = InitialSum * (Percent / 100m);

            Saving = saved < UsersViewModel.Instance.Points ? UsersViewModel.Instance.Points : saved;

            PaymentMethods.ReplaceRangeWithInject(ShopInfoStatic.shopConfiguration.ActualPaymentMethods);
        }

        public async Task<HttpResponseMessage> PostOrder()
        {
            try
            {
                HttpClient client = await createUserClient();

                var newOrder = delivery ? ConstructDeliveryOrder() : ConstructTakeawayOrder();

                var serializedObj = SerializeIgnoreNull(newOrder);
                var data = new StringContent(serializedObj, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(ApiStrings.HOST + ApiStrings.ORDERS_CONTROLLER, data);
                if (response.IsSuccessStatusCode) 
                {
                    await SaveAutofill();
                    await UsersViewModel.Instance.UpdatePoints.ExecuteAsync();
                }
                return response;
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

        private Order ConstructDeliveryOrder() 
        {
            return new Order()
            {
                PointsUsed = UsePoints,
                Delivery = true,
                OrderInfo = new OrderInfo()
                {
                    Apartment = Apartment,
                    Commentary = Commentary,
                    Entrance = Entrance,
                    Floor = Floor,
                    House = House,
                    OrdererName = Name,
                    Street = Street
                },
                ChangeBanknote = PaymentMethod == PaymentMethod.cash ? SelectedBanknote?.ChangeBanknote : null,
                PaymentMethod = PaymentMethod,
                OrderDetails = orderDetails
            };
        }

        private Order ConstructTakeawayOrder() 
        {
            return new Order()
            {
                PointsUsed = UsePoints,
                Delivery = false,
                OrderInfo = new OrderInfo()
                {
                    Commentary = Commentary,
                    OrdererName = Name
                },
                ChangeBanknote = PaymentMethod == PaymentMethod.cash ? SelectedBanknote?.ChangeBanknote : null,
                PaymentMethod = PaymentMethod,
                OrderDetails = orderDetails
            };
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
