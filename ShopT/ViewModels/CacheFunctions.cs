using Akavache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace ShopT.ViewModels
{
    /// <summary>
    /// Содержит ряд функций для взаимодействия с кэшем
    /// </summary>
    public class CacheFunctions
    {
        private readonly List<IBlobCache> caches = new List<IBlobCache>() { BlobCache.Secure, 
                                                                            BlobCache.LocalMachine, 
                                                                            BlobCache.UserAccount };
        public enum BlobCaches
        {
            Secure,
            LocalMachine,
            UserAccount
        }

        /// <summary>
        /// Проверяет наличие ключей в secure кэше и исправляет их отсутствие
        /// </summary>
        public async Task<bool> firstTimeLaunchCheck()
        {
            var cacheKeys = await BlobCache.Secure.GetAllKeys();
            if (!cacheKeys.Contains("token"))
            {
                await BlobCache.Secure.InsertObject<string>("token", null);
            }
            return true;
        }

        /// <summary>
        /// Пытается достать данные из ячейки кэша, 
        /// если ячейки не существовало - создает новую
        /// </summary>
        /// <param name="key">Ключ от, предположительно, существующей ячейки кэша</param>
        /// <returns>Полученное значение или значение по-умолчанию для типа T</returns>
        public async Task<T> tryToGet<T>(string key, BlobCaches cache)
        {
            IBlobCache blobCache = caches[(int)cache];

            //Если ячейка кэша существует - возвратить значение
            try
            {
                return await blobCache.GetObject<T>(key);
            }
            catch (Exception)
            {
                await blobCache.InsertObject<T>(key, default(T));
                return default(T);
            }
        }
    }
}
