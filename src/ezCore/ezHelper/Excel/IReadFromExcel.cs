using System.Collections.Generic;
using System.Text;

namespace ez.Core.Excel
{
    public interface IReadFromExcel
    {
        /// <summary>
        /// 通过读取
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cellHeard"> cellHeard为<属性名,标题名>，T类型的属性名[Display(Name="属性名")]及Excel表中的行标题名称</param>
        /// <param name="filePath">上传的Excel文件路径</param>
        /// <param name="errorMsg">传出的错误信息</param>
        /// <param name="sheetName">Excel表中的工作簿名称，默认为第1个</param>
        /// <param name="startCellName">Excel中数据行开始单元格,包括标题行</param>
        /// <param name="mergeTitleRow">标题行占用的行数</param>
        /// <paramref name="keyColumn"/>返回的Dictionary<string,T>中的Key所在列，如如A列中为"部门、姓名、出生日期"，并且需保证该行不能有重复值</param>
        /// <returns>返回一个Dictionary数据结构，Key为关键列中的内容，T为传入的对象</returns>
        Dictionary<string, T> ExcelToEntityDictionary<T>(Dictionary<string, string> cellHeard, string filePath, out StringBuilder errorMsg, string sheetName = null, string startCellName = "A1", int mergeTitleRow = 1, int keyColumn = 1) where T : new();

        /// <summary>
        /// 根据cellHeard的内容，填充类型为T的List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cellHeard">cellHeard为<属性名,标题名>，T类型的属性名[Display(Name="属性名")]及Excel表中的行标题名称</param>
        /// <param name="filePath">上传的Excel文件路径</param>
        /// <param name="errorMsg">传出的错误信息</param>
        /// <param name="sheetName"></param>
        /// <param name="startCellName">Excel表中的工作簿名称，默认为第1个</param>
        /// <param name="mergeTitleRow">标题行占用的行数</param>
        /// <returns>返回一个类型为T的List</returns>
        List<T> ExcelToEntityList<T>(Dictionary<string, string> cellHeard, string filePath, out StringBuilder errorMsg, string sheetName = null, string startCellName = "A1", int mergeTitleRow = 1) where T : new();
    }
}
