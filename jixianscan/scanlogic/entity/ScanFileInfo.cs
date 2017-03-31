using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scanlogic
{
    public class ScanFileInfo
    {
        /// <summary>
        /// 客户
        /// </summary>
        public string Customer { get; set; }
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 打印类型
        /// </summary>
        public string PrintType { get; set; }
        /// <summary>
        /// 完整路径
        /// </summary>
        public string FullPath { get; set; }
        /// <summary>
        /// 长度
        /// </summary>
        public double Length { get; set; }
        /// <summary>
        /// 宽度
        /// </summary>
        public double Width { get; set; }
        /// <summary>
        /// 面积
        /// </summary>
        public double Area { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        public double UnitPrice { get; set; }
        /// <summary>
        /// 合计总价
        /// </summary>
        public double TotalAmount { get; set; }


    }
}
