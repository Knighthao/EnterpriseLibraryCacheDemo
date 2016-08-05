using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseLibraryCacheDemo
{
    public class BusinessService
    {
        public static List<FilterEntity> GetAllFilter()
        {
            var result = new List<FilterEntity>();
            for (int i = 0; i < 10000; i++)
            {
                result.Add(new FilterEntity() { CityID = i, FilterID = i, FilterName = i.ToString() });
            }
            return result;
        }
    }
}
