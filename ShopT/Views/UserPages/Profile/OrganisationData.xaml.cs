using ShopT.StaticValues;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShopT.Views.UserPages.Profile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrganisationData : ContentPage
    {
        public OrganisationData()
        {
            InitializeComponent();

            BindingContext = ShopInfoStatic.shopInfo;
        }
    }
}