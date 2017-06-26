using jixian.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using jixian.Data.Inte;

namespace jixian.Data.Impl
{
    public class AccountTypeDal : BaseDal<AccountType>, IAccountTypeDal
    {
        protected override string GetCondition(AccountType query)
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
                return "AccountType";
            }
        }
        protected override string UpdateSql()
        {
            return "";
        }

    }
}
