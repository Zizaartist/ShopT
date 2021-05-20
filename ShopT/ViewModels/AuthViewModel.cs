using Akavache;
using Newtonsoft.Json;
using ShopT.Models.Statistics;
using ShopT.StaticValues;
using System;
using System.Net.Http;
using System.Reactive.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ShopT.ViewModels
{
    public class AuthViewModel : ViewModel
    {

        #region properties

        private bool codeIsValid;
        public bool CodeIsValid 
        {
            get => codeIsValid;
            set 
            {
                if (codeIsValid == value) return;
                codeIsValid = value;
                OnPropertyChanged("AllFieldsValid");
            }
        }

        private string phone;
        public string Phone
        {
            get => phone;
            set
            {
                if (phone == value) return;
                phone = value;
                OnPropertyChanged("IsValidNumber");
            }
        }

        private string code;
        public string Code
        {
            get => code;
            set
            {
                if (code == value) return;
                code = value;
                OnPropertyChanged("AllFieldsValid");
            }
        }

        private bool agreement1;
        public bool Agreement1 
        {
            get => agreement1;
            set 
            {
                if (agreement1 == value) return;
                agreement1 = value;
                OnPropertyChanged("AllFieldsValid");
            }
        }

        private bool agreement2;
        public bool Agreement2
        {
            get => agreement2;
            set
            {
                if (agreement2 == value) return;
                agreement2 = value;
                OnPropertyChanged("AllFieldsValid");
            }
        }

        public bool IsValidNumber 
        { 
            get 
            {
                if (Phone == null) return false;
                return Regex.Match(Phone, @"^((8|\+7|7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?[\d\- ]{7,10}$").Success;
            }
        }

        public bool AllFieldsValid
        {
            get
            {
                return IsValidNumber && agreement1 && agreement2;
            }
        }

        #endregion

        public async Task<HttpResponseMessage> SmsCheck()
        {
            try
            {
                CodeIsValid = false;
                var client = HttpClientSingleton.Instance;
                var response = await client.PostAsync(ApiStrings.HOST + ApiStrings.ACCOUNT_SMS_CHECK + "?phone=" + Phone, null);
                return response;
            }
            catch (Exception e)
            {
                throw CheckIfConnectionException(e);
            }
        }

        public async Task<HttpResponseMessage> CodeCheck()
        {
            try
            {
                var client = HttpClientSingleton.Instance;
                var response = await client.PostAsync($"{ApiStrings.HOST}{ApiStrings.ACCOUNT_CODE_CHECK}?code={Code}&phone={Phone}", null);
                return response;
            }
            catch (Exception e)
            {
                throw CheckIfConnectionException(e);
            }
        }

        public async Task<HttpResponseMessage> GetToken()
        {
            try
            {
                HttpClient client = HttpClientSingleton.Instance;

                var response = await client.PostAsync($"{ApiStrings.HOST}{ApiStrings.ACCOUNT_USERS_TOKEN}?phone={Phone}&code={Code}", null);

                if (response.IsSuccessStatusCode) 
                {
                    var template = new { access_token = "", username = "" };
                    string result = await response.Content.ReadAsStringAsync();
                    var tokenModel = JsonConvert.DeserializeAnonymousType(result, template);
                    await BlobCache.Secure.InsertObject($"{ShopInfoStatic.shopInfo.ShopId}_{Caches.TOKEN_CACHE.key}", tokenModel.access_token);
                    await BlobCache.Secure.InsertObject($"{ShopInfoStatic.shopInfo.ShopId}_{Caches.TOKENTYPE_CACHE.key}", "User");
                }
                return response;
            }
            catch (Exception e)
            {
                throw CheckIfConnectionException(e);
            }
        }

        public async Task<HttpResponseMessage> Validation()
        {
            try
            {
                HttpClient client = await createUserClient();
                return await client.GetAsync($"{ApiStrings.HOST}{ ApiStrings.ACCOUNT_VALIDATE}");
            }
            catch (Exception e)
            {
                throw CheckIfConnectionException(e);
            }
        }

        public async Task GetDefaultToken()
        {
            try
            {
                HttpClient client = await createUserClient();
                var response = await client.GetAsync($"{ApiStrings.HOST}{ ApiStrings.ACCOUNT_DEFAULT_TOKEN}");

                if (response.IsSuccessStatusCode)
                {
                    var template = new { access_token = "", username = "" };
                    string result = await response.Content.ReadAsStringAsync();
                    var tokenModel = JsonConvert.DeserializeAnonymousType(result, template);
                    await BlobCache.Secure.InsertObject($"{ShopInfoStatic.shopInfo.ShopId}_{Caches.TOKEN_CACHE.key}", tokenModel.access_token);
                    await BlobCache.Secure.InsertObject($"{ShopInfoStatic.shopInfo.ShopId}_{Caches.TOKENTYPE_CACHE.key}", "Default");
                }
            }
            catch (Exception e)
            {
                throw CheckIfConnectionException(e);
            }
        }
    }
}
