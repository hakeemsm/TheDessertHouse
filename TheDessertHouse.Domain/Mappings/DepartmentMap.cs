using FluentNHibernate.Mapping;

namespace TheDessertHouse.Domain.Mappings
{
    public class DepartmentMap:ClassMap<Department>
    {
        public DepartmentMap()
        {
            Table("Departments");
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.AddedBy).CustomType("AnsiString").Not.Nullable();
            Map(x => x.DateAdded).Not.Nullable();
            Map(x => x.Description).CustomType("AnsiString");
            Map(x => x.ImageUrl).CustomType("AnsiString");
            Map(x => x.Importance).Not.Nullable();
            Map(x => x.Title).CustomType("AnsiString").Not.Nullable();
            HasMany(x => x.Products).KeyColumn("DepartmentId").Cascade.AllDeleteOrphan().Inverse();
        }
    }
}
