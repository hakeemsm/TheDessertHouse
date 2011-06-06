using FluentNHibernate.Mapping;

namespace TheDessertHouse.Domain.Mappings
{
    public class ForumMap:ClassMap<Forum>
    {
        public ForumMap()
        {
            Table("Forums");
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.DateAdded).Not.Nullable();
            Map(x => x.AddedBy).CustomType("AnsiString").Not.Nullable();
            Map(x => x.Description).CustomType("AnsiString").Not.Nullable();
            Map(x => x.Importance).Not.Nullable();
            Map(x => x.Moderated).Not.Nullable();
            Map(x => x.Path).CustomType("AnsiString").Not.Nullable();
            Map(x => x.Title).CustomType("AnsiString").Not.Nullable();
            HasMany(x => x.Posts).KeyColumn("ForumId").LazyLoad().Cascade.AllDeleteOrphan().Inverse();
        }
    }
}
