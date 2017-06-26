using jixian.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using jixian.Data.Inte;
namespace jixian.Data.Impl
{
    public class SettlementInfoDal : BaseDal<SettlementInfo>, ISettlementInfoDal
    {
        protected override string GetCondition(SettlementInfo query)
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
                return "SettlementInfo";
            }
        }
        protected override string UpdateSql()
        {
            return "";
        }
    }
}
