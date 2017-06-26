using System;
namespace jixian.Entity
{
	public partial class SettlementInfo
	{
		/// <summary>
		/// 
		/// <summary>
		public int Id { get; set; }
		/// <summary>
		/// 
		/// <summary>
		public DateTime SettlementDate { get; set; }
		/// <summary>
		/// 
		/// <summary>
		public int CustomerId { get; set; }
		/// <summary>
		/// 
		/// <summary>
		public int SettlementType { get; set; }
		/// <summary>
		/// 
		/// <summary>
		public decimal AmountReceivable { get; set; }
		/// <summary>
		/// 
		/// <summary>
		public decimal AmountReceived { get; set; }
		/// <summary>
		/// 
		/// <summary>
		public string Payee { get; set; }
		/// <summary>
		/// 
		/// <summary>
		public string ReceiveAccount { get; set; }
		/// <summary>
		/// 
		/// <summary>
		public string ReceiveAccountType { get; set; }
		/// <summary>
		/// 
		/// <summary>
		public string Payer { get; set; }
		/// <summary>
		/// 
		/// <summary>
		public string PaidAccount { get; set; }
		/// <summary>
		/// 
		/// <summary>
		public string PaidAccountType { get; set; }
	}
}
