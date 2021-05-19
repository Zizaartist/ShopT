
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
[assembly: ExportFont("NeoSansPro-Regular.ttf", Alias = "NeoSansPro")]
[assembly: ExportFont("NeoSansPro-Medium.ttf", Alias = "NeoSansProBold")]

namespace ShopT.ResourceDictionary
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BlueTheme
    {
        public BlueTheme()
        {
            InitializeComponent();
        }
    }
}