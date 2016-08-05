using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseLibraryCacheDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("start");
            new ProcessManager().RefreshCouponCache();
            var result = new ProcessManager().GetAllOrderFilterForCoupon();
            Console.WriteLine(result.Count);
            Console.WriteLine("end");

            Console.ReadLine();
        }
    }
}
