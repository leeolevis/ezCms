using System;

namespace ezModel.DbModel
{
    public class dictionary
    {

        /// <summary>
        /// id
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 父id
        /// </summary>
        public int? pid { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string val { get; set; }

        /// <summary>
        /// 代码
        /// </summary>
        public string code { get; set; }

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
        public bool? isystem { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? modifyon { get; set; }

    }
}
