using System.Linq;

namespace Xamarin.Forms
{
    public static class NavigationExtension
    {
        /// <summary>
        /// Позволяет безопасно переместиться на любую страницу без переназначения mainPage
        /// </summary>
        /// <param name="targetPage">Желаемая страница</param>
        public static void teleportTo(this INavigation navigation, Page targetPage)
        {
            //Запоминаем текущую и корневую страницы
            var thisPage = navigation.NavigationStack[navigation.NavigationStack.Count - 1];
            var navigationPage = navigation.NavigationStack[0];

            //Удаляем все кроме тех двух
            var toRemove = navigation.NavigationStack.ToList();
            toRemove.Remove(thisPage);
            toRemove.Remove(navigationPage);

            foreach (var page in toRemove)
            {
                navigation.RemovePage(page);
            }

            //В случае если текущая страница была корневой и ничего не удалено - просто пуш
            if (thisPage.Equals(navigationPage))
            {
                navigation.PushAsync(targetPage, false);
            }
            else
            {
                //Ставим желаемую страницу позади текущей и закрываем текущую
                navigation.InsertPageBefore(targetPage, thisPage);
                navigation.PopAsync(false);
            }
        }
    }
}
