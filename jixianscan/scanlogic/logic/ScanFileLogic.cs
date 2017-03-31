using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace scanlogic
{
    public class ScanFileLogic
    {
        static List<ScanFileInfo> fileList;
        static List<CustomerPrice> priceList;
        public List<ScanFileInfo> Excute(string filePath, DateTime scanDate)
        {
            fileList = new List<ScanFileInfo>();
            priceList = PriceInitLogic.GetPrices();
            ScanFile(filePath, filePath);
            return fileList;
        }

        private void ScanFile(string scanFolder, string folder, string customerName = "")
        {
            DirectoryInfo directory = new DirectoryInfo(folder);
            if (directory == null || !directory.Exists) return;
            var postfix = ConfigurationManager.AppSettings["postfix"].ToUpper().Trim().Split('|');
            foreach (FileInfo file in directory.GetFiles())
            {
                var fileName = file.Name.Trim().ToUpper();
                //文件扩展名是否符合扫描标准
                var filePostfix = fileName.Split('.')[1];
                if (!postfix.Contains(filePostfix))
                    continue;

                string[] folderList = folder.Split('\\');
                string fullName = folder + "\\" + file.Name;
                ScanFileInfo fileInfo = new ScanFileInfo();
                fileInfo.FileName = fileName;
                fileInfo.FullPath = folder + "\\" + fileName;
                fileInfo.Customer = customerName;
                MatchFileInfo(fileInfo, fileName, folderList);
                fileList.Add(fileInfo);
            }
            //扫描子文件夹
            foreach (DirectoryInfo folderItem in directory.GetDirectories())
            {
                ScanFile(scanFolder, folder + folderItem.Name, folder == scanFolder ? folderItem.Name : "");
            }
        }

        private bool MatchFileInfo(ScanFileInfo fileInfo, string fileName, string[] folderList)
        {
            string printType = fileName.GetPrintType();
            if (printType == "")
            {
                foreach (var item in folderList)
                {
                    printType = item.GetPrintType();
                    if (printType != "") break;
                }
            }
            if (printType == "")
                printType = "写真";
            var ld = fileName.MatchLength();
            if (ld[0] == 0)
            {
                foreach (var item in folderList)
                {
                    ld = item.MatchLength();
                    if (ld[0] > 0) break;
                }
            }
            if (printType == "喷绘")
            {
                ld[0] = ld[0] + 0.1;
                ld[1] = ld[1] + 0.1;
            }
            fileInfo.Length = ld[0] / 100;
            fileInfo.Width = ld[1] / 100;
            var size = (1000 * ld[0] * ld[1]) / 10000000;
            fileInfo.Area = size;
            fileInfo.Quantity = fileName.MatchQTY();
            fileInfo.PrintType = printType;
            var tempCustomerName = priceList.Select(x => x.Customer).Contains(fileInfo.Customer) ? fileInfo.Customer : "通用";
            var price = priceList.Where(x => x.Customer == tempCustomerName
                                          && x.PrintType == fileInfo.PrintType)
                                 .FirstOrDefault().UnitPrice;
            fileInfo.UnitPrice = price;
            var amount = fileInfo.Quantity * fileInfo.UnitPrice * (fileInfo.PrintType == "奖牌" ? 1 : fileInfo.Area);
            fileInfo.TotalAmount = Convert.ToDouble(amount);
            if (amount == 0)
                return false;
            return true;
        }

    }
}
