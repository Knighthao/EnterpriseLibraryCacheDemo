using Microsoft.Practices.EnterpriseLibrary.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseLibraryCacheDemo
{
   
    /// <summary> 
    /// 自定义缓存刷新操作 
    /// </summary> 
    [Serializable]
    public class FilterCacheRefreshAction : ICacheItemRefreshAction
    {
        #region ICacheItemRefreshAction 成员
        /// <summary> 
        /// 自定义刷新操作 
        /// </summary> 
        /// <param name="removedKey">移除的键</param> 
        /// <param name="expiredValue">过期的值</param> 
        /// <param name="removalReason">移除理由</param> 
        void ICacheItemRefreshAction.Refresh(string removedKey, object expiredValue, CacheItemRemovedReason removalReason)
        {
            if (removalReason == CacheItemRemovedReason.Expired)
            {
                List<FilterEntity> orderFilterlist = BusinessService.GetAllFilter();

                ICacheItemRefreshAction refreshAction = new FilterCacheRefreshAction();
                CacheHelper.Add(removedKey, orderFilterlist, refreshAction, ConstantDataManager.GetExpiredSeconds(), true);

            }
        }

        #endregion
    }
}
