using System;

namespace ShopT.StaticValues
{
    /// <summary>
    /// Статическое хранилище ключей и их сроков жизни
    /// </summary>
    public static class Caches
    {
        //Secure
        /// <summary>
        /// Кэш для хранения номера телефона пользователя
        /// </summary>
        public static readonly Cache PHONE_CACHE = new Cache("phone");
        /// <summary>
        /// Кэш для хранения токена
        /// </summary>
        public static readonly Cache TOKEN_CACHE = new Cache("token");


        public static readonly Cache TOKENTYPE_CACHE = new Cache("tokenType");

        //UserAccount
        /// <summary>
        /// Кэш для хранения пользовательских данных
        /// </summary>
        public static readonly Cache AUTOFILL_CACHE = new Cache("User");
        /// <summary>
        /// Кэш для хранения деталей заказа в корзине покупок
        /// </summary>
        public static readonly Cache SHOPCART_CACHE = new Cache("Shopcart");
        /// <summary>
        /// Кэш для хранения категории брендов текущего авторизованного "администратора"
        /// </summary>
        public static readonly Cache CURRENT_ADMIN_CATEGORY = new Cache("Category");

        //LocalMachine
        /// <summary>
        /// Кэш для хранения типов продукции
        /// </summary>
        public static readonly Cache KINDS_CACHE = new Cache("Categories");
        /// <summary>
        /// Кэш для хранения хэштегов
        /// </summary>
        public static readonly Cache HASHTAGS_CACHE = new Cache("Hashtags");
        /// <summary>
        /// Кэш для хранения методов оплаты
        /// </summary>
        public static readonly Cache PAYMENTMETHODS_CACHE = new Cache("PaymentMethods");
        /// <summary>
        /// Кэш для хранения банкнот различных номиналов
        /// </summary>
        public static readonly Cache BANKNOTES_CACHE = new Cache("Banknotes");
        /// <summary>
        /// Кэш для хранения брендов внутри категории
        /// </summary>
        public static readonly Cache BRANDS_CACHE = new Cache("Brands", TimeSpan.FromHours(1));
        /// <summary>
        /// Кэш для хранения заказов пользователя
        /// </summary>
        public static readonly Cache ORDERS_CACHE = new Cache("Orders", TimeSpan.FromHours(1));
        /// <summary>
        /// Кэш для хранения категорий внутри бренда
        /// </summary>
        public static readonly Cache CATEGORIES_CACHE = new Cache("Menus", TimeSpan.FromHours(1));
        /// <summary>
        /// Кэш для хранения продуктов внутри меню
        /// </summary>
        public static readonly Cache PRODUCTS_CACHE = new Cache("Products", TimeSpan.FromHours(1));
        /// <summary>
        /// Кэш для хранения изображений
        /// </summary>
        public static readonly Cache IMAGE_CACHE = new Cache("Images", TimeSpan.FromDays(1));
    }

    public class Cache
    {
        public readonly string key;
        public readonly TimeSpan lifeTime;

        public static object ShopInfoStatis { get; internal set; }

        public Cache(string key, TimeSpan lifeTime) 
        {
            this.key = key;
            this.lifeTime = lifeTime;
        }

        public Cache(string key)
        {
            this.key = key;
        }
    }
}
