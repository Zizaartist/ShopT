using ShopT.Models;
using ShopT.StaticValues;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ShopT.ViewModels
{
    public class ErrorReportViewModel : ViewModel
    {
        private string text;
        public string Text 
        {
            get => text;
            set { SetProperty(ref text, value); }
        }

        //Fire&forget
        public async Task PostReport() 
        {
            HttpClient client = await createUserClient();

            var errorReport = new ErrorReport()
            {
                Text = Text
            };

            var serializedObj = SerializeIgnoreNull(errorReport);
            var data = new StringContent(serializedObj, Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{ApiStrings.HOST}{ApiStrings.ERRORREPORTS_CONTROLLER}", data);
            if (response.IsSuccessStatusCode) Text = null;
        }
    }
}
