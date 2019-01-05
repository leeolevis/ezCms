using System;
using System.Collections.Generic;
using System.Text;

namespace ezModel.BaseModel
{
    /// <summary>
    /// Model验证错误信息
    /// </summary>
    public class Verify
    {
        public string prop { get; set; }

        public List<string> erros { get; set; }
    }


    /// <summary>
    /// KEY VALUE 结构的模型
    /// </summary>
    public class KeyValueModel
    {
        public virtual string name { get; set; }

        public virtual string value { get; set; }
    }

    /// <summary>
    /// 身份实体枚举
    /// </summary>
    public enum MyClaimTypes
    {
        Id,//序号

        Account,//账号

        Role,//角色

        RoleName,//角色名称

        Name,//姓名

        Avatar,//头像

        DisplayName, //显示名

        Email, //邮箱

        Phone,//手机

        Company, //单位

        CompanyName,//单位名称

        PrivilegeManage,//管理权限

        CompanyType,//单位类型

        AreaCode//所属区域

    }
}
