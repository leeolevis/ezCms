﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DapperExtensions.Sql
{
    public class DB2Dialect : SqlDialectBase
    {
        public override string GetIdentitySql(string tableName)
        {
            return "SELECT CAST(IDENTITY_VAL_LOCAL() AS BIGINT) AS \"ID\" FROM SYSIBM.SYSDUMMY1";
        }

        public override string GetPagingSql(string sql, int page, int resultsPerPage, IDictionary<string, object> parameters)
        {
            var startValue = ((page - 1) * resultsPerPage) + 1;
            var endValue = (page * resultsPerPage);
            return GetSetSql(sql, startValue, endValue, parameters);
        }

        public override string GetSetSql(string sql, int firstResult, int maxResults, IDictionary<string, object> parameters)
        {
            if (string.IsNullOrEmpty(sql))
            {
                throw new ArgumentNullException("SQL");
            }

            if (parameters == null)
            {
                throw new ArgumentNullException("Parameters");
            }

            var selectIndex = GetSelectEnd(sql) + 1;
            var orderByClause = GetOrderByClause(sql) ?? "ORDER BY CURRENT_TIMESTAMP";


            var projectedColumns = GetColumnNames(sql).Aggregate(new StringBuilder(), (sb, s) => (sb.Length == 0 ? sb : sb.Append(", ")).Append(GetColumnName("_TEMP", s, null)), sb => sb.ToString());
            var newSql = sql
                .Replace(" " + orderByClause, string.Empty)
                .Insert(selectIndex, string.Format("ROW_NUMBER() OVER(ORDER BY {0}) AS {1}, ", orderByClause.Substring(9), GetColumnName(null, "_ROW_NUMBER", null)));

            var result = string.Format("SELECT {0} FROM ({1}) AS \"_TEMP\" WHERE {2} BETWEEN @_pageStartRow AND @_pageEndRow",
                projectedColumns.Trim(), newSql, GetColumnName("_TEMP", "_ROW_NUMBER", null));

            parameters.Add("@_pageStartRow", firstResult);
            parameters.Add("@_pageEndRow", maxResults);
            return result;
        }

        protected string GetOrderByClause(string sql)
        {
            var orderByIndex = sql.LastIndexOf(" ORDER BY ", StringComparison.InvariantCultureIgnoreCase);
            if (orderByIndex == -1)
            {
                return null;
            }

            var result = sql.Substring(orderByIndex).Trim();

            var whereIndex = result.IndexOf(" WHERE ", StringComparison.InvariantCultureIgnoreCase);
            return whereIndex == -1 ? result : result.Substring(0, whereIndex).Trim();
        }

        protected int GetFromStart(string sql)
        {
            var selectCount = 0;
            var words = sql.Split(' ');
            var fromIndex = 0;
            foreach (var word in words)
            {
                if (word.Equals("SELECT", StringComparison.InvariantCultureIgnoreCase))
                {
                    selectCount++;
                }

                if (word.Equals("FROM", StringComparison.InvariantCultureIgnoreCase))
                {
                    selectCount--;
                    if (selectCount == 0)
                    {
                        break;
                    }
                }

                fromIndex += word.Length + 1;
            }

            return fromIndex;
        }

        protected virtual int GetSelectEnd(string sql)
        {
            if (sql.StartsWith("SELECT DISTINCT", StringComparison.InvariantCultureIgnoreCase))
            {
                return 15;
            }

            if (sql.StartsWith("SELECT", StringComparison.InvariantCultureIgnoreCase))
            {
                return 6;
            }

            throw new ArgumentException("SQL must be a SELECT statement.", "sql");
        }

        protected virtual IList<string> GetColumnNames(string sql)
        {
            var start = GetSelectEnd(sql);
            var stop = GetFromStart(sql);
            var columnSql = sql.Substring(start, stop - start).Split(',');
            var result = new List<string>();
            foreach (var c in columnSql)
            {
                var index = c.IndexOf(" AS ", StringComparison.InvariantCultureIgnoreCase);
                if (index > 0)
                {
                    result.Add(c.Substring(index + 4).Trim());
                    continue;
                }

                var colParts = c.Split('.');
                result.Add(colParts[colParts.Length - 1].Trim());
            }

            return result;
        }
    }
}