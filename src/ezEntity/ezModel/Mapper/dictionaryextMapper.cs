using DapperExtensions.Mapper;
using ezModel.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ezModel.Mapper
{
    public class dictionaryextMapper : ClassMapper<dictionaryextModel>
    {
        public dictionaryextMapper()
        {

            Table("dictionaryext");
            Map(x => x.id).Column("id").Key(KeyType.Identity);
            Map(x => x.dictionaryid).Column("dictionaryid");
            Map(x => x.name).Column("name");
            Map(x => x.val).Column("val");

        }
    }
}
