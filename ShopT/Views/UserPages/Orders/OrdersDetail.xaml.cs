
using ShopT.Models.LocalModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShopT.Views.UserPages.Orders
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrdersDetail : ContentPage
    {
        public OrdersDetail(OrderLocal order)
        {
            InitializeComponent();
            //ItemsCollection.BindingContext = new ItemViewModel();
            //SpanSum.Text = order.Order.Sum.ToString();
        }

    }
}