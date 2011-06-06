using FluentNHibernate.Mapping;

namespace TheDessertHouse.Domain.Mappings
{
    public class ArticleMap:ClassMap<Article>
    {
        public ArticleMap()
        {
            Table("Articles");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity();
            DynamicUpdate();
            Map(x => x.DateAdded);
            Map(x => x.AddedBy).Not.Nullable().Length(256);
            HasMany(x => x.Comments).KeyColumn("ArticleId").Cascade.AllDeleteOrphan().Inverse();
            //Map(x => x.ArticleCategoryId).Not.Nullable();
            References(x => x.ArticleCategory,"CategoryId").Not.LazyLoad();
            Map(x => x.Title).Not.Nullable().Length(256);
            Map(x => x.Path).Not.Nullable().Length(256);
            Map(x => x.Abstract).Nullable().Length(4000);
            Map(x => x.Body).Not.Nullable();
            Map(x => x.Country).Nullable().Length(256);
            Map(x => x.State).Nullable().Length(256);
            Map(x => x.City).Nullable().Length(256);
            Map(x => x.ReleaseDate).Not.Nullable();
            Map(x => x.ExpireDate).Nullable();
            Map(x => x.Approved).Not.Nullable();
            Map(x => x.Listed).Not.Nullable();
            Map(x => x.CommentsEnabled).Not.Nullable();
            Map(x => x.OnlyForMembers).Not.Nullable();
            Map(x => x.ViewCount).Not.Nullable();
            Map(x => x.Votes).Not.Nullable();
            Map(x => x.TotalRating).Not.Nullable();
        }
    }
}
