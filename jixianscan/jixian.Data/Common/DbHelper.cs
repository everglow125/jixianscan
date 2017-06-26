using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;
using System.IO;
using System.Data.SQLite;
using System.Data;
using System.Configuration;

namespace jixian.Data.Impl
{
    public class DbHelper : IDbHelper
    {
        static string dbFolder;
        static string dbName;
        static object locker = new object();
        static DbHelper Instant;

        public static DbHelper GetInstant(string _dbFolder, string _dbName)
        {
            if (Instant == null)
            {
                lock (locker)
                {
                    if (Instant == null)
                    {
                        Instant = new DbHelper();
                        dbFolder = _dbFolder;
                        dbName = _dbName;
                    }
                }
            }
            return Instant;
        }

        private DbHelper() { }


        private IDbConnection GetConn()
        {
            try
            {
                SQLiteConnectionStringBuilder sb = new SQLiteConnectionStringBuilder();
                if (!Directory.Exists(dbFolder))
                    Directory.CreateDirectory(dbFolder);
                sb.DataSource = string.Format(@"{0}\{1}", dbFolder, dbName);
                IDbConnection con = new SQLiteConnection(sb.ToString());
                return con;
            }
            catch (
                Exception ex)
            {
                return null;
            }
        }


        public int Excute(string sql, object param = null)
        {
            using (var con = GetConn())
            {
                return con.Execute(sql, param);
            }
        }


        public List<T> QueryList<T>(string sql, object param = null)
        {
            using (var con = GetConn())
            {
                return con.Query<T>(sql, param) as List<T>;
            }
        }

        public T QueryFirst<T>(string sql, object param = null)
        {
            using (var con = GetConn())
            {
                return con.QueryFirstOrDefault<T>(sql, param);
            }
        }

        public DataSet ExcuteDataSet(string sql, SQLiteParameter[] parameters = null)
        {
            using (var connection = GetConn())
            {
                DataSet ds = new DataSet();
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                SQLiteDataAdapter sqlDA = new SQLiteDataAdapter();
                sqlDA.SelectCommand =
                    BuildQueryCommand(connection, sql, parameters);
                sqlDA.Fill(ds, "ds");
                return ds;
            }
        }

        private SQLiteCommand BuildQueryCommand(IDbConnection connection, string sql, IDataParameter[] parameters)
        {
            SQLiteCommand command = new SQLiteCommand(sql, connection as SQLiteConnection);
            command.CommandType = CommandType.Text;
            command.CommandTimeout = 30;
            if (parameters != null)
            {
                foreach (SQLiteParameter parameter in parameters)
                {
                    if (parameter != null)
                    {
                        // 检查未分配值的输出参数,将其分配以DBNull.Value.
                        if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                            (parameter.Value == null))
                        {
                            parameter.Value = DBNull.Value;
                        }
                        command.Parameters.Add(parameter);
                    }
                }
            }
            return command;
        }



    }
}
