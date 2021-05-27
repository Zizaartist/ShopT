using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShopT.Views.UserPages.ShopsPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShopsShell
    {
        public ShopsShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("shops/locations", typeof(FindCity));
        }
    }
}