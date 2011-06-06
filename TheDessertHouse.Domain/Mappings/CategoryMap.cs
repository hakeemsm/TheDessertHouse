using FluentNHibernate.Mapping;

namespace TheDessertHouse.Domain.Mappings
{
    public class CategoryMap:ClassMap<Category>
    {
        public CategoryMap()
        {
            Table("Categories");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity();
            HasMany(x => x.Articles).KeyColumn("CategoryId").Cascade.AllDeleteOrphan().Inverse();
            Map(x => x.DateAdded).Not.Nullable();
            Map(x => x.AddedBy).Not.Nullable().Length(256);
            Map(x => x.Title).Not.Nullable().Length(256);
            Map(x => x.Path).Not.Nullable().Length(256);
            Map(x => x.Importance).Not.Nullable();
            Map(x => x.Description).Length(4000);
            Map(x => x.ImageUrl).Length(256);
        }
    }
}
