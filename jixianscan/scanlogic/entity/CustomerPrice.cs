using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scanlogic
{
    public class CustomerPrice
    {
        /// <summary>
        /// 客户
        /// </summary>
        public string Customer { get; set; }
        /// <summary>
        /// 打印类型
        /// </summary>
        public string PrintType { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        public double UnitPrice { get; set; }
    }
}
