using DapperExtensions.Mapper;
using ezModel.ViewModel;

namespace ezModel.Mapper
{
    public class dictionaryMapper : ClassMapper<dictionaryModel>
    {
        public dictionaryMapper()
        {

            Table("dictionary");
            Map(x => x.id).Column("id").Key(KeyType.Identity);
            Map(x => x.pid).Column("pid");
            Map(x => x.name).Column("name");
            Map(x => x.val).Column("val");
            Map(x => x.code).Column("code");
            Map(x => x.type).Column("type");
            Map(x => x.remark).Column("remark");
            Map(x => x.isystem).Column("isystem");
            Map(x => x.modifyon).Column("modifyon");

            Map(x => x.pname).Ignore();
            Map(x => x.exts).Ignore();
        }
    }
}
