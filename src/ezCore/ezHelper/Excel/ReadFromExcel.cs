using OfficeOpenXml;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;


namespace ez.Core.Excel
{
    public class ReadFromExcel : IReadFromExcel
    {
        #region 把Excel文件转换为Dictionary<string, T>
        /// <summary>
        /// Dictionary<string, T> Key为keyColumn列的内容，并且需保证该行不能有重复值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cellHeard"><string,string>T类型的属性名及Excel表中的行标题名称</param>
        /// <param name="filePath"></param>
        /// <param name="errorMsg"></param>
        /// <param name="sheetName"></param>
        /// <param name="startCellName"></param>
        /// <param name="mergeTitleRow"></param>
        /// <returns></returns>
        public Dictionary<string, T> ExcelToEntityDictionary<T>(Dictionary<string, string> cellHeard, string filePath, out StringBuilder errorMsg, string sheetName = null, string startCellName = "A1", int mergeTitleRow = 1, int keyColumn = 1) where T : new()
        {
            Dictionary<string, T> enlist = new Dictionary<string, T>();
            errorMsg = new StringBuilder(100);
            try
            {
                if (Regex.IsMatch(filePath, ".xlsx$")) //2007处理
                {
                    enlist = Excel2007ToEntityDictionaryMergerTitleRow<T>(cellHeard, filePath, out errorMsg, sheetName, startCellName, mergeTitleRow, keyColumn);
                }
                else
                {
                    errorMsg.Append("TypeError:Only support .xlsx!");
                }
                return enlist;
            }
            catch (Exception ex)
            {
                throw new NotSupportedException("无法读取文件内容！ " + ex.Message);
            }
        }
        /// <summary>
        /// Excel2007s to entity list merger title row.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cellHeader">The cell header.</param>
        /// <param name="filePath">The file path.</param>
        /// <param name="errorMsg">The error MSG.</param>
        /// <param name="sheetName">Name of the sheet.</param>
        /// <param name="startCellName">Start name of the cell.</param>
        /// <param name="mergeTitleRow">标题所占行数.</param>
        /// <returns>Dictionary，指定Column中的内容。</returns>
        private Dictionary<string, T> Excel2007ToEntityDictionaryMergerTitleRow<T>(Dictionary<string, string> cellHeader, string filePath, out StringBuilder errorMsg, string sheetName = null, string startCellName = "A1", int mergeTitleRow = 1, int keyColumn = 1) where T : new()
        {
            errorMsg = new StringBuilder(100);// 错误信息,Excel转换到实体对象时，会有格式的错误信息
            Dictionary<string, T> enlist = new Dictionary<string, T>(); // 转换后的集合

            List<string> keys = cellHeader.Keys.ToList();// 要赋值的实体对象属性名称            
            try
            {
                FileInfo existingFile = new FileInfo(filePath);
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
                        errorMsg.Append("EmptyError:File is Empty");
                        return enlist;
                    }
                    else
                    {

                        int rowStart = (int)GetRowIndex(startCellName);
                        string startColumn = GetColumnName(startCellName);
                        int colStart = (int)startColumn[0] - 'A' + 1;
                        //获得标题字典，将每列标题添加到字典中
                        int rowEnd = worksheet.Dimension.End.Row;
                        int colEnd = worksheet.Dimension.End.Column;
                        //获得<列标题,colIndex>字典
                        var titleHeader = new Dictionary<string, int>();
                        //for (int i = colStart; i <= colEnd; i++)
                        //{
                        //    var titleHeaderName = new StringBuilder(50);
                        //    //获取主、副标题，如主标题为“主标题1”，副标题为“副标题1”，mergeTitleRow为2，生成的Dictionary为Dictionary<主标题1副标题1,colIndex>，如果标题为空，则跳过该列所有内容
                        //    for (int titleRow = rowStart; titleRow <= mergeTitleRow; titleRow++)
                        //    {

                        //        titleHeaderName.Append(worksheet.Cells[titleRow, i] != null ? worksheet.Cells[titleRow, i].Value.ToString() : "Null");
                        //    }
                        //    titleHeader[titleHeaderName.ToString()] = i;
                        //}
                        titleHeader = GetExcelTitleHeader(worksheet, rowStart, colStart, colEnd, mergeTitleRow);
                        //转换字典，变成<属性名,colIndex>
                        var dicHeader = TransferCellHeadToPropertyHead(cellHeader, titleHeader);
                        //获得Class的所有属性名
                        List<PropertyInfo> propertyInfoList = new List<PropertyInfo>(typeof(T).GetProperties());


                        for (int row = rowStart + mergeTitleRow; row <= rowEnd; row++)
                        {
                            string errStr = ""; //// 当前行转换时，是否有错误信息，格式为：第1行数据转换异常：XXX列；
                            int col = colStart;

                            T en = new T();

                            //解析T的属性
                            foreach (PropertyInfo p in propertyInfoList)
                            {
                                try
                                {

                                    if (dicHeader.ContainsKey(p.Name))
                                    {
                                        var cell = worksheet.Cells[row, dicHeader[p.Name]];
                                        p.SetValue(en, GetExcelCellToProperty(p.PropertyType, cell), null);
                                    }
                                }
                                catch (KeyNotFoundException)
                                {

                                    //throw;
                                    if (errStr.Length == 0)
                                    {
                                        errStr = "第" + row + "行数据转换异常";
                                    }
                                    errStr += dicHeader[p.Name] + "列；";
                                }

                            }

                            // 若有错误信息，就添加到错误信息里
                            if (errStr.Length > 0)
                            {
                                errorMsg.AppendLine(errStr);
                            }
                            try
                            {
                                enlist.Add(worksheet.Cells[row, keyColumn].Value != null ? worksheet.Cells[row, keyColumn].Value.ToString() : System.Guid.NewGuid().ToString(), en);
                                //不能出现相同Key

                            }
                            catch (ArgumentException)
                            {
                                //捕获重复键值
                                var key2 = worksheet.Cells[row, keyColumn].Value.ToString() + System.Guid.NewGuid().ToString();
                                enlist.Add(key2, en);
                            }
                        }
                    }
                }

