using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace ez.Core.Helpers
{
    public static class ListHelper
    {
        /// <summary>
        /// List转DataTable
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(List<T> items,List<string> hideItem = null)
        {
            var table = new DataTable(typeof(T).Name);

            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(item=> hideItem == null || !hideItem.Contains(item.Name)).ToArray();
            foreach (PropertyInfo prop in props)
            {
                Type type = GetCoreType(prop.PropertyType);
                table.Columns.Add(prop.Name, type);
            }

            foreach (T item in items)
            {
                var values = new object[props.Length];

                for (int i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null);
                }

                table.Rows.Add(values);
            }

            return table;
        }
        
        /// <summary>
        /// 判断类型是否为空
        /// </summary>
        public static bool IsNullable(Type type)
        {
            return !type.IsValueType || (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>));
        }

        /// <summary>
        /// 如果type为Nullable，则返回底层类型，否则返回该类型
        /// </summary>
        public static Type GetCoreType(Type type)
        {
            if (type != null && IsNullable(type))
            {
                if (!type.IsValueType)
                {
                    return type;
                }
                else
                {
                    return Nullable.GetUnderlyingType(type);
                }
            }
            else
            {
                return type;
            }
        }
    }
}
