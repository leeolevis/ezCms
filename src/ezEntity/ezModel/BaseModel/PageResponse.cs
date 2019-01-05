using System;
using System.Collections.Generic;
using System.Text;

namespace ezModel.BaseModel
{
    /// <summary>
    /// LayUi Table 分页类
    /// </summary>
    public class PageResponse
    {
        public PageResponse(long Count, dynamic Data, int Code = 0, string Msg = "")
        {
            count = Count;
            data = Data;
            code = Code;
            if (Count == 0)
                msg = "暂无数据";
            else
                msg = Msg;
        }

        /// <summary>
        /// 状态码 默认为0 非必填
        /// </summary>
        public int code { get; set; } = 0;

        /// <summary>
        /// 消息
        /// </summary>
        public string msg { get; set; } = "";

        public long count { get; set; }

        public dynamic data { get; set; }
    }

    public class AJaxResponse
    {
        public AJaxResponse(bool Result, string Message = "")
        {
            result = Result;
            message = string.IsNullOrEmpty(Message) ? (Result ? "操作成功" : "操作失败") : Message;
        }

        public bool result;

        public string message;
    }
}
