using System;
namespace jixian.Entity
{
    public partial class PrintType
    {
        /// <summary>
        /// 
        /// <summary>
        public int Id { get; set; }
        /// <summary>
        /// 
        /// <summary>
        public string TypeName { get; set; }

        /// <summary>
        /// �ؼ��� ��"|"�ָ�
        /// </summary>
        public string Keyword { get; set; }

        public int SortIndex { get; set; }
        /// <summary>
        /// 
        /// <summary>
        public decimal DefaultUnitPrice { get; set; }
    }
}
