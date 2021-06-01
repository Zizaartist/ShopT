using ShopT.Models.EnumModels;
using ShopT.ResourceDictionary;
using ShopT.StaticValues;
using System.Collections.Generic;

namespace ShopT.ViewModels.Templates
{
    public class ThemeManager
    {
        /// <summary>
        /// Обновляет тему в соответсвии с текущей конфигурацией магазина
        /// Если конфигурации нет, то ставит по умолчанию
        /// </summary>
        public static void ChangeTheme() 
        {
            var chosenTheme = ShopInfoStatic.shopConfiguration?.Theme ?? Theme.sbie;

            ICollection<Xamarin.Forms.ResourceDictionary> mergedDictionaries = App.Current.Resources.MergedDictionaries;
            if (mergedDictionaries != null)
            {
                mergedDictionaries.Clear();

                switch (chosenTheme)
                {
                    case Theme.simple:
                        mergedDictionaries.Add(new SimpleTheme());
                        break;
                    case Theme.sbie:
                        mergedDictionaries.Add(new SbieTheme());
                        break;
                    case Theme.blue:
                        mergedDictionaries.Add(new BlueTheme());
                        break;
                }
            }
        }
    }
}
