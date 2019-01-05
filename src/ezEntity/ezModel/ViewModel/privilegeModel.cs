using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ezModel.ViewModel
{
    public class privilegeModel
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
        /// 0后台功能，1界面功能，2界面功能-带快捷方式
        /// </summary>
        [Required(ErrorMessage = "类型不能为空")]
        public int? type { get; set; }

        /// <summary>
        /// 资源
        /// </summary>
        public string resource { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? modifyon { get; set; } = DateTime.Now;


        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public virtual List<privilegeModel> children { get; set; }

    }
}
