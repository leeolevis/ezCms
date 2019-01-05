using DapperExtensions;
using EnumsNET;
using ez.Core.Excel;
using ez.Core.Helpers;
using ezModel.BaseModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;

namespace ezLay
{
    public class baseController : Controller
    {
        public static string[] CommonPostfixes = { "Controller" };
        public baseController()
        {

        }
        /// <summary>
        /// 字典缓存Key
        /// </summary>
        public const string _cacheDictList = "_cacheDictList";

        public const string _cacheCompanyList = "_cacheCompanyList";

        public const string _cacheRoleList = "_cacheRoleList";

        public const string _cacheUserList = "_cacheUserList";

        public const string _cachePrecinctList = "_cachePrecinctList";//管辖的缓存数据

        public DistributedCacheEntryOptions cacheEntryOptions = new DistributedCacheEntryOptions().SetSlidingExpiration(new TimeSpan(48, 0, 0));

        #region 弹出消息

        /// <summary>
        /// 枚举值参考http://blog.csdn.net/beauxie/article/details/60959971
        /// </summary>
        public enum LayerIconType
        {
            Success = 1,
            Erro = 2,
            Question = 3,
            Lock = 4,
            Sad = 5,
            Joy = 6,
            Sigh = 7
        }

        private const string Message = "Message"; //改成 private

        protected void RunJs(string functionname)
        {
            TempData[Message] = functionname;
        }

        protected void ShowAlertMessage(string message)
        {
            TempData[Message] = string.Format("layer.alert('{0}！');", message);
        }

        protected void ShowTipLoading(string message, bool closePage = true, bool isParent = true, string callback = "", bool isRefresh = true)
        {
            TempData[Message] = WebAlert.ShowTipLoading(message, closePage, isParent, callback, isRefresh);
        }

        /// <summary>
        /// 前台弹窗，默认为popup配置参数
        /// </summary>
        /// <param name="message">提示消息</param>
        /// <param name="iconType">提示类型</param>
        /// <param name="closePage">是否关闭</param>
        /// <param name="isParent">是否是父页面</param>
        /// <param name="callback">回调</param>
        /// <param name="isRefresh">是否刷新param>
        protected void ShowTipMessage(LayerIconType iconType, string message = "", bool closePage = true, bool isParent = true, string callback = "", bool isRefresh = true)
        {
            if (string.IsNullOrEmpty(message))
            {
                switch (iconType)
                {
                    case LayerIconType.Success:
                        message = "保存成功";
                        break;
                    case LayerIconType.Erro:
                        message = "保存失败";
                        break;
                    case LayerIconType.Question:
                        break;
                    case LayerIconType.Lock:
                        break;
                    case LayerIconType.Sad:
                        break;
                    case LayerIconType.Joy:
                        break;
                    case LayerIconType.Sigh:
                        message = "参数验证无效";
                        break;
                    default:
                        break;
                }
            }
            TempData[Message] = WebAlert.ShowTipMessage(message, (int)iconType, closePage, isParent, callback, isRefresh);
        }

        #endregion

        #region 收集ModelState验证消息

        public string GetModelStateMsg(ModelStateDictionary ModelState)
        {
            List<Verify> verifys = new List<Verify>();
            foreach (var item in ModelState.Keys)
                verifys.Add(new Verify { prop = item.ToString() });

            int i = 0;
            foreach (var s in ModelState.Values)
            {
                List<string> erromsg = new List<string>();
                foreach (var p in s.Errors)
                    erromsg.Add(p.ErrorMessage);
                verifys[i].erros = erromsg;
                i++;
            }

            verifys.Reverse();
            ez.Core.Helpers.String msg = new ez.Core.Helpers.String();
            foreach (var item in verifys)
            {
                //msg.Append($"{item.prop}验证失败<br/>");
                foreach (var erro in item.erros)
                    msg.Append($"{erro}<br/>");
            }
            return msg.ToString();
        }

        #endregion

        #region 获取登录信息
        //获取当前登录信息
        public string CurrentLoginInfo(MyClaimTypes value)
        {
            if (!HttpContext.User.Identity.IsAuthenticated) return "";
            var userInfo = HttpContext.User.Identities.First(u => u.IsAuthenticated).FindFirst(value.ToString()).Value;
            return userInfo;
        }
        #endregion

        #region 查询表达式

        public static Expression<Func<T, object>> GetExpression<T>(Predicate p) where T : class
        {
            var cloumName = p.columnItem.Split('.')[1];
            var parameter = Expression.Parameter(typeof(T), "p");
            return Expression.Lambda<Func<T, object>>(Expression.Convert(Expression.Property(parameter, cloumName), typeof(object)), parameter);
        }

