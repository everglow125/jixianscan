﻿using jixian.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using jixian.Data.Inte;
namespace jixian.Data.Impl
{
    public class PrintTypeDal : BaseDal<PrintType>, IPrintTypeDal
    {
        protected override string GetCondition(PrintType query)
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
                return "PrintType";
            }
        }
        protected override string UpdateSql()
        {
            return "";
        }
    }
}
