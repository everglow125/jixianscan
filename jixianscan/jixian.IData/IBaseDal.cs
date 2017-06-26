using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace jixian.Data.Inte
{
    public interface IBaseDal<T>
        where T : class
    {
        T QueryById(int id);
        T QueryFirst(T query);
        List<T> QueryList(T query);
        bool Insert(T entity);

        bool Insert(List<T> entitys);

        bool Update(T entity);

        bool Update(List<T> entitys);

        bool Delete(int id);
        bool Delete(List<int> ids);
    }
}