        //动态构建表达式条件
        public static PredicateGroup GetSearchPredicate<T>(List<Predicate> predicates) where T : class
        {
            var pg = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };
            foreach (var p in predicates)
            {
                if (p == null || p.columnItem == null) continue;

                var operatorVal = Enums.Parse<Operator>(p.operatorItem, EnumFormat.Description).ToString();

                if (operatorVal.ToUpper() == "LIKE")
                    p.valueItem = $"%{p.valueItem}%";

                var predicate = Predicates.Field(GetExpression<T>(p), (Operator)System.Enum.Parse(typeof(Operator), operatorVal), p.valueItem);
                pg.Predicates.Add(predicate);
            }
            return pg;
        }

        //动态构建SQL条件
        public static string GetSearchSQL(List<Predicate> predicates, List<ConditionPredicate> conditionPredicates = null)
        {
            if (predicates.All(item => item == null || item.columnItem == null || string.IsNullOrEmpty(item.operatorItem) || string.IsNullOrEmpty(item.valueItem)))
                return System.String.Empty;
            if (conditionPredicates != null)
            {
                return ConditionPredicateBuilder(conditionPredicates);
            }
            else
            {
                var filterSQL = string.Empty;
                foreach (var p in predicates)
                {
                    if (p == null || p.columnItem == null || string.IsNullOrEmpty(p.operatorItem)) continue;

                    if (p.operatorItem.Equals("like"))
                        filterSQL += $" AND {p.columnItem} {p.operatorItem} '%{p.valueItem}%'";
                    else
                        filterSQL += $" AND {p.columnItem} {p.operatorItem} '{p.valueItem}'";
                }
                return filterSQL;
            }
        }

        private static string ConditionPredicateBuilder(List<ConditionPredicate> conditionPredicates)
        {
            if (conditionPredicates == null || !conditionPredicates.Any())
                return string.Empty;
            else
            {
                //conditionPredicates.Remove()
                var result = new List<string>();
                for (var index = 0; index < conditionPredicates.Count; index++)
                {
                    var conditionPredicate = conditionPredicates[index];
                    var childConditionPredicate = conditionPredicate.value as List<ConditionPredicate>;
                    if (childConditionPredicate != null)
                    {
                        var childResult = new List<string>();
                        for (var i = 0; i < childConditionPredicate.Count; i++)
                        {
                            var predicate = childConditionPredicate[i];
                            if (string.IsNullOrEmpty(predicate.name) ||
                                string.IsNullOrEmpty(predicate.@operator) ||
                                string.IsNullOrEmpty(predicate.fieldValue))
                                continue;
                            if (i != 0)
                                childResult.Add(predicate.beforeOperator);
                            if (predicate.@operator.Equals("like", StringComparison.OrdinalIgnoreCase))
                                childResult.Add($"{predicate.name} {predicate.@operator} '%{predicate.fieldValue}%'");
                            else
                                childResult.Add($"{predicate.name} {predicate.@operator} '{predicate.fieldValue}'");
                        }
                        if (index != 0)
                        {
                            result.Add(conditionPredicate.beforeOperator);
                        }
                        if (childResult.Count > 0)//每个组元素生成的sql合并为一个组
                        {
                            childResult.Insert(0, "(");
                            childResult.Add(")");
                            result.Add(string.Join(" ", childResult));
                        }
                    }
                }

                if (result.Count > 0)//将自动生成的sql作为一个整体 与 其他where条件合并，格式形如：and （动态生成的sql）
                {
                    result.Insert(0, "and");
                    result.Insert(1, "(");
                    result.Add(")");
                    return string.Join(" ", result);
                }
                else
                    return string.Empty;
            }
        }

        #endregion

        #region 导出Excel

        /// <summary>
        /// Excel导出，暂时支持实体类和DataTable数据源
        /// </summary>
        /// <param name="excelData"></param>
        /// <param name="excelName"></param>
        /// <param name="isModel"></param>
        /// <returns></returns>
        public static FileContentResult ExcelExport(dynamic excelData, string excelName, bool isDynamic = false, bool isShowSlNo = true)
        {
            var factory = ExcelEntityFactory.GetInstance();
            byte[] result = null;
            if (!isDynamic)
                result = factory.CreateWriteToExcel().ExportListToExcel(excelData, null, isShowSlNo);
            else
            {
                var dtResult = DynamicHelper.ToDataTable(excelData);
                result = factory.CreateWriteToExcel().ExportDataTableToExcel(dtResult, null, isShowSlNo);
            }
            var returnResult =
                new FileContentResult(result, factory.CreateWriteToExcel().ExcelContentType)
                {
                    FileDownloadName = $"{excelName}-{DateTime.Now:yyyyMMddHHmmss}.xlsx"
                };
            return returnResult;
        }

