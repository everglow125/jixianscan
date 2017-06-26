using jixian.Data.Inte;
using jixian.Entity;
using jixian.Logic.Inte;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace jixian.Logic.Impl
{
    public class PrintTypeLogic : BaseLogic<PrintType>, IPrintTypeLogic
    {
        public IPrintTypeDal db;
        public PrintTypeLogic(IPrintTypeDal _db)
            : base(_db)
        {
            db = _db;
        }
        public Dictionary<string, string> GetPrintTypeMatchs()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            var list = db.QueryAll().OrderBy(x => x.SortIndex).ToList();
            foreach (var item in list)
            {
                var keywords = item.Keyword.Split('|');
                foreach (var keyword in keywords)
                {
                    result.Add(keyword, item.Type);
                }
            }
            return result;
        }

        public string GetTypeByFileName(string fileName)
        {
            var printTypes = GetPrintTypeMatchs();
            foreach (var item in printTypes)
            {
                if (fileName.Contains(item.Key))
                {
                    return item.Value;
                }
            }
            return "";
        }
    }
}