                return enlist;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        #region 把Excel文件转换为List<T>        
        /// <summary>
        /// 把Excel文件转换为List<T>
        /// 1、读取Excel文件并以此初始化一个工作簿(Workbook)；
        ///2、从工作簿上获取一个工作表(Sheet)；默认为工作薄的第一个工作表；
        ///3、遍历工作表所有的行(row)；第一行为标题行,生成一个包含行索引的Dictionary；
        ///4、提供一个类属性名与Excel标题名相对应的Dictionary
        ///4、遍历行的每一个单元格(cell)，根据一定的规律赋值给对象的属性。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cellHeard">The cell heard.</param>
        /// <param name="filePath">The file path.</param>
        /// <param name="errorMsg">The error MSG.</param>
        /// <param name="sheetName">从A-Z开始，AA之后开始的不支持</param>
        /// <returns>返回只属于该类所拥有的类属性，Excel中的其他字段不会读出来</returns>
        /// <exception cref="NotSupportedException"></exception>
        /// 
        public List<T> ExcelToEntityList<T>(Dictionary<string, string> cellHeard, string filePath, out StringBuilder errorMsg, string sheetName = null, string startCellName = "A1", int mergeTitleRow = 1) where T : new()
        {
            List<T> enlist = new List<T>();
            errorMsg = new StringBuilder(100);
            try
            {
                if (Regex.IsMatch(filePath, ".xlsx$")) //2007处理
                {
                    enlist = Excel2007ToEntityListMergerTitleRow<T>(cellHeard, filePath, out errorMsg, sheetName, startCellName, mergeTitleRow);
                }
                else
                {
                    errorMsg.Append("TypeError:Only support .xlsx!");
                }
                return enlist;
            }
            catch (Exception ex)
            {

                throw new NotSupportedException("无法读取文件内容！ " + ex.Message);
            }
        }


