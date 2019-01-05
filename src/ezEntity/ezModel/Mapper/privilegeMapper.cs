using DapperExtensions.Mapper;
using ezModel.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ezModel.Mapper
{
    public class privilegeMapper : ClassMapper<privilegeModel>
    {
        public privilegeMapper()
        {

            Table("privilege");
            Map(x => x.id).Column("id").Key(KeyType.Identity);
            Map(x => x.pid).Column("pid");
            Map(x => x.name).Column("name");
            Map(x => x.type).Column("type");
            Map(x => x.resource).Column("resource");
            Map(x => x.remark).Column("remark");
            Map(x => x.modifyon).Column("modifyon");

            Map(x => x.children).Ignore();
            Map(x => x.pname).Ignore();
        }
    }
}
