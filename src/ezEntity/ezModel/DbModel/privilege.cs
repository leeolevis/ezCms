using System;

namespace ezModel.DbModel
{
    public class privilege
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
        /// 0后台功能，1界面功能，2界面功能-带快捷方式
        /// </summary>
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
        public DateTime? modifyon { get; set; }

    }
}
