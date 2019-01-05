using System;
using System.Collections.Generic;

namespace DapperExtensions
{
    public class Page<T> 
    {

        /// <summary>
        /// ����Ŀ��
        /// </summary>
        public long TotalItems { get; set; }

        /// <summary>
        /// ÿҳ��Ŀ��
        /// </summary>
        public long ItemsPerPage { get; set; }

        /// <summary>
        /// ��ǰҳ��
        /// </summary>
        public long CurrentPage { get; set; }

        public IEnumerable<T> Items { get; set; }

        /// <summary>
        /// ��ҳ��
        /// </summary>
        public long TotalPages => (long)Math.Ceiling((decimal)TotalItems / ItemsPerPage);
    }
}