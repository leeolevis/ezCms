using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ezModel.BaseModel
{
    public class ConditionPredicate
    {
        public string name { get; set; }
        public string type { get; set; }
        public string objectId { get; set; }
        public string @operator { get; set; }
        public List<ConditionPredicate> value { get; set; }
        public string fieldValue { get; set; }
        public string afterOperator { get; set; }
        public string beforeOperator { get; set; }
    }
    public class Predicate
    {
        public string objectId { get; set; }
        /// <summary>
        /// 列名
        /// </summary>
        public string columnItem { get; set; }
        /// <summary>
        /// 操作符
        /// </summary>
        public string operatorItem { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public string valueItem { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    public class SearchAttribute : Attribute
    {

        public const string String = "System.String";
        public const string Int = "System.Number";
        public const string Double = "System.Double";
        public const string DateTime = "System.DateTime";
        public const string Date = "System.Date";
        public const string Api = "System.Api";

        public static List<SelectListItem> DefaultOperators =
            new List<SelectListItem>()
            {
                new SelectListItem {Text = "等于", Value = "="},
                new SelectListItem {Text = "大于", Value = ">"},
                new SelectListItem {Text = "大于或等于", Value = ">="},
                new SelectListItem {Text = "小于", Value = "<"},
                new SelectListItem {Text = "小于或等于", Value = "<="},
                new SelectListItem {Text = "模糊", Value = "like"}
            };

        private List<KeyValuePair<string, string>> _operators;
        public List<KeyValuePair<string, string>> Operators
        {
            get
            {
                if (string.IsNullOrEmpty(this.OperatorsString))
                {
                    _operators = SearchAttribute.DefaultOperators.Select(item => new KeyValuePair<string, string>(item.Text, item.Value)).ToList();
                }
                else
                {
                    var operators = Newtonsoft.Json.JsonConvert
                        .DeserializeObject<List<KeyValuePair<string, string>>>(this.OperatorsString);
                    _operators = operators
                        .Where(item => !string.IsNullOrEmpty(item.Key) && !string.IsNullOrEmpty(item.Value))
                        .ToList();
                }

                return _operators;
            }
            set { _operators = value; }
        }

        public string OperatorsString { get; set; }
        private string _FiledName;
        public string FiledName
        {
            get { return _FiledName; }
            set { _FiledName = value; }
        }

        private string _description;


        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public bool Required { get; set; }
        public string Type { get; set; }
        public string ApiAddress { get; set; }
        public string Placeholder { get; set; }
        public string DefaultValue { get; set; }
    }
}
