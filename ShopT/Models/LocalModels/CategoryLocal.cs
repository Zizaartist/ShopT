using ShopT.StaticValues;
using System;
using Xamarin.Forms;

namespace ShopT.Models.LocalModels
{
    public class CategoryLocal
    {
        public CategoryLocal(Category _category) 
        {
            Category = _category;

            Image = Category.Image != null ? new UriImageSource
            {
                Uri = new Uri(ApiStrings.HOST_ADMIN + ApiStrings.IMAGES_FOLDER + Category.Image),
                CachingEnabled = true,
                CacheValidity = Caches.IMAGE_CACHE.lifeTime
            } : null;
        }

        public Category Category { get; private set; }
        public UriImageSource Image { get; private set; }
    }
}
