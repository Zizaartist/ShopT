using ShopT.Models.EnumModels;
using ShopT.Models.HubModels;
using ShopT.StaticValues;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ShopT.Models.LocalModels
{
    public class ShopLocal
    {
        public ShopLocal(Shop _shop)
        {
            Shop = _shop;

            Image = _shop.ShopInfo.Avatar != null ? new UriImageSource
            {
                Uri = new Uri(ApiStrings.SHOPT_HUB + ApiStrings.IMAGES_FOLDER + _shop.ShopInfo.Avatar),
                CachingEnabled = true,
                CacheValidity = Caches.IMAGE_CACHE.lifeTime
            } : null;

            BannerImage = _shop.ShopInfo.Banner != null ? new UriImageSource
            {
                Uri = new Uri(ApiStrings.SHOPT_HUB + ApiStrings.IMAGES_FOLDER + _shop.ShopInfo.Banner),
                CachingEnabled = true,
                CacheValidity = Caches.IMAGE_CACHE.lifeTime
            } : null;
        }

        public Shop Shop { get; private set; }
        public ImageSource Image { get; private set; }
        public ImageSource BannerImage { get; private set; }
    }
}
