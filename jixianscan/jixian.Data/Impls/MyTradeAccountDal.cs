using jixian.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using jixian.Data.Inte;
namespace jixian.Data.Impl
{
    public class MyTradeAccountDal : BaseDal<MyTradeAccount>, IMyTradeAccountDal
    {
        protected override string GetCondition(MyTradeAccount query)
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
                return "MyTradeAccount";
            }
        }
        protected override string UpdateSql()
        {
            return "";
        }
    }
}
