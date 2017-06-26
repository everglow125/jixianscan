using jixian.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using jixian.Data.Inte;
namespace jixian.Data.Impl
{
    public class CustomerInfoDal : BaseDal<CustomerInfo>, ICustomerInfoDal
    {
        protected override string GetCondition(CustomerInfo query)
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
                return "CustomerInfo";
            }
        }
        protected override string UpdateSql()
        {
            return "";
        }
    }
}
