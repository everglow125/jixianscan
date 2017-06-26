using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace jixian.Data.Impl
{
    interface IDbHelper
    {
        int Excute(string sql, object param = null);
        List<T> QueryList<T>(string sql, object param = null);
        T QueryFirst<T>(string sql, object param = null);
        DataSet ExcuteDataSet(string sql, SQLiteParameter[] parameters = null);
    }
}
