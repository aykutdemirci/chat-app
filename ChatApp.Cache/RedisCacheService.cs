using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;

namespace ChatApp.Cache
{
    public class RedisCacheService : ICacheManager
    {
        private readonly IDistributedCache _distributedCache;
        private readonly DistributedCacheEntryOptions cacheOptions;

        public RedisCacheService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
            cacheOptions = new DistributedCacheEntryOptions().SetAbsoluteExpiration(DateTime.Now.AddMinutes(10)).SetSlidingExpiration(TimeSpan.FromMinutes(2));
        }

        /// <summary>
        /// Key değeri verilen kaynağı önbellekten okur
        /// </summary>
        /// <typeparam name="T">Kaynağın tipi</typeparam>
        /// <param name="key">Ön bellekten okunacak kaynağın key'i</param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            var cachedString = _distributedCache.GetString(key);
            if (!string.IsNullOrEmpty(cachedString))
            {
                return JsonConvert.DeserializeObject<T>(cachedString);
            }

            return Activator.CreateInstance<T>();
        }

        /// <summary>
        /// Verilen key üzerinden önbelleğe kaynak ekler
        /// </summary>
        /// <typeparam name="T">Eklenecek kaynağın tipi</typeparam>
        /// <param name="key">Eklenecek kaynağın key'i</param>
        /// <param name="model">Kaynak tipinden örnek</param>
        public void Set<T>(string key, T model)
        {
            var stringToCache = JsonConvert.SerializeObject(model);
            _distributedCache.SetString(key, stringToCache, cacheOptions);
        }

        /// <summary>
        /// Key'i verilen kaynağı önbellekten siler
        /// </summary>
        /// <param name="key">Önbellekten silinecek kaynağın key'i</param>
        public void Remove(string key)
        {
            _distributedCache.Remove(key);
        }
    }
}
