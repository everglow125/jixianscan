using jixian.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using jixian.Data.Inte;
namespace jixian.Data.Impl
{
    public class OrderInfoDal : BaseDal<OrderInfo>, IOrderInfoDal
    {
        protected override string GetCondition(OrderInfo query)
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
                return "OrderInfo";
            }
        }
        protected override string UpdateSql()
        {
            return "";
        }
    }
}
