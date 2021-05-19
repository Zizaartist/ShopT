using Akavache;
using ShopT.Models.Statistics;
using ShopT.Views.Registration;
using ShopT.Views.UserPages.ShopsPage;
using Xamarin.Forms;

namespace ShopT
{
    public partial class App : Application
    {
        public Style payment, paymentSelected;
        public bool subcategory = true, Sale = true;
        public App()
        {
            InitializeComponent();
            InitializationPublicStyles();

            BlobCache.ApplicationName = "ShopT";
            BlobCache.EnsureInitialized();

            MainPage = new ShopsShell();
        }
        void InitializationPublicStyles()
        {
            payment = Payment;
            paymentSelected = PaymentSelected;
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
