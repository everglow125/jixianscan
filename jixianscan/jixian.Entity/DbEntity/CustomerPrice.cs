using System;
namespace jixian.Entity
{
	public partial class CustomerPrice
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
		public int PrintTypeId { get; set; }
		/// <summary>
		/// 
		/// <summary>
		public decimal UnitPrice { get; set; }
	}
}
