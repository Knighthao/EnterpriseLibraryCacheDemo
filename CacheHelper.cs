using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseLibraryCacheDemo
{
    /*
        *在Caching Application Block中，主要提供以下四种保存缓存数据的途径，
        *分别是：内存存储（默认）、独立存储（Isolated Storage）、
        *数据库存储（DataBase Cache Storage）和自定义存储（Custom Cache Storage）。
        *In-Memory：保存在内存中。   
        *Isolated Storage Cache Store：系统将缓存的信息保存在独立文件中（C:\Users\<<user name>>\AppData\Local\IsolatedStorage）。
        *Data Cache Storage：将缓存数据保存在数据库中。（需要运行CreateCachingDatabase.sql脚本）
        *Custom Cache Storage：自己扩展的处理器。我们可以将数据保存在注册表中或文本文件中。
        *
        * 缓存等级，在企业库的缓存模块中已经提供了4个缓存等级：Low，Normal，High和NotRemovable，在超出最大缓存数量后会自动根据缓存等级来移除对象。
        * 过期方式，企业库默认提供4种过期方式
        * AbsoluteTime：绝对是时间过期，传递一个时间对象指定到时过期
        * SlidingTime：缓存在最后一次访问之后多少时间后过期，默认为2分钟，有2个构造函数可以指定一个过期时间或指定一个过期时间和一个最后使用时
        * ExtendedFormatTime ：指定过期格式，以特定的格式来过期，通过ExtendedFormat.cs类来包装过期方式，具体可参照ExtendedFormat.cs，源代码中已经给出了很多方式
        * FileDependency：依赖于文件过期，当所依赖的文件被修改则过期，这个我觉得很有用，因为在许多网站，如论坛、新闻系统等都需要大量的配置，可以将配置文件信息进行缓存，将依赖项设为配置文件，这样当用户更改了配置文件后通过ICacheItemRefreshAction.Refresh可以自动重新缓存。
        */
    public class CacheHelper
    {
        //2种建立CacheManager的方式
        //static ICacheManager cache = EnterpriseLibraryContainer.Current.GetInstance<ICacheManager>();
        private static ICacheManager cache = CacheFactory.GetCacheManager("Cache Manager");
        private static object locker = new object();

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="cacheItemRefreshAction"></param>
        /// <param name="expiredSeconds"></param>
        /// <param name="isRefresh"></param>
        public static void Add(string key, object value, ICacheItemRefreshAction cacheItemRefreshAction, int expiredSeconds, bool isRefresh = false)
        {
            lock (locker)
            {
                if (isRefresh)
                {
                    //自定义刷新方式,如果过期将自动重新加载,过期时间为5分钟
                    cache.Add(key, value, CacheItemPriority.Normal, cacheItemRefreshAction, new AbsoluteTime(TimeSpan.FromSeconds(expiredSeconds)));
                }
                else
                {
                    cache.Add(key, value);
                }
            }
        }
        /// <summary>
        /// 获取缓存对象
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public static object GetCache(string key)
        {
            return cache.GetData(key);
        }

        /// <summary>
        /// 检查是否有某个缓存项
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool ContainsKey(string key)
        {
            return cache.Contains(key);
        }

        #region
        /// <summary>
        /// 向缓存中添加一个对象。
        /// </summary>
        /// <param name="key">缓存的键值，该值通常是使用缓存机制的方法的名称。</param>
        /// <param name="valKey">缓存值的键值，该值通常是由使用缓存机制的方法的参数值所产生。</param>
        /// <param name="value">需要缓存的对象。</param>
        public void Add(string key, string valKey, object value)
        {
            Dictionary<string, object> dict = null;
            if (cache.Contains(key))
            {
                dict = (Dictionary<string, object>)cache[key];
                dict[valKey] = value;
            }
            else
            {
                dict = new Dictionary<string, object>();
                dict.Add(valKey, value);
            }
            cache.Add(key, dict);
        }
        /// <summary>
        /// 向缓存中更新一个对象。
        /// </summary>
        /// <param name="key">缓存的键值，该值通常是使用缓存机制的方法的名称。</param>
        /// <param name="valKey">缓存值的键值，该值通常是由使用缓存机制的方法的参数值所产生。</param>
        /// <param name="value">需要缓存的对象。</param>
        public void Put(string key, string valKey, object value)
        {
            Add(key, valKey, value);
        }
        /// <summary>
        /// 从缓存中读取对象。
        /// </summary>
        /// <param name="key">缓存的键值，该值通常是使用缓存机制的方法的名称。</param>
        /// <param name="valKey">缓存值的键值，该值通常是由使用缓存机制的方法的参数值所产生。</param>
        /// <returns>被缓存的对象。</returns>
        public object Get(string key, string valKey)
        {
            if (cache.Contains(key))
            {
                Dictionary<string, object> dict = (Dictionary<string, object>)cache[key];
                if (dict != null && dict.ContainsKey(valKey))
                    return dict[valKey];
                else
                    return null;
            }
            return null;
        }
        /// <summary>
        /// 从缓存中移除对象。
        /// </summary>
        /// <param name="key">缓存的键值，该值通常是使用缓存机制的方法的名称。</param>
        public void Remove(string key)
        {
            cache.Remove(key);
        }
        /// <summary>
        /// 获取一个<see cref="Boolean"/>值，该值表示拥有指定键值的缓存是否存在。
        /// </summary>
        /// <param name="key">指定的键值。</param>
        /// <returns>如果缓存存在，则返回true，否则返回false。</returns>
        public bool Exists(string key)
        {
            return cache.Contains(key);
        }
        /// <summary>
        /// 获取一个<see cref="Boolean"/>值，该值表示拥有指定键值和缓存值键的缓存是否存在。
        /// </summary>
        /// <param name="key">指定的键值。</param>
        /// <param name="valKey">缓存值键。</param>
        /// <returns>如果缓存存在，则返回true，否则返回false。</returns>
        public bool Exists(string key, string valKey)
        {
            return cache.Contains(key) &&
                ((Dictionary<string, object>)cache[key]).ContainsKey(valKey);
        }
        #endregion
    }
}