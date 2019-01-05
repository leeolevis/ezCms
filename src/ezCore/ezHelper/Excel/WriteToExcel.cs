using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using EPPlus.Core.Extensions;
using OfficeOpenXml.Style;


namespace ez.Core.Excel
{
    public class WriteToExcel : IWriteToExcel
    {
        #region Excel2007版本类型
        public string ExcelContentType
        {
            get
            {
                return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            }
        }

        private readonly Dictionary<Type, PropertyInfo[]> _typeCache = new Dictionary<Type, PropertyInfo[]>();
        private readonly object _lock = new object();

        public byte[] ExportListToExcel(DataTable data, List<string> heading, bool isShowSlNo, List<string> keySelectors, List<string> tails)
        {
            if (data == null)
                throw new ArgumentException($"{nameof(data)}can't be null");
            byte[] results = null;
            using (ExcelPackage package = new ExcelPackage())
            {
                var worksheetName = "OutPutExcel";

                var workSheet = package.Workbook.Worksheets.Add($"{worksheetName} Data");
                workSheet.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                workSheet.Cells.Style.Font.Name = "宋体";
                workSheet.Cells.Style.Font.Size = 12;

                int startRowIndex = 1;
                int startColumnIndex = 1;
                var dataRowCount = data.Rows.Count;

                var properties = new List<string>();

                var dt = new DataTable();

                foreach (DataColumn dataColumn in data.Columns)
                {
                    properties.Add(dataColumn.ColumnName);
                    dt.Columns.Add(new DataColumn(dataColumn.ColumnName, dataColumn.DataType));
                }
                //var orderedList = data.Select().OrderBy(dataRow => dataRow[properties.FirstOrDefault()]).ToArray();
                foreach (DataRow row in data.Select())
                {
                    dt.ImportRow(row);
                }
                data = dt;
                var totalColumnCount = properties.Count;

                if (isShowSlNo)
                {
                    //加入序号                   
                    workSheet.Cells[startRowIndex, 1].Value = "序号";

                    for (int slNo = 0; slNo < dataRowCount + heading?.Count + tails.Count; slNo++)
                    {
                        workSheet.Cells[slNo + 2, 1].Value = slNo + 1;
                    }
                    startColumnIndex = 2;
                    totalColumnCount++;
                }

                if (heading != null && heading.Count > 0)
                {
                    for (int rowIndex = 0; rowIndex < heading.Count; rowIndex++)
                    {
                        if (!string.IsNullOrEmpty(heading[rowIndex]))
                        {
                            workSheet.Cells[startRowIndex, !isShowSlNo ? 1 : 2].Value = heading[rowIndex];
                            workSheet.Cells[startRowIndex, !isShowSlNo ? 1 : 2, startRowIndex, totalColumnCount].Merge =
                                true;
                            workSheet.Cells[startRowIndex, !isShowSlNo ? 1 : 2, startRowIndex, totalColumnCount].Style
                                .Font.Bold = true;
                            workSheet.Cells[startRowIndex, !isShowSlNo ? 1 : 2, startRowIndex, totalColumnCount].Style
                                .Font.Size = 20 - rowIndex * 4 > 9 ? 20 - rowIndex * 4 : 9;
                            startRowIndex++;
                        }
                    }
                }

                workSheet.Cells[startRowIndex, startColumnIndex].LoadFromDataTable(data, true);

                var groupStartRowIndex = startRowIndex;
                var groupProperty = properties.FirstOrDefault();

                var groupResult = data.Select().GroupBy(dataRow => dataRow[groupProperty]);//第一个作为主Key，用于分组，其他列则依赖主Key分组进行合并，不在单独分组合并
                foreach (var keySelector in keySelectors)
                {
                    var propIndex = 0;

                    for (var index = 0; index < properties.Count; index++)
                    {
                        var propertyInfo = properties[index];
                        if (propertyInfo == keySelector)
                        {
                            propIndex = index + 1;
                        }
                    }

                    foreach (var item in groupResult)
                    {
                        var groupCount = item.Count();

                        workSheet.Cells[groupStartRowIndex + 1, propIndex + (!isShowSlNo ? 0 : 1), groupStartRowIndex + groupCount, propIndex + (!isShowSlNo ? 0 : 1)].Merge = true;
                        groupStartRowIndex += groupCount;
                    }

                    groupStartRowIndex = startRowIndex;
                }

                startRowIndex += dataRowCount;//累计行数
                if (tails != null && tails.Count > 0)
                {
                    for (int rowIndex = 0; rowIndex < tails.Count; rowIndex++)
                    {
                        startRowIndex++;
                        workSheet.Cells[startRowIndex, !isShowSlNo ? 1 : 2].Value = tails[rowIndex];
                        workSheet.Cells[startRowIndex, (!isShowSlNo ? 1 : 2), startRowIndex, totalColumnCount].Merge = true;

                    }
                }
                workSheet.Cells.Style.ShrinkToFit = true;
                //workSheet.Cells.AutoFitColumns();//设置列宽自适应
                results = package.GetAsByteArray();
                workSheet.Dispose();
            }

            return results;
        }

        #endregion


        #region 带有总计，合计，分组的Excel

        public byte[] ExportGroupListToExcel(DataTable data, List<string> heading, bool isShowSlNo, List<string> keySelectors, List<string> tails, List<string> coloumsColor = null)
        {
            if (data == null)
                throw new ArgumentException($"{nameof(data)}can't be null");
            byte[] results = null;
            using (ExcelPackage package = new ExcelPackage())
            {
                var worksheetName = "OutPutExcel";

                var workSheet = package.Workbook.Worksheets.Add($"{worksheetName} Data");
                workSheet.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                workSheet.Cells.Style.Font.Name = "宋体";
                workSheet.Cells.Style.Font.Size = 11;

                int startRowIndex = 1;
                int startColumnIndex = 1;
                var dataRowCount = data.Rows.Count;

                var properties = new List<string>();

                var dt = new DataTable();
                var coloumsColorDict = new Dictionary<int, string>();
                for (var i = 0; i < data.Columns.Count; i++)
                {
                    var dataColumn = data.Columns[i];
                    properties.Add(dataColumn.ColumnName);
                    dt.Columns.Add(new DataColumn(dataColumn.ColumnName, dataColumn.DataType));
                    if (coloumsColor != null && coloumsColor.Contains(dataColumn.ColumnName)) coloumsColorDict.Add(i, dataColumn.ColumnName);
                }
                var totalColumnCount = properties.Count;

                var rangeCellstr = "";
                //给表头加粗
                rangeCellstr = $@"A{heading.Count + 1}:{Convert.ToChar('A' + totalColumnCount - 1)}{heading.Count + 1}";
                workSheet.Cells[rangeCellstr].Style.Font.Bold = true;
                workSheet.Cells[rangeCellstr].Style.Font.Size = 12;

                var selectData = data.Select();
                var groupColumnWidth = 0;
                for (var i = 0; i < selectData.Length; i++)
                {
                    DataRow row = selectData[i];
                    dt.ImportRow(row);
                    groupColumnWidth = groupColumnWidth < Convert.ToString(row[0]).Length * 2 ? Convert.ToString(row[0]).Length * 2 : groupColumnWidth;
                    if (Convert.ToString(row[0]) == "合计" || Convert.ToString(row[1]) == "总计")
                    {
                        //给总计和合计项加粗
                        var boldCellStart = Convert.ToString(row[0]) == "合计" ? 'A' : 'B';
                        rangeCellstr = $@"{boldCellStart}{heading.Count + i + 2}:{Convert.ToChar(boldCellStart + data.Columns.Count)}{heading.Count + i + 2}";
                        workSheet.Cells[rangeCellstr].Style.Font.Bold = true;
                        workSheet.Cells[rangeCellstr].Style.Font.Size = 12;
                    }
                    foreach (var item in coloumsColorDict)
                    {
                        var coutRate = Convert.ToDouble(row[item.Value].ToString().Replace("%", ""));
                        if (coutRate < 60 || coutRate > 100)
                        {
                            //workSheet.Cells[$@"{Convert.ToChar('A' + item.Key)}{heading.Count + i + 2}"].Style.SetBackgroundColor(coutRate > 100 ? ColorTranslator.FromHtml("#FFC7CE") : ColorTranslator.FromHtml("#C6EFCE"));

                            workSheet.Cells[$@"{Convert.ToChar('A' + item.Key)}{heading.Count + i + 2}"].Style.Fill.BackgroundColor.SetColor(coutRate > 100 ? ColorTranslator.FromHtml("#FFC7CE") : ColorTranslator.FromHtml("#C6EFCE"));
                        }
                    }
                }
                data = dt;
                if (heading != null && heading.Count > 0)
                {
                    for (int rowIndex = 0; rowIndex < heading.Count; rowIndex++)
                    {
                        if (!string.IsNullOrEmpty(heading[rowIndex]))
                        {
                            workSheet.Cells[startRowIndex, !isShowSlNo ? 1 : 2].Value = heading[rowIndex];
                            workSheet.Cells[startRowIndex, !isShowSlNo ? 1 : 2, startRowIndex, totalColumnCount].Merge =
                                true;
                            workSheet.Cells[startRowIndex, !isShowSlNo ? 1 : 2, startRowIndex, totalColumnCount].Style
                                .Font.Bold = true;
                            workSheet.Cells[startRowIndex, !isShowSlNo ? 1 : 2, startRowIndex, totalColumnCount].Style
                                .Font.Size = 20 - rowIndex * 8 > 12 ? 20 - rowIndex * 8 : 12;
                            if (rowIndex != 0)
                            {
                                //workSheet.Cells[startRowIndex, !isShowSlNo ? 1 : 2, startRowIndex, totalColumnCount].Style.SetHorizontalAlignment(ExcelHorizontalAlignment.Left);

                                workSheet.Cells[startRowIndex, !isShowSlNo ? 1 : 2, startRowIndex, totalColumnCount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            }
                            startRowIndex++;
                        }
                    }
                }

                workSheet.Cells[startRowIndex, startColumnIndex].LoadFromDataTable(data, true);

                var groupStartRowIndex = startRowIndex;
                var groupProperty = properties.FirstOrDefault();

                var groupResult = data.Select().GroupBy(dataRow => dataRow[groupProperty]);//第一个作为主Key，用于分组，其他列则依赖主Key分组进行合并，不在单独分组合并
                foreach (var keySelector in keySelectors)
                {
                    var propIndex = 0;

                    for (var index = 0; index < properties.Count; index++)
                    {
                        var propertyInfo = properties[index];
                        if (propertyInfo == keySelector)
                        {
                            propIndex = index + 1;
                        }
                    }

                    foreach (var item in groupResult)
                    {
                        var groupCount = item.Count();

                        workSheet.Cells[groupStartRowIndex + 1, propIndex + (!isShowSlNo ? 0 : 1), groupStartRowIndex + groupCount, propIndex + (!isShowSlNo ? 0 : 1)].Merge = true;
                        groupStartRowIndex += groupCount;
                    }

                    groupStartRowIndex = startRowIndex;
                }

                startRowIndex += dataRowCount;//累计行数
                if (tails != null && tails.Count > 0)
                {
                    for (int rowIndex = 0; rowIndex < tails.Count; rowIndex++)
                    {
                        startRowIndex++;
                        workSheet.Cells[startRowIndex, !isShowSlNo ? 1 : 2].Value = tails[rowIndex];
                        workSheet.Cells[startRowIndex, (!isShowSlNo ? 1 : 2), startRowIndex, totalColumnCount].Merge = true;
                    }
                }
                if (Convert.ToString(((DataRow)selectData[selectData.Length - 1])[0]) == "合计")
                {
                    //最后一行合计   合并第一第二单元格
                    workSheet.Cells[$@"A{startRowIndex}:B{startRowIndex}"].Merge = false;
                    workSheet.Cells[$@"A{startRowIndex}:B{startRowIndex}"].Merge = true;
                }
                workSheet.Cells.AutoFitColumns();//设置列宽自适应
                workSheet.Column(1).Width = groupColumnWidth + 4;
                results = package.GetAsByteArray();
                workSheet.Dispose();
            }

            return results;
        }

        #endregion

        #region 将数据写入到指定Excel模版中
        /// <summary>
        /// 对应的模版为第一行为大标题，第1列为各关键列，第二行为关键列对应的各属性值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sourceData"><标题,T>key为Excel模版中的keyCell对应列的内容，如A1列中为"部门1，部门2，部门3",<部门1, T>，可将部门1等内容写在属性的Display(Name="部门1")上</param>
        /// <param name="templateFilePath">Excel模版路径</param>
        /// <param name="Message">输出错误信息</param>
        /// <param name="dataRangeStartCell">数据区域开始的单元格，如第1行为标题，A列为关键字，数据区域从B2开始写</param>
        /// <param name="keyCell">如A1列中为"部门1，部门2，部门3"，标题为“姓名、年龄、生日”等</param>
        /// <param name="changeTitleCell">需更改的标题所在单元格</param>
        /// <param name="changeContent">将标题所在单元格替换为该内容</param>
        /// <param name="columnData">Key为对应单元格，Value为T的属性名，将属性值写入到对应单元格内，如//<C5,Name> <D5, Age> Name与Age均为T中的属性，表示将Name中的值写入到C5</param>
        /// <param name="sheetName">工作表名称，默认为第一个</param>
        /// <returns>写入成功，返回True，否则返回false</returns>
        public bool ExportEntityToExcelFile<T>(Dictionary<string, T> sourceData, string templateFilePath, out StringBuilder Message, string dataRangeStartCell, string keyCell, string changeTitleCell, string changeContent, Dictionary<string, string> columnData, string sheetName) where T : class
        {
            bool result = false;
            var message = new StringBuilder(100);
            try
            {
                FileInfo existingFile = new FileInfo(templateFilePath);
                using (ExcelPackage package = new ExcelPackage(existingFile))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[1];

                    if (!string.IsNullOrEmpty(sheetName))
                    {
                        //如果没有获取该名称的sheet，获取第一个
                        worksheet = package.Workbook.Worksheets[sheetName];
                    }
                    if (worksheet.Dimension == null)
                    {
                        message.Append("EmptyError:File is Empty");
                        Message = message;
                        return result;
                    }
                    else
                    {
                        var keyRowStart = GetRowIndex(keyCell);
                        var keyColumn = GetColumnName(keyCell)[0] - 'A' + 1;
                        var dataRowStart = GetRowIndex(dataRangeStartCell);
                        var dataColumnStart = GetColumnName(dataRangeStartCell);
                        //需获得keyColumn中的Rows行
                        worksheet.DefaultColWidth = 250;
                        var keyHeader = new Dictionary<int, string>();
                        int colEnd = worksheet.Dimension.End.Column;
                        int rowEnd = worksheet.Dimension.End.Row;
                        for (int i = (int)keyRowStart; i <= rowEnd; i++)
                        {
                            if (worksheet.Cells[i, keyColumn].Value != null)
                            {
                                keyHeader[i] = worksheet.Cells[i, keyColumn].Value.ToString();
                            }
                        }
                        //
                        //更改标题栏的年与月
                        worksheet.Cells[changeTitleCell].Value = changeContent;

                        //将sourceData转换成Dictionary<row, T>形式
                        var destData = ConvertDataSourceToRowDataDictionary<T>(sourceData, keyHeader);
                        //制作原值-固定表和制作原值-无形表
                        for (int dataRow = (int)dataRowStart; dataRow <= rowEnd; dataRow++)
                        {
                            if (worksheet.Cells[dataRow, keyColumn].Value == null)
                            {
                                break;
                            }
                            //
                            /// <param name="columnData">Key为对应单元格，Value为T的属性名，将属性值写入到对应单元格内，如//<C5,Name> <D5, Age> Name与Age均为T中的属性，表示将Name中的值写入到C5</param>
                            foreach (var columnName in columnData)
                            {
                                var cellName = columnName.Key + dataRow.ToString();
                                var propertiInfo = typeof(T).GetProperty(columnName.Value);
                                var pointData = propertiInfo.GetValue(destData[dataRow]);
                                worksheet.Cells[cellName].Value = pointData;
                            }

                        }
                    }
                    package.Save();
                    result = true;
                }
            }
            catch (Exception)
            {

                throw;
            }
            Message = message;
            return result;
        }


