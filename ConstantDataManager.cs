using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
namespace EnterpriseLibraryCacheDemo
{
    public class ConstantDataManager
    {
        public static string CouponCacheKey = "EnterpriseLibraryCacheDemo.FilterCache";
        /// <summary>
        /// AppID
        /// </summary>
        public static String AppID
        {
            get
            {
                return ConfigurationManager.AppSettings["AppID"];
            }
        }

        /// <summary>
        /// 缓存一个小时过期
        /// </summary>
        public static int GetExpiredSeconds()
        {
            int expiredSeconds = 0;

            if (!int.TryParse(ConfigurationManager.AppSettings["ExpiredSeconds"], out expiredSeconds))
            {
                expiredSeconds = 3600;
            }

            return expiredSeconds;
        }
    }
}