        /// <summary>
        /// Excel2007s to entity list merger title row.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cellHeader">The cell header.</param>
        /// <param name="filePath">The file path.</param>
        /// <param name="errorMsg">The error MSG.</param>
        /// <param name="sheetName">Name of the sheet.</param>
        /// <param name="startCellName">Start name of the cell.</param>
        /// <param name="mergeTitleRow">标题所占行数.</param>
        /// <returns>List&lt;T&gt;.</returns>
        private List<T> Excel2007ToEntityListMergerTitleRow<T>(Dictionary<string, string> cellHeader, string filePath, out StringBuilder errorMsg, string sheetName = null, string startCellName = "A1", int mergeTitleRow = 1) where T : new()
        {
            errorMsg = new StringBuilder(100);// 错误信息,Excel转换到实体对象时，会有格式的错误信息
            List<T> enlist = new List<T>(); // 转换后的集合

            List<string> keys = cellHeader.Keys.ToList();// 要赋值的实体对象属性名称            
            try
            {
                FileInfo existingFile = new FileInfo(filePath);
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
                        errorMsg.Append("EmptyError:File is Empty");
                        return enlist;
                    }
                    else
                    {

                        int rowStart = (int)GetRowIndex(startCellName);
                        string startColumn = GetColumnName(startCellName);
                        int colStart = (int)startColumn[0] - 'A' + 1;
                        //获得标题字典，将每列标题添加到字典中
                        int rowEnd = worksheet.Dimension.End.Row;
                        int colEnd = worksheet.Dimension.End.Column;
                        var titleHeader = new Dictionary<string, int>();
                        //for (int i = colStart; i <= colEnd; i++)
                        //{
                        //    var titleHeaderName = new StringBuilder(50);
                        //    if(mergeTitleRow > 1)
                        //    {
                        //        for (int titleRow = rowStart; titleRow <= mergeTitleRow; titleRow++)
                        //        {
                        //            titleHeaderName.Append(worksheet.Cells[titleRow, i] != null ? worksheet.Cells[titleRow, i].Value.ToString() : "NULL");
                        //        }
                        //    }
                        //    else
                        //    {
                        //        titleHeaderName.Append((worksheet.Cells[rowStart, i] != null ? worksheet.Cells[rowStart, i].Value.ToString() : "NULL"));
                        //    }
                        //    titleHeader[titleHeaderName.ToString()] = i;
                        //}
                        titleHeader = GetExcelTitleHeader(worksheet, rowStart, colStart, colEnd, mergeTitleRow);
                        //转换字典，变成<属性名,colIndex>
                        var dicHeader = TransferCellHeadToPropertyHead(cellHeader, titleHeader);
                        if (dicHeader == null) return null;
                        //获得Class的所有属性名
                        List<PropertyInfo> propertyInfoList = new List<PropertyInfo>(typeof(T).GetProperties());


                        for (int row = rowStart + mergeTitleRow; row <= rowEnd; row++)
                        {
                            string errStr = ""; //// 当前行转换时，是否有错误信息，格式为：第1行数据转换异常：XXX列；
                            int col = colStart;

                            T en = new T();

                            //解析T的属性
                            foreach (PropertyInfo p in propertyInfoList)
                            {
                                try
                                {
                                    if (dicHeader.ContainsKey(p.Name))
                                    {
                                        var cell = worksheet.Cells[row, dicHeader[p.Name]];
                                        p.SetValue(en, GetExcelCellToProperty(p.PropertyType, cell), null);
                                    }
                                }
                                catch (KeyNotFoundException)
                                {

                                    //throw;
                                    if (errStr.Length == 0)
                                    {
                                        errStr = "第" + row + "行数据转换异常";
                                    }
                                    errStr += dicHeader[p.Name] + "列；";
                                }

                            }

                            // 若有错误信息，就添加到错误信息里
                            if (errStr.Length > 0)
                            {
                                errorMsg.AppendLine(errStr);
                            }
                            enlist.Add(en);

                        }
                    }
                }

                return enlist;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        private Dictionary<string, int> GetExcelTitleHeader(ExcelWorksheet worksheet, int rowStart, int colStart, int colEnd, int mergeTitleRow)
        {
            var titleHeader = new Dictionary<string, int>();
            var titleHeaderName = new StringBuilder(50);
            for (int i = colStart; i <= colEnd; i++)
            {
                titleHeaderName.Clear();
                if (mergeTitleRow > 1)
                {
                    //获取主、副标题，如主标题为“主标题1”，副标题为“副标题1”，mergeTitleRow为2，生成的Dictionary为Dictionary<主标题1副标题1,colIndex>，如果标题为空，则跳过该列所有内容
                    for (int titleRow = rowStart; titleRow <= mergeTitleRow; titleRow++)
                    {
                        titleHeaderName.Append(worksheet.Cells[titleRow, i] != null ? worksheet.Cells[titleRow, i].Value.ToString() : "NULL");
                    }
                }
                else
                {
                    titleHeaderName.Append((worksheet.Cells[rowStart, i] != null ? worksheet.Cells[rowStart, i].Value.ToString() : "NULL"));
                }
                titleHeader[titleHeaderName.ToString()] = i;
            }
            return titleHeader;
        }

