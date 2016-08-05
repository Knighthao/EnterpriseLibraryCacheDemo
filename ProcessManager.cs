using Microsoft.Practices.EnterpriseLibrary.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseLibraryCacheDemo
{
 public   class ProcessManager
    {
        /// <summary> 
        /// 从缓存中获取配置参数 
        /// </summary> 
        /// <returns></returns> 
     public List<FilterEntity> GetAllOrderFilterForCoupon()
        {
            var cacheValue = CacheHelper.GetCache(ConstantDataManager.CouponCacheKey);
            if (!CacheHelper.ContainsKey(ConstantDataManager.CouponCacheKey))
            {
                List<FilterEntity> orderFilterlist = BusinessService.GetAllFilter();
                ICacheItemRefreshAction refreshAction = new FilterCacheRefreshAction();
                CacheHelper.Add(ConstantDataManager.CouponCacheKey, orderFilterlist, refreshAction, ConstantDataManager.GetExpiredSeconds(), true);

                return orderFilterlist;
            }
            else
            {
                return cacheValue as List<FilterEntity>;
            }
        }

     public List<FilterEntity> RefreshCouponCache()
        {
            List<FilterEntity> orderFilterlist = BusinessService.GetAllFilter();
            ICacheItemRefreshAction refreshAction = new FilterCacheRefreshAction();
            CacheHelper.Add(ConstantDataManager.CouponCacheKey, orderFilterlist, refreshAction, ConstantDataManager.GetExpiredSeconds(), true);

            return GetAllOrderFilterForCoupon();
        }
    }
}
