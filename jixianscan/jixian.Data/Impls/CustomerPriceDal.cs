﻿using jixian.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using jixian.Data.Inte;
namespace jixian.Data.Impl
{
    public class CustomerPriceDal : BaseDal<CustomerPrice>, ICustomerPriceDal
    {
        protected override string GetCondition(CustomerPrice query)
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
                return "CustomerPrice";
            }
        }
        protected override string UpdateSql()
        {
            return "";
        }
    }
}