using ShopT.StaticValues;
using ShopT.ViewModels;
using ShopT.Views.UserPages.ShopsPage;
using System;
using System.Net;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShopT.Views.Registration
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartPage : ContentPage
    {
        private enum AuthResult
        {
            success,
            registration,
            error
        }
        public StartPage()
        {
            InitializeComponent();

            Task.Run(async () =>
            {
                switch (await Authorize())
                {
                    case AuthResult.success:
                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            Task.Run(() => UsersViewModel.Instance.UpdatePoints.ExecuteAsync());
                            App.Current.MainPage = new MainPage();
                        });
                        break;
                    case AuthResult.registration:
                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            Task.Run(() => new AuthViewModel().GetDefaultToken());
                            App.Current.MainPage = new MainPage();
                        });
                        break;
                    case AuthResult.error:
                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            DisplayAlert("Error", AlertMessages.UNEXPECTED_ERROR, "Понятно");
                        });
                        //Выход из приложения
                        break;
                }
            });
            //Task.Run(() => NewPage());
        }
        //async void NewPage()
        //{
        //    logo.Opacity = 0;
        //    await logo.FadeTo(1, 3000);
        //}

        private async Task<AuthResult> Authorize()
        {
            AuthViewModel authentificator = new AuthViewModel();
            CacheFunctions cacheManager = new CacheFunctions();

            try
            {
                if (await cacheManager.firstTimeLaunchCheck())
                {
                    var response = await authentificator.Validation();

                    if (!response.IsSuccessStatusCode)
                    {
                        switch (response.StatusCode)
                        {
                            case HttpStatusCode.Unauthorized:
                                {
                                    return AuthResult.registration;
                                }
                            default: return AuthResult.error; //Необработанная ошибка сервера
                        }
                    }
                    else
                    {
                        return AuthResult.success; //единственный случай успешного исхода
                    }
                }
                return AuthResult.error; //Необработанная ошибка кэша
            }
            catch (Exception e)
            {
                return AuthResult.error; //Необработанная ошибка
            }
        }
    }
}