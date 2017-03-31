﻿using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace scanlogic
{
    public static class ExcelHelper
    {
        #region 私有方法

        /// <summary>
        /// 获取要保存的文件名称（含完整路径）
        /// </summary>
        /// <returns></returns>
        public static string GetSaveFilePath(string fileName = "")
        {
            SaveFileDialog saveFileDig = new SaveFileDialog();
            saveFileDig.Filter = "Excel Office97-2003(*.xls)|*.xls|Excel Office2007及以上(*.xlsx)|*.xlsx";
            saveFileDig.FileName = fileName;
            saveFileDig.FilterIndex = 0;
            saveFileDig.Title = "导出到";
            saveFileDig.OverwritePrompt = true;
            saveFileDig.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            string filePath = null;
            if (saveFileDig.ShowDialog() == DialogResult.OK)
            {
                filePath = saveFileDig.FileName;
            }

            return filePath;
        }

        /// <summary>
        /// 获取要打开要导入的文件名称（含完整路径）
        /// </summary>
        /// <returns></returns>
        private static string GetOpenFilePath()
        {
            OpenFileDialog openFileDig = new OpenFileDialog();
            openFileDig.Filter = "Excel Office97-2003(*.xls)|*.xls|Excel Office2007及以上(*.xlsx)|*.xlsx";
            openFileDig.FilterIndex = 0;
            openFileDig.Title = "打开";
            openFileDig.CheckFileExists = true;
            openFileDig.CheckPathExists = true;
            openFileDig.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            string filePath = null;
            if (openFileDig.ShowDialog() == DialogResult.OK)
            {
                filePath = openFileDig.FileName;
            }

            return filePath;
        }

        /// <summary>
        /// 判断是否为兼容模式
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private static bool GetIsCompatible(string filePath)
        {
            return filePath.EndsWith(".xls", StringComparison.OrdinalIgnoreCase);
        }



        /// <summary>
        /// 创建工作薄
        /// </summary>
        /// <param name="isCompatible"></param>
        /// <returns></returns>
        private static IWorkbook CreateWorkbook(bool isCompatible)
        {
            if (isCompatible)
            {
                return new HSSFWorkbook();
            }
            else
            {
                return new XSSFWorkbook();
            }
        }

        /// <summary>
        /// 创建工作薄(依据文件流)
        /// </summary>
        /// <param name="isCompatible"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        private static IWorkbook CreateWorkbook(bool isCompatible, dynamic stream)
        {
            if (isCompatible)
            {
                return new HSSFWorkbook(stream);
            }
            else
            {
                return new XSSFWorkbook(stream);
            }
        }

        /// <summary>
        /// 创建表格头单元格
        /// </summary>
        /// <param name="sheet"></param>
        /// <returns></returns>
        private static ICellStyle GetCellStyle(IWorkbook workbook)
        {
            ICellStyle style = workbook.CreateCellStyle();
            style.FillPattern = FillPattern.SolidForeground;
            style.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
            style.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            style.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            style.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            style.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            style.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Top;

            return style;
        }


        /// <summary>
        /// 从工作表中生成DataTable
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="headerRowIndex"></param>
        /// <returns></returns>
        private static DataTable GetDataTableFromSheet(ISheet sheet, int headerRowIndex)
        {
            DataTable table = new DataTable();

            IRow headerRow = sheet.GetRow(headerRowIndex);
            int cellCount = headerRow.LastCellNum;

            for (int i = headerRow.FirstCellNum; i < cellCount; i++)
            {
                if (headerRow.GetCell(i) == null || headerRow.GetCell(i).StringCellValue.Trim() == "")
                {
                    // 如果遇到第一个空列，则不再继续向后读取
                    cellCount = i;
                    break;
                }
                DataColumn column = new DataColumn(headerRow.GetCell(i).StringCellValue);
                table.Columns.Add(column);
            }

            for (int i = (headerRowIndex + 1); i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                //如果遇到某行的第一个单元格的值为空，则不再继续向下读取
                if (row != null && !string.IsNullOrEmpty(row.GetCell(0).ToString()))
                {
                    DataRow dataRow = table.NewRow();

                    for (int j = row.FirstCellNum; j < cellCount; j++)
                    {
                        dataRow[j] = row.GetCell(j).ToString();
                    }

                    table.Rows.Add(dataRow);
                }
            }

            return table;
        }

        #endregion

        #region 公共导出方法

        /// <summary>
        /// 由DataSet导出Excel
        /// </summary>
        /// <param name="sourceTable">要导出数据的DataTable</param>
        /// <returns>Excel工作表</returns>
        public static string ExportToExcel(DataSet sourceDs, string filePath = null)
        {

            if (string.IsNullOrEmpty(filePath))
            {
                filePath = GetSaveFilePath();
            }

            if (string.IsNullOrEmpty(filePath)) return null;

            bool isCompatible = GetIsCompatible(filePath);

            IWorkbook workbook = CreateWorkbook(isCompatible);
            ICellStyle cellStyle = GetCellStyle(workbook);

            for (int i = 0; i < sourceDs.Tables.Count; i++)
            {
                DataTable table = sourceDs.Tables[i];
                string sheetName = "result" + i.ToString();
                ISheet sheet = workbook.CreateSheet(sheetName);
                IRow headerRow = sheet.CreateRow(0);
                // handling header.
                foreach (DataColumn column in table.Columns)
                {
                    ICell cell = headerRow.CreateCell(column.Ordinal);
                    cell.SetCellValue(column.ColumnName);
                    cell.CellStyle = cellStyle;
                }

                // handling value.
                int rowIndex = 1;

                foreach (DataRow row in table.Rows)
                {
                    IRow dataRow = sheet.CreateRow(rowIndex);

                    foreach (DataColumn column in table.Columns)
                    {
                        dataRow.CreateCell(column.Ordinal).SetCellValue((row[column] ?? "").ToString());
                    }

                    rowIndex++;
                }
            }

            FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            workbook.Write(fs);
            fs.Dispose();
            workbook = null;

            return filePath;

        }


        /// <summary>
        /// 由DataTable导出Excel
        /// </summary>
        /// <param name="sourceTable">要导出数据的DataTable</param>
        /// <returns>Excel工作表</returns>
        public static string ExportToExcel(DataTable sourceTable, string sheetName = "result", string filePath = null)
        {
            if (sourceTable.Rows.Count <= 0) return null;

            if (string.IsNullOrEmpty(filePath))
            {
                filePath = GetSaveFilePath();
            }

            if (string.IsNullOrEmpty(filePath)) return null;

            bool isCompatible = GetIsCompatible(filePath);

            IWorkbook workbook = CreateWorkbook(isCompatible);
            ICellStyle cellStyle = GetCellStyle(workbook);

            ISheet sheet = workbook.CreateSheet(sheetName);
            IRow headerRow = sheet.CreateRow(0);
            IFont fontHead = workbook.CreateFont();
            fontHead.Boldweight = (int)FontBoldWeight.Bold;
            fontHead.FontHeight = 256;

            ICellStyle styleHead = workbook.CreateCellStyle();
            styleHead.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            styleHead.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            styleHead.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            styleHead.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            styleHead.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
            styleHead.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            styleHead.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Top;
            styleHead.SetFont(fontHead);

            ICellStyle style = workbook.CreateCellStyle();
            style.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            style.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            style.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            style.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            style.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Top;


            IDataFormat datastyle = workbook.CreateDataFormat();

            ICellStyle styleDate = workbook.CreateCellStyle();
            styleDate.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            styleDate.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            styleDate.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            styleDate.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            styleDate.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            styleDate.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Top;
            styleDate.DataFormat = datastyle.GetFormat("mm月dd日");

            ICellStyle styleleft = workbook.CreateCellStyle();
            styleleft.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            styleleft.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            styleleft.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            styleleft.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            styleleft.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Left;
            styleleft.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Top;
            // handling header.
            foreach (DataColumn column in sourceTable.Columns)
            {
                // if (column.ColumnName == "完全路径") continue;
                ICell headerCell = headerRow.CreateCell(column.Ordinal);
                headerCell.SetCellValue(column.ColumnName);
                headerCell.CellStyle = styleHead;

            }

            // handling value.
            int rowIndex = 1;
            int margenIndex = 1;
            int margenCount = 0;
            string currentDate = sourceTable.Rows[0][0].ToString();

            foreach (DataRow row in sourceTable.Rows)
            {
                IRow dataRow = sheet.CreateRow(rowIndex);

                foreach (DataColumn column in sourceTable.Columns)
                {

                    var temp = dataRow.CreateCell(column.Ordinal);
                    if (column.ColumnName == "长度" || column.ColumnName == "宽度" || column.ColumnName == "面积" || column.ColumnName == "单价" || column.ColumnName == "总价" || column.ColumnName == "数量")
                    {
                        temp.SetCellValue(Convert.ToDouble(row[column]));
                        temp.SetCellType(CellType.Numeric);
                        if (column.ColumnName == "面积")
                        {
                            temp.SetCellFormula(string.Format("C{0}*D{0}", rowIndex + 1));
                        }
                        if (column.ColumnName == "总价")
                        {
                            if (row["类型"].ToString() != "奖牌")
                                temp.SetCellFormula(string.Format("E{0}*H{0}*G{0}", rowIndex + 1));
                            else
                                temp.SetCellFormula(string.Format("H{0}*G{0}", rowIndex + 1));
                        }

                    }
                    else if (column.ColumnName == "时间")
                    {
                        var dateTmp = Convert.ToDateTime(row["时间"].ToString()).Date;
                        temp.SetCellValue(dateTmp);
                    }
                    else
                    {
                        temp.SetCellValue((row[column] ?? "").ToString());
                    }
                    temp.CellStyle = column.ColumnName == "文件名" ? styleleft : (column.ColumnName == "时间" ? styleDate : style);
                    if (column.ColumnName == "完全路径")
                    {
                        //创建一个超链接对象
                        IHyperlink link = new HSSFHyperlink(HyperlinkType.Url);
                        // strTableName 这个参数为 sheet名字 A1 为单元格 其他是固定格式
                        if (ConfigurationManager.AppSettings["IsOpenFile"].Trim().ToLower() == "true")
                        {
                            link.Address = row[column].ToString();
                            temp.SetCellValue("打开文件");
                        }
                        else
                        {
                            link.Address = row[column].ToString().Substring(0, row[column].ToString().LastIndexOf("\\"));
                            temp.SetCellValue("打开文件夹");
                        }
                        temp.Hyperlink = link;

                    }
                }


                if (rowIndex > 1)
                {
                    string rowDate = sourceTable.Rows[rowIndex - 1]["时间"].ToString();
                    if (rowIndex != sourceTable.Rows.Count && rowDate == currentDate)
                    {
                        margenCount++;
                    }
                    else
                    {
                        if (rowIndex == sourceTable.Rows.Count) margenCount++;
                        currentDate = rowDate;
                        sheet.AddMergedRegion(new CellRangeAddress(margenIndex, margenIndex + margenCount, 0, 0));
                        //合并之前单元格
                        margenIndex = rowIndex;
                        margenCount = 0;
                    }
                }
                rowIndex++;
            }
            for (int i = 0; i < sourceTable.Rows.Count - 1; i++)
            {
                if (i == 1) sheet.SetColumnWidth(i, 25 * 256); else sheet.SetColumnWidth(i, 256 * 12);
                //sheet.AutoSizeColumn(i, true);
            }
            IRow countRow = sheet.CreateRow(rowIndex);
            var tt = countRow.CreateCell(8);
            tt.SetCellFormula(string.Format("SUM(I2:I{0})", rowIndex));
            sheet.CreateFreezePane(0, 1, 0, 1);
            FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            workbook.Write(fs);
            fs.Dispose();

            sheet = null;
            headerRow = null;
            workbook = null;

            return filePath;
        }

        /// <summary>
        /// 由List导出Excel
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="data">在导出的List</param>
        /// <param name="sheetName">sheet名称</param>
        /// <returns></returns>
        public static string ExportToExcel<T>(List<T> data, IList<KeyValuePair<string, string>> headerNameList, string sheetName = "result", string filePath = null) where T : class
        {
            if (data.Count <= 0) return null;

            if (string.IsNullOrEmpty(filePath))
            {
                filePath = GetSaveFilePath();
            }

            if (string.IsNullOrEmpty(filePath)) return null;

            bool isCompatible = GetIsCompatible(filePath);

            IWorkbook workbook = CreateWorkbook(isCompatible);
            ICellStyle cellStyle = GetCellStyle(workbook);
            ISheet sheet = workbook.CreateSheet(sheetName);
            IRow headerRow = sheet.CreateRow(0);

            for (int i = 0; i < headerNameList.Count; i++)
            {
                ICell cell = headerRow.CreateCell(i);
                cell.SetCellValue(headerNameList[i].Value);
                cell.CellStyle = cellStyle;
            }

            Type t = typeof(T);
            int rowIndex = 1;
            foreach (T item in data)
            {
                IRow dataRow = sheet.CreateRow(rowIndex);
                for (int n = 0; n < headerNameList.Count; n++)
                {
                    object pValue = t.GetProperty(headerNameList[n].Key).GetValue(item, null);
                    dataRow.CreateCell(n).SetCellValue((pValue ?? "").ToString());
                }
                rowIndex++;
            }
            FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            workbook.Write(fs);
            fs.Dispose();

            sheet = null;
            headerRow = null;
            workbook = null;

            return filePath;
        }

        /// <summary>
        /// 由DataGridView导出
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="sheetName"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string ExportToExcel(DataGridView grid, string sheetName = "result", string filePath = null)
        {
            if (grid.Rows.Count <= 0) return null;

            if (string.IsNullOrEmpty(filePath))
            {
                filePath = GetSaveFilePath();
            }

            if (string.IsNullOrEmpty(filePath)) return null;

            bool isCompatible = GetIsCompatible(filePath);

            IWorkbook workbook = CreateWorkbook(isCompatible);
            ICellStyle cellStyle = GetCellStyle(workbook);
            ISheet sheet = workbook.CreateSheet(sheetName);

            IRow headerRow = sheet.CreateRow(0);

            for (int i = 0; i < grid.Columns.Count; i++)
            {
                ICell cell = headerRow.CreateCell(i);
                cell.SetCellValue(grid.Columns[i].HeaderText);
                cell.CellStyle = cellStyle;
            }

            int rowIndex = 1;
            foreach (DataGridViewRow row in grid.Rows)
            {
                IRow dataRow = sheet.CreateRow(rowIndex);
                for (int n = 0; n < grid.Columns.Count; n++)
                {
                    dataRow.CreateCell(n).SetCellValue((row.Cells[n].Value ?? "").ToString());
                }
                rowIndex++;
            }

            FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            workbook.Write(fs);
            fs.Dispose();

            sheet = null;
            headerRow = null;
            workbook = null;

            return filePath;
        }

        #endregion

        #region 公共导入方法

        /// <summary>
        /// 由Excel导入DataTable
        /// </summary>
        /// <param name="excelFileStream">Excel文件流</param>
        /// <param name="sheetName">Excel工作表名称</param>
        /// <param name="headerRowIndex">Excel表头行索引</param>
        /// <param name="isCompatible">是否为兼容模式</param>
        /// <returns>DataTable</returns>
        public static DataTable ImportFromExcel(Stream excelFileStream, string sheetName, int headerRowIndex, bool isCompatible)
        {
            IWorkbook workbook = CreateWorkbook(isCompatible, excelFileStream);
            ISheet sheet = null;
            int sheetIndex = -1;
            if (int.TryParse(sheetName, out sheetIndex))
            {
                sheet = workbook.GetSheetAt(sheetIndex);
            }
            else
            {
                sheet = workbook.GetSheet(sheetName);
            }

            DataTable table = GetDataTableFromSheet(sheet, headerRowIndex);

            excelFileStream.Close();
            workbook = null;
            sheet = null;
            return table;
        }

        /// <summary>
        /// 由Excel导入DataTable
        /// </summary>
        /// <param name="excelFilePath">Excel文件路径，为物理路径。</param>
        /// <param name="sheetName">Excel工作表名称</param>
        /// <param name="headerRowIndex">Excel表头行索引</param>
        /// <returns>DataTable</returns>
        public static DataTable ImportFromExcel(string excelFilePath, string sheetName, int headerRowIndex)
        {
            if (string.IsNullOrEmpty(excelFilePath))
            {
                excelFilePath = GetOpenFilePath();
            }

            if (string.IsNullOrEmpty(excelFilePath))
            {
                return null;
            }

            using (FileStream stream = System.IO.File.OpenRead(excelFilePath))
            {
                bool isCompatible = GetIsCompatible(excelFilePath);
                return ImportFromExcel(stream, sheetName, headerRowIndex, isCompatible);
            }
        }

        /// <summary>
        /// 由Excel导入DataSet，如果有多个工作表，则导入多个DataTable
        /// </summary>
        /// <param name="excelFileStream">Excel文件流</param>
        /// <param name="headerRowIndex">Excel表头行索引</param>
        /// <param name="isCompatible">是否为兼容模式</param>
        /// <returns>DataSet</returns>
        public static DataSet ImportFromExcel(Stream excelFileStream, int headerRowIndex, bool isCompatible)
        {
            DataSet ds = new DataSet();
            IWorkbook workbook = CreateWorkbook(isCompatible, excelFileStream);
            for (int i = 0; i < workbook.NumberOfSheets; i++)
            {
                ISheet sheet = workbook.GetSheetAt(i);
                DataTable table = GetDataTableFromSheet(sheet, headerRowIndex);
                ds.Tables.Add(table);
            }

            excelFileStream.Close();
            workbook = null;

            return ds;
        }

        /// <summary>
        /// 由Excel导入DataSet，如果有多个工作表，则导入多个DataTable
        /// </summary>
        /// <param name="excelFilePath">Excel文件路径，为物理路径。</param>
        /// <param name="headerRowIndex">Excel表头行索引</param>
        /// <returns>DataSet</returns>
        public static DataSet ImportFromExcel(string excelFilePath, int headerRowIndex)
        {
            if (string.IsNullOrEmpty(excelFilePath))
            {
                excelFilePath = GetOpenFilePath();
            }

            if (string.IsNullOrEmpty(excelFilePath))
            {
                return null;
            }

            using (FileStream stream = System.IO.File.OpenRead(excelFilePath))
            {
                bool isCompatible = GetIsCompatible(excelFilePath);
                return ImportFromExcel(stream, headerRowIndex, isCompatible);
            }
        }

        #endregion

        #region 公共转换方法

        /// <summary>
        /// 将Excel的列索引转换为列名，列索引从0开始，列名从A开始。如第0列为A，第1列为B...
        /// </summary>
        /// <param name="index">列索引</param>
        /// <returns>列名，如第0列为A，第1列为B...</returns>
        public static string ConvertColumnIndexToColumnName(int index)
        {
            index = index + 1;
            int system = 26;
            char[] digArray = new char[100];
            int i = 0;
            while (index > 0)
            {
                int mod = index % system;
                if (mod == 0) mod = system;
                digArray[i++] = (char)(mod - 1 + 'A');
                index = (index - 1) / 26;
            }
            StringBuilder sb = new StringBuilder(i);
            for (int j = i - 1; j >= 0; j--)
            {
                sb.Append(digArray[j]);
            }
            return sb.ToString();
        }


        /// <summary>
        /// 转化日期
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns></returns>
        public static DateTime ConvertToDate(object date)
        {
            string dtStr = (date ?? "").ToString();

            DateTime dt = new DateTime();

            if (DateTime.TryParse(dtStr, out dt))
            {
                return dt;
            }

            try
            {
                string spStr = "";
                if (dtStr.Contains("-"))
                {
                    spStr = "-";
                }
                else if (dtStr.Contains("/"))
                {
                    spStr = "/";
                }
                string[] time = dtStr.Split(spStr.ToCharArray());
                int year = Convert.ToInt32(time[2]);
                int month = Convert.ToInt32(time[0]);
                int day = Convert.ToInt32(time[1]);
                string years = Convert.ToString(year);
                string months = Convert.ToString(month);
                string days = Convert.ToString(day);
                if (months.Length == 4)
                {
                    dt = Convert.ToDateTime(date);
                }
                else
                {
                    string rq = "";
                    if (years.Length == 1)
                    {
                        years = "0" + years;
                    }
                    if (months.Length == 1)
                    {
                        months = "0" + months;
                    }
                    if (days.Length == 1)
                    {
                        days = "0" + days;
                    }
                    rq = "20" + years + "-" + months + "-" + days;
                    dt = Convert.ToDateTime(rq);
                }
            }
            catch
            {
                throw new Exception("日期格式不正确，转换日期类型失败！");
            }
            return dt;
        }

        /// <summary>
        /// 转化数字
        /// </summary>
        /// <param name="d">数字字符串</param>
        /// <returns></returns>
        public static decimal ConvertToDecimal(object d)
        {
            string dStr = (d ?? "").ToString();
            decimal result = 0;
            if (decimal.TryParse(dStr, out result))
            {
                return result;
            }
            else
            {
                throw new Exception("数字格式不正确，转换数字类型失败！");
            }

        }


        /// <summary>
        /// 转化布尔
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool ConvertToBoolen(object b)
        {
            string bStr = (b ?? "").ToString().Trim();
            bool result = false;
            if (bool.TryParse(bStr, out result))
            {
                return result;
            }
            else if (bStr == "0" || bStr == "1")
            {
                return (bStr == "0");
            }
            else
            {
                throw new Exception("布尔格式不正确，转换布尔类型失败！");
            }
        }

        #endregion

    }
}
