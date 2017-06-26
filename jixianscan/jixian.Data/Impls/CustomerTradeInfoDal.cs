using jixian.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using jixian.Data.Inte;
namespace jixian.Data.Impl
{
    public class CustomerTradeInfoDal : BaseDal<CustomerTradeInfo>, ICustomerTradeInfoDal
    {
        protected override string GetCondition(CustomerTradeInfo query)
        {
            return "";
        }
        protected override string InsertSql()
        {
            return "";
        }

        protected override string TableName
        {
            get
            {
                return "CustomerTradeInfo";
            }
        }
        protected override string UpdateSql()
        {
            return "";
        }
    }
}