        #endregion

        #region 将数据写入到byte[]中
        public byte[] ExportListToExcel<T>(List<T> data, List<string> heading, bool isShowSlNo = false)
        {
            //return ExportExcel(ListToDataTable<T>(data), heading, isShowSlNo, ColumnsToTake);
            return WriteListDataToExcel(data, heading, isShowSlNo);

        }



        private byte[] WriteListDataToExcel<T>(List<T> data, List<string> heading, bool isShowSlNo)
        {
            byte[] results = null;
            using (ExcelPackage package = new ExcelPackage())
            {
                //var worksheetName = heading.Count > 0 ? heading[0] : String.Empty;
                var worksheetName = "OutPutExcel";

                ExcelWorksheet workSheet = package.Workbook.Worksheets.Add(string.Format("{0} Data", worksheetName));

                int startRowIndex = 1;
                int startColumnIndex = 1;
                //在第一列加入序号
                if (isShowSlNo)
                {
                    //加入序号                   
                    workSheet.Cells[startRowIndex, 1].Value = "序号";

                    for (int slNo = 0; slNo < data.Count; slNo++)
                    {
                        workSheet.Cells[slNo + 2, 1].Value = slNo + 1;
                    }
                    //起始列改为从第2行开始
                    startColumnIndex = 2;
                }

                //在第一行加入Heading
                if (heading != null && heading.Count > 0)
                {
                    for (int columnIndex = 0; columnIndex < heading.Count; columnIndex++)
                    {
                        workSheet.Cells[1, columnIndex + startColumnIndex].Value = heading[columnIndex];
                    }
                    //起始行改为从第2行开始
                    startRowIndex = 2;
                    workSheet.Cells[startRowIndex, startColumnIndex].LoadFromCollection(data, false);
                }
                else
                {
                    workSheet.Cells[startRowIndex, startColumnIndex].LoadFromCollection(data, true);
                }
                workSheet.Cells.AutoFitColumns();//设置列宽自适应
                results = package.GetAsByteArray();
            }

            return results;
        }

