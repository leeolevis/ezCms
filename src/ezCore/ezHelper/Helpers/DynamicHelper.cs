using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace ez.Core.Helpers
{
    public static class DynamicHelper
    {
        /// <summary>
        /// dynamic转DataTable
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public static DataTable ToDataTable(IEnumerable<dynamic> items)
        {
            DataTable dtDataTable = new DataTable();
            if (!items.Any()) return dtDataTable;
            bool initColumns = false;
            foreach (var item in items)
            {
                DataRow dr = dtDataTable.NewRow();
                var result = (IDictionary<string, object>)item;
                if (!initColumns)
                {
                    result.Keys.ToList().ForEach(col => dtDataTable.Columns.Add(col));
                    initColumns = true;
                }
                foreach (var key in result.Keys)
                {
                    object val = string.Empty;
                    result.TryGetValue(key, out val);
                    dr[key] = val;
                }
                dtDataTable.Rows.Add(dr);
            }
            return dtDataTable;
        }

        /// <summary>
        /// 将DataTable 转换成 List[dynamic]
        /// reverse 反转：控制返回结果中是只存在 FilterField 指定的字段,还是排除.
        /// [flase 返回FilterField 指定的字段]|[true 返回结果剔除 FilterField 指定的字段]
        /// FilterField  字段过滤，FilterField 为空 忽略 reverse 参数；返回DataTable中的全部数
        /// </summary>
        /// <param name="table">DataTable</param>
        /// <param name="reverse">
        /// 反转：控制返回结果中是只存在 FilterField 指定的字段,还是排除.
        /// [flase 返回FilterField 指定的字段]|[true 返回结果剔除 FilterField 指定的字段]
        ///</param>
        /// <param name="fields">字段过滤，FilterField 为空 忽略 reverse 参数；返回DataTable中的全部数据</param>
        /// <returns>List[dynamic]</returns>
        public static List<dynamic> ToDynamicList(DataTable table, bool reverse = true, params string[] fields)
        {
            var modelList = new List<dynamic>();
            foreach (DataRow row in table.Rows)
            {
                dynamic model = new ExpandoObject();
                var dict = (IDictionary<string, object>)model;
                foreach (DataColumn column in table.Columns)
                {
                    if (fields.Length == 0 || reverse == !fields.Contains(column.ColumnName))
                    {
                        dict[column.ColumnName] = row[column];
                    }
                }
                modelList.Add(model);
            }
            return modelList;
        }
    }
}
