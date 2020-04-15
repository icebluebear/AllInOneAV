using Model.ScanModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;

namespace Utils
{
    public class CacheTools
    {
        /// <summary>
        /// 插入缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="absoluteExpiration">过期时间</param>
        public static void CacheInsert(string key, object value, System.DateTime absoluteExpiration)
        {
            if (value != null)
            {
                HttpRuntime.Cache.Insert(key, value, null, absoluteExpiration, Cache.NoSlidingExpiration);
            }
        }

        /// <summary>
        /// 插入缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="slidingExpiration">相对过期时间间隔</param>
        public static void CacheInsert(string key, object value, TimeSpan slidingExpiration)
        {
            if (value != null)
            {
                HttpRuntime.Cache.Insert(key, value, null, Cache.NoAbsoluteExpiration, slidingExpiration);
            }
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="absoluteExpiration"></param>
        public static T GetCache<T>(string key)
        {
            if (HasCache(key))
            {
                return (T)HttpRuntime.Cache[key];
            }
            return default(T);
        }

        public static T GetCache<T>(string cacheKey, Func<T> func, System.DateTime absoluteExpiration) where T : new()
        {
            var objectT = GetCache<T>(cacheKey);
            if (objectT == null)
            {
                objectT = func();
                CacheTools.CacheInsert(cacheKey, objectT, absoluteExpiration);
            }
            return objectT;
        }

        public static TResult GetCache<T1, TResult>(string cacheKey, Func<T1, TResult> func, T1 p1, System.DateTime absoluteExpiration) where TResult : new()
        {
            var objectT = GetCache<TResult>(cacheKey);
            if (objectT == null)
            {
                objectT = func(p1);
                CacheInsert(cacheKey, objectT, absoluteExpiration);
            }
            return objectT;
        }

        public static TResult GetCache<T1, T2, TResult>(string cacheKey, Func<T1, T2, TResult> func, T1 p1, T2 p2, System.DateTime absoluteExpiration) where TResult : new()
        {
            var objectT = GetCache<TResult>(cacheKey);
            if (objectT == null)
            {
                objectT = func(p1, p2);
                CacheInsert(cacheKey, objectT, absoluteExpiration);
            }
            return objectT;
        }

        public static TResult GetCache<T1, T2, T3, TResult>(string cacheKey, Func<T1, T2, T3, TResult> func, T1 p1, T2 p2, T3 p3, System.DateTime absoluteExpiration) where TResult : new()
        {
            var objectT = GetCache<TResult>(cacheKey);
            if (objectT == null)
            {
                objectT = func(p1, p2, p3);
                CacheInsert(cacheKey, objectT, absoluteExpiration);
            }
            return objectT;
        }

        /// <summary>
        /// 缓存是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool HasCache(string key)
        {
            return HttpRuntime.Cache[key] != null;
        }

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static void CacheRemove(string key)
        {
            HttpRuntime.Cache.Remove(key);
        }
    }
}