        public byte[] ExportDataTableToExcel(DataTable data, List<string> heading, bool isShowSlNo = false)
        {
            //return ExportExcel(ListToDataTable<T>(data), heading, isShowSlNo, ColumnsToTake);
            return WriteDataTableToExcel(data, heading, isShowSlNo);
        }

        private byte[] WriteDataTableToExcel(DataTable data, List<string> heading, bool isShowSlNo)
        {
            byte[] results = null;
            using (ExcelPackage package = new ExcelPackage())
            {
                //var worksheetName = heading.Count > 0 ? heading[0] : String.Empty;
                var worksheetName = "OutPutExcel";

                ExcelWorksheet workSheet = package.Workbook.Worksheets.Add(string.Format("{0} Data", worksheetName));
                workSheet.Cells.Style.Font.Name = "宋体";
                workSheet.Cells.Style.Font.Size = 11;
                int startRowIndex = 1;
                int startColumnIndex = 1;
                //在第一列加入序号
                if (isShowSlNo)
                {
                    //加入序号                   
                    workSheet.Cells[startRowIndex, 1].Value = "序号";

                    for (int slNo = 0; slNo < data.Rows.Count; slNo++)
                    {
                        workSheet.Cells[slNo + 2, 1].Value = slNo + 1;
                    }
                    //起始列改为从第2行开始
                    startColumnIndex = 2;
                }
                var coloumnWidths = new int[data.Columns.Count];
                //在第一行加入Heading
                if (heading != null && heading.Count > 0)
                {
                    for (int columnIndex = 0; columnIndex < heading.Count; columnIndex++)
                    {
                        workSheet.Cells[1, columnIndex + startColumnIndex].Value = heading[columnIndex];
                        coloumnWidths[columnIndex] = Regex.IsMatch(heading[columnIndex], @"^[+-]?/d*[.]?/d*$") ? heading[columnIndex].Length : heading[columnIndex].Length * 2;
                    }
                    //起始行改为从第2行开始
                    startRowIndex = 2;
                    workSheet.Cells[startRowIndex, startColumnIndex].LoadFromDataTable(data, false);
                }
                else
                {
                    workSheet.Cells[startRowIndex, startColumnIndex].LoadFromDataTable(data, true);
                }
                var selectData = data.Select();
                for (var i = 0; i < selectData.Length; i++)
                {
                    DataRow row = selectData[i];
                    for (var j = 0; j < data.Columns.Count; j++)
                    {
                        var cololumnStr = Convert.ToString(row[j]);
                        var cololumnStrWidth = Regex.IsMatch(cololumnStr, @"^[+-]?/d*[.]?/d*$") ? cololumnStr.Length : cololumnStr.Length * 2;
                        coloumnWidths[j] = coloumnWidths[j] < cololumnStrWidth ? cololumnStrWidth : coloumnWidths[j];
                    }
                }
                for (var i = 0; i < coloumnWidths.Length; i++)
                {
                    workSheet.Column(i + 2).Width = coloumnWidths[i];
                }
                results = package.GetAsByteArray();
            }

            return results;
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sourceData"></param>
        /// <param name="keyHeader"></param>
        /// <returns></returns>
        private Dictionary<int, T> ConvertDataSourceToRowDataDictionary<T>(Dictionary<string, T> sourceData, Dictionary<int, string> keyHeader) where T : class
        {
            var result = new Dictionary<int, T>();
            foreach (var temp in keyHeader)
            {
                try
                {
                    if (sourceData.Keys.Contains(temp.Value))
                    {
                        var colNumber = sourceData[temp.Value];
                        result.Add(temp.Key, colNumber);
                    }
                }
                catch (Exception)
                {
                    continue;
                }
            }
            return result;
        }

        internal uint GetRowIndex(string cellName)
        {
            // Create a regular expression to match the row index portion the cell name.
            Regex regex = new Regex(@"\d+");
            Match match = regex.Match(cellName);

            return uint.Parse(match.Value);
        }
        // Given a cell name, parses the specified cell to get the column name.
        internal string GetColumnName(string cellName)
        {
            // Create a regular expression to match the column name portion of the cell name.
            Regex regex = new Regex("[A-Za-z]+");
            Match match = regex.Match(cellName);

            return match.Value;
        }
    }
}
