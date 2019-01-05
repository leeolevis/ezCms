using ezModel.BaseModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
namespace ezModel.ViewModel
{
    public class role
    {

        /// <summary>
        /// id
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Required(ErrorMessage = "名称不能为空")]
        [StringLength(32, MinimumLength = 1, ErrorMessage = "名称必须在{2}~{1}个字符")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Remote(areaName: "", action: "ExistRoleName", controller: "Verify", AdditionalFields = "name,currentname", ErrorMessage = "角色已存在")]
        public string name { get; set; }

        public string currentname => name;

        /// <summary>
        /// 跳转首页
        /// </summary>
        public string defaulturl { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string remake { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? modifyon { get; set; }

        public virtual JsTree privileges { get; set; }

    }
}
