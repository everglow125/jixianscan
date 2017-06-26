using System;
namespace jixian.Entity
{
	public partial class OrderInfo
	{
		/// <summary>
		/// 
		/// <summary>
		public int Id { get; set; }
		/// <summary>
		/// 
		/// <summary>
		public int CustomerId { get; set; }
		/// <summary>
		/// 
		/// <summary>
		public string CustomerName { get; set; }
		/// <summary>
		/// 
		/// <summary>
		public DateTime PrintDate { get; set; }
		/// <summary>
		/// 
		/// <summary>
		public string PrintType { get; set; }
		/// <summary>
		/// 
		/// <summary>
		public double Length { get; set; }
		/// <summary>
		/// 
		/// <summary>
		public double Width { get; set; }
		/// <summary>
		/// 
		/// <summary>
		public int Quantity { get; set; }
		/// <summary>
		/// 
		/// <summary>
		public decimal UnitPrice { get; set; }
		/// <summary>
		/// 
		/// <summary>
		public double AreaSize { get; set; }
		/// <summary>
		/// 
		/// <summary>
		public decimal TotalAmount { get; set; }
		/// <summary>
		/// 
		/// <summary>
		public string FileName { get; set; }
		/// <summary>
		/// 
		/// <summary>
		public string FileFullName { get; set; }
		/// <summary>
		/// 
		/// <summary>
		public int OrderStatus { get; set; }
		/// <summary>
		/// 
		/// <summary>
		public int SettlementId { get; set; }
	}
}
