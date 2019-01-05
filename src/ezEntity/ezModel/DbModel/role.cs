using System;

namespace ezModel.DbModel
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
        public string name { get; set; }

        /// <summary>
        /// 跳转首页
        /// </summary>
        public string defaulturl { get; set; }

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
