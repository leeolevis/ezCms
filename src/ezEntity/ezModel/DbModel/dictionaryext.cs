using System;
using System.Collections.Generic;
using System.Text;

namespace ezModel.DbModel
{
    public class dictionaryext
    {

        /// <summary>
        /// id
        /// </summary>
        public virtual int id { get; set; }

        /// <summary>
        /// 字典id
        /// </summary>
        public virtual int? dictionaryid { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public virtual string name { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public virtual string val { get; set; }

    }
}