        /// <summary>
        /// Excel导出，暂时支持实体类和DataTable数据源
        /// </summary>
        /// <param name="excelData"></param>
        /// <param name="excelName"></param>
        /// <param name="isModel"></param>
        /// <returns></returns>
        public static FileContentResult ExcelExport<T>(List<T> excelData, string excelName, bool isDynamic = false, bool isShowSlNo = true)
        {
            var factory = ExcelEntityFactory.GetInstance();
            byte[] result = null;
            if (!isDynamic)
                result = factory.CreateWriteToExcel().ExportListToExcel(excelData, null, isShowSlNo);
            else
            {
                var dtResult = ListHelper.ToDataTable(excelData);
                result = factory.CreateWriteToExcel().ExportDataTableToExcel(dtResult, null, isShowSlNo);
            }
            var returnResult =
                new FileContentResult(result, factory.CreateWriteToExcel().ExcelContentType)
                {
                    FileDownloadName = $"{excelName}-{DateTime.Now:yyyyMMddHHmmss}.xlsx"
                };
            return returnResult;
        }

        public static FileContentResult ExcelExport(dynamic excelData, string excelName, List<string> heading, bool isShowSlNo, List<string> keySelectors, List<string> tails)
        {
            var factory = ExcelEntityFactory.GetInstance();
            byte[] result = null;
            // var listResult = DynamicHelper.ToDynamicList(DynamicHelper.ToDataTable(excelData));
            DataTable dtResult = DynamicHelper.ToDataTable(excelData);
            result = factory.CreateWriteToExcel().ExportListToExcel(dtResult, heading, isShowSlNo, keySelectors, tails);
            var returnResult =
                new FileContentResult(result, factory.CreateWriteToExcel().ExcelContentType)
                {
                    FileDownloadName = $"{excelName}-{DateTime.Now:yyyyMMddHHmmss}.xlsx"
                };
            return returnResult;
        }

        public static FileContentResult ExcelExport<T>(List<T> excelData, string excelName, List<string> heading, bool isShowSlNo, List<string> keySelectors, List<string> tails, bool isGroupTotalExcel = false, List<string> coloumsColor = null, List<string> hideColoums = null)
        {
            var factory = ExcelEntityFactory.GetInstance();
            byte[] result = null;
            DataTable dtResult = ListHelper.ToDataTable(excelData, hideColoums);
            result = !isGroupTotalExcel
                    ? factory.CreateWriteToExcel().ExportListToExcel(dtResult, heading, isShowSlNo, keySelectors, tails)
                    : factory.CreateWriteToExcel().ExportGroupListToExcel(dtResult, heading, isShowSlNo, keySelectors, tails, coloumsColor);
            var returnResult =
                new FileContentResult(result, factory.CreateWriteToExcel().ExcelContentType)
                {
                    FileDownloadName = $"{excelName}-{DateTime.Now:yyyyMMddHHmmss}.xlsx"
                };
            return returnResult;
        }


        #endregion

        #region 归档Excel 这个地方这样写，可能会被Hangfire调用，由于Hangfire会新建线程（跨线程的时候不能共享上下文），所以路径是传过来的

        public IActionResult ExcelBackupBase(dynamic excelData, string excelName, bool isDynamic = false, string savePath = "")
        {
            if (!Directory.Exists(savePath))
                Directory.CreateDirectory(savePath);

            if (Directory.Exists(savePath))
            {
                var factory = ExcelEntityFactory.GetInstance();
                byte[] result = null;
                if (!isDynamic)
                    result = factory.CreateWriteToExcel().ExportListToExcel(excelData, null, true);
                else
                {
                    var dtResult = DynamicHelper.ToDataTable(excelData);
                    result = factory.CreateWriteToExcel().ExportDataTableToExcel(dtResult, null, true);
                }
                string fileName = $"{excelName}-{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                System.IO.File.WriteAllBytes($@"{savePath}\{fileName}", result);
            }
            return new EmptyResult();
        }

        #endregion

        #region 文件下载

        public FileResult DownWebFile(string filePath, string name = "")
        {
            string fileName = name;
            if (string.IsNullOrEmpty(name))
                fileName = System.IO.Path.GetFileName(filePath);
            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/x-msdownload", fileName);
        }
        #endregion

        #region 获取扩展列名或所有列名
        /// <summary>
        /// 获取列名
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        public static IEnumerable<string> GetColumnName(IDatabase _database, string tableName)
        {
            var Sql = $"select column_name as 列名 from Information_schema.columns where table_Name = @tableName and column_name like '文_'UNION select column_name as 列名 from Information_schema.columns where table_Name = @tableName and column_name like '时_' UNION select column_name as 列名 from Information_schema.columns where table_Name = @tableName and column_name like '数_'";
            var result = _database.GetListSQL<string>(Sql, new { tableName = tableName });
            return result;
        }

        /// <summary>
        /// 获取所有列名
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        public static IEnumerable<string> GetColumnNameAll(IDatabase _database, string tableName)
        {
            var Sql = $"select DISTINCT column_name as 列名 from Information_schema.columns where table_Name =@tableName";
            var result = _database.GetListSQL<string>(Sql, new { tableName = tableName });
            return result;
        }
        #endregion
    }
}
