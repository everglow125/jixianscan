using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scanlogic
{
    public class PriceInitLogic
    {
        public static List<CustomerPrice> GetPrices()
        {
            List<CustomerPrice> result = new List<CustomerPrice>();
            string sysPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "priceConfig\\price.xls";
            var dt = ExcelHelper.ImportFromExcel(sysPath, "Sheet1", 0);
            foreach (DataRow dr in dt.Rows)
            {
                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName != "客户")
                    {
                        CustomerPrice price = new CustomerPrice();
                        price.Customer = dr["客户"].ToString();
                        price.PrintType = column.ColumnName;
                        price.UnitPrice = Convert.ToDouble(dr[column.ColumnName].ToString());
                        result.Add(price);
                    }
                }
            }

            return result;

        }
    }
}
