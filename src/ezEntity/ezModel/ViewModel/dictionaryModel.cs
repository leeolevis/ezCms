using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ezModel.ViewModel
{
    public class dictionaryModel
    {

        /// <summary>
        /// id
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 父id
        /// </summary>
        public int pid { get; set; } = 0;

        public string pname { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Required(ErrorMessage = "名称不能为空")]
        [StringLength(32, MinimumLength = 1, ErrorMessage = "名称必须在{2}~{1}个字符")]
        public string name { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        [Required(ErrorMessage = "名称不能为空")]
        [StringLength(32, MinimumLength = 1, ErrorMessage = "名称必须在{2}~{1}个字符")]
        public string val { get; set; }

        /// <summary>
        /// 代码
        /// </summary>
        [Remote(areaName: "", action: "ExistDictCode", controller: "Verify", AdditionalFields = "code,ccode", ErrorMessage = "代码已存在")]
        public string code { get; set; }

        public string ccode => code;

        /// <summary>
        /// 类型
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        /// 是否系统
        /// </summary>
        public bool isystem { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? modifyon { get; set; } = DateTime.Now;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public virtual List<dictionaryModel> exts { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public virtual List<dictionaryModel> children { get; set; }

    }
}
