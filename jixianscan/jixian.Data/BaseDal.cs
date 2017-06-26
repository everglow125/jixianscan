using jixian.Data.Inte;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace jixian.Data.Impl
{
    public abstract class BaseDal<T> : IBaseDal<T> where T : class
    {
        static object locker = new object();
        static IDbHelper db;
        protected abstract string TableName { get; }
        public BaseDal()
        {
            if (db == null)
                lock (locker)
                    if (db == null)
                    {
                        var dbFolder = ConfigurationManager.AppSettings.Get("dbFolder");
                        var dbName = ConfigurationManager.AppSettings.Get("dbName");
                        db = DbHelper.GetInstant(dbFolder, dbName);
                    }
        }


        public T QueryById(int id)
        {
            string sql = string.Format(" select top 1 * from {0} where Id=@Id ", TableName);
            return db.QueryFirst<T>(sql, new { Id = id });
        }

        protected abstract string GetCondition(T query);
        protected abstract string InsertSql();
        protected abstract string UpdateSql();

        public T QueryFirst(T query)
        {
            string where = GetCondition(query);
            string sql = string.Format("select top 1 * from  {0} {1} {2}", TableName, where.Length > 0 ? "where" : "", where);
            return db.QueryFirst<T>(sql, query);
        }
        /// <summary>
        /// 抽象成分页 sql 语句 需将 RowNumber 命名为 RowId
        /// </summary>
        protected string GetSqlPaging(string sql)
        {
            return string.Format(" select top (@PageSize) * from ({0}) TT where TT.RowId > @PageIndex order by TT.RowId", sql);
        }
        public List<T> QueryList(T query, out int count, int pageIndex, int pageSize)
        {
            count = 0;
            return null;

        }
        public List<T> QueryAll()
        {
            string sql = string.Format(" select * from {0} ", TableName);
            return db.QueryList<T>(sql);
        }
        public bool Insert(T entity)
        {
            return db.Excute(InsertSql(), entity) > 0;
        }

        public bool Insert(List<T> entitys)
        {
            return db.Excute(InsertSql(), entitys) > 0;
        }

        public bool Update(T entity)
        {
            return db.Excute(UpdateSql(), entity) > 0;
        }

        public bool Update(List<T> entitys)
        {
            return db.Excute(UpdateSql(), entitys) > 0;
        }

        public bool Delete(int id)
        {
            string sql = string.Format("delete from {0} where Id=@Id", TableName);
            return db.Excute(sql, new { Id = id }) > 0;
        }
        public bool Delete(List<int> ids)
        {
            string sql = string.Format("delete from {0} where Id=@Id", TableName);
            return db.Excute(sql, ids.Select(x => new { Id = x }).ToList()) > 0;
        }
    }
}
