using FluentNHibernate.Mapping;

namespace TheDessertHouse.Domain.Mappings
{
    public class CommentsMap:ClassMap<Comments>
    {
        public CommentsMap()
        {
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.DateAdded).Not.Nullable();
            Map(x => x.AddedBy).Not.Nullable().Length(256);
            Map(x => x.AddedByEmail).Not.Nullable().Length(256);
            Map(x => x.AddedByIP).Not.Nullable().Length(15);
            References(x => x.ForArticle,"ArticleId");//.Not.Nullable();
            Map(x => x.Body).Not.Nullable();
        }
    }
}
