using DapperExtensions.Mapper;
using ezModel.ViewModel;

namespace ezModel.Mapper
{
    public class roleMapper : ClassMapper<roleModel>
    {
        public roleMapper()
        {

            Table("role");
            Map(x => x.id).Column("id").Key(KeyType.Identity);
            Map(x => x.name).Column("name");
            Map(x => x.remark).Column("remark");
            Map(x => x.defaulturl).Column("defaulturl");
            Map(x => x.modifyon).Column("modifyon");

            Map(x => x.Permissions).Ignore();
        }
    }
}
