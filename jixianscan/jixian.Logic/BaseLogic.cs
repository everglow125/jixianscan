using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using jixian.Logic.Inte;
using jixian.Data.Inte;

namespace jixian.Logic.Impl
{
    public class BaseLogic<T> : IBaseLogic<T> where T : class
    {
        IBaseDal<T> dbClient;
        public BaseLogic(IBaseDal<T> _dbClient)
        {
            dbClient = _dbClient;
        }
        public T QueryById(int id)
        {
            return dbClient.QueryById(id);
        }
        public T QueryFirst(T query)
        {
            return dbClient.QueryFirst(query);
        }
        public List<T> QueryList(T query, out int count, int pageIndex, int pageSize)
        {
            return dbClient.QueryList(query, out count, pageIndex, pageSize);
        }
        public List<T> QueryAll()
        {
            return dbClient.QueryAll();
        }
        public bool Insert(T entity) { return dbClient.Insert(entity); }

        public bool Insert(List<T> entitys) { return dbClient.Insert(entitys); }

        public bool Update(T entity) { return dbClient.Update(entity); }

        public bool Update(List<T> entitys) { return dbClient.Update(entitys); }

        public bool Delete(int id) { return dbClient.Delete(id); }
        public bool Delete(List<int> ids) { return dbClient.Delete(ids); }
    }
}