        /// <summary>
        /// Excel2007s to entity list2.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cellHeader">The cell heard.</param>
        /// <param name="filePath">The file path.</param>
        /// <param name="errorMsg">The error MSG.</param>
        /// <param name="sheetName">Name of the sheet.</param>
        /// <param name="startCellName">Start name of the cell.</param>
        /// <returns>List&lt;T&gt;.</returns>
        private List<T> Excel2007ToEntityList<T>(Dictionary<string, string> cellHeader, string filePath, out StringBuilder errorMsg, string sheetName = null, string startCellName = "A1") where T : new()
        {
            errorMsg = new StringBuilder(100);// 错误信息,Excel转换到实体对象时，会有格式的错误信息
            List<T> enlist = new List<T>(); // 转换后的集合

            List<string> keys = cellHeader.Keys.ToList();// 要赋值的实体对象属性名称            
            try
            {
                FileInfo existingFile = new FileInfo(filePath);
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
                        errorMsg.Append("EmptyError:File is Empty");
                        return enlist;
                    }
                    else
                    {

                        int rowStart = (int)GetRowIndex(startCellName);
                        string startColumn = GetColumnName(startCellName);
                        int colStart = (int)startColumn[0] - 'A' + 1;
                        //获得标题字典，将每列标题添加到字典中
                        int rowEnd = worksheet.Dimension.End.Row;
                        int colEnd = worksheet.Dimension.End.Column;
                        var titleHeader = new Dictionary<string, int>();
                        for (int i = colStart; i <= colEnd; i++)
                        {
                            titleHeader[worksheet.Cells[rowStart, i].Value.ToString()] = i;
                        }
                        //转换字典，变成<属性名,colIndex>
                        var dicHeader = TransferCellHeadToPropertyHead(cellHeader, titleHeader);
                        //获得Class的所有属性名
                        List<PropertyInfo> propertyInfoList = new List<PropertyInfo>(typeof(T).GetProperties());


                        for (int row = rowStart + 1; row <= rowEnd; row++)
                        {
                            string errStr = ""; //// 当前行转换时，是否有错误信息，格式为：第1行数据转换异常：XXX列；
                            int col = colStart;

                            T en = new T();

                            //解析T的属性
                            foreach (PropertyInfo p in propertyInfoList)
                            {
                                try
                                {
                                    var cell = worksheet.Cells[row, dicHeader[p.Name]];
                                    p.SetValue(en, GetExcelCellToProperty(p.PropertyType, cell), null);
                                }
                                catch (KeyNotFoundException)
                                {

                                    //throw;
                                    if (errStr.Length == 0)
                                    {
                                        errStr = "第" + row + "行数据转换异常";
                                    }
                                    errStr += dicHeader[p.Name] + "列；";
                                }

                            }

                            // 若有错误信息，就添加到错误信息里
                            if (errStr.Length > 0)
                            {
                                errorMsg.AppendLine(errStr);
                            }
                            enlist.Add(en);

                        }
                    }
                }

                return enlist;
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// 从Excel获取值传递到对象的属性里
        /// </summary>
        /// <param name="distanceType">目标对象类型</param>
        /// <param name="sourceCell">对象属性的值</param>
        private object GetExcelCellToProperty(Type distanceType, ExcelRange sourceCell)
        {
            object rs = distanceType.GetTypeInfo().IsValueType ? Activator.CreateInstance(distanceType) : null;

            // 1.判断传递的单元格是否为空
            if (sourceCell == null || string.IsNullOrEmpty(sourceCell.ToString()))
            {
                return rs;
            }

            string valueDataType = distanceType.Name;

            // 在这里进行特定类型的处理
            switch (valueDataType.ToLower()) // 以防出错，全部小写
            {
                case "string":
                    rs = sourceCell.GetValue<string>();
                    break;
                case "bool":
                case "boolean":
                    rs = sourceCell.GetValue<bool>();
                    break;
                case "int16":
                    rs = sourceCell.GetValue<short>();
                    break;
                case "int":
                case "int32":
                    rs = sourceCell.GetValue<int>();
                    break;
                case "int64":
                    rs = sourceCell.GetValue<long>();
                    break;
                case "float":
                case "single":
                    rs = sourceCell.GetValue<float>();
                    break;
                case "double":
                    rs = sourceCell.GetValue<double>();
                    break;
                case "decimal":
                    rs = sourceCell.GetValue<decimal>();
                    break;
                case "datetime":
                    rs = sourceCell.GetValue<DateTime>();
                    break;
                case "guid":
                    rs = sourceCell.GetValue<Guid>();
                    break;
                case "char":
                    rs = sourceCell.GetValue<char>();
                    break;
                case "byte":
                    rs = sourceCell.GetValue<byte>();
                    break;
                default:
                    break;

            }
            return rs;
        }

        /// <summary>
        /// cellHeard中为<属性名,标题名>，titleHeard为<标题名, colNumber>，需转换成<属性名,colNumber>
        /// </summary>
        /// <param name="cellHead"></param>
        /// <param name="titleHead"></param>
        /// <returns></returns>
        private Dictionary<string, int> TransferCellHeadToPropertyHead(Dictionary<string, string> cellHead, Dictionary<string, int> titleHead)
        {
            var result = new Dictionary<string, int>();
            var sameHeadCount = 0;
            foreach (var temp in cellHead)
            {
                try
                {
                    if (titleHead.Keys.Contains(temp.Value))
                    {
                        var colNumber = titleHead[temp.Value];
                        result.Add(temp.Key, colNumber);
                        sameHeadCount++;
                    }
                }
                catch (Exception)
                {

                    continue;

                }
            }
            return sameHeadCount==cellHead.Count ? result:null;
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
            Regex regex = new Regex("[A-Za-z]{1,2}");
            Match match = regex.Match(cellName);

            return match.Value;
        }
        #endregion
    }
}
