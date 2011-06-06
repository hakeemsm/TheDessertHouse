using FluentNHibernate.Mapping;

namespace TheDessertHouse.Domain.Mappings
{
    public class ForumPostMap:ClassMap<ForumPost>
    {
        public ForumPostMap()
        {
            Table("ForumPosts");
            Id(p => p.Id).GeneratedBy.Identity();
            Map(x => x.DateAdded).Not.Nullable();
            Map(p => p.AddedBy).CustomType("AnsiString").Not.Nullable();
            Map(p => p.AddedByIP).CustomType("AnsiString").Not.Nullable();
            Map(p => p.Title).CustomType("AnsiString").Not.Nullable();
            Map(p => p.Path).CustomType("AnsiString").Not.Nullable();
            Map(p => p.Body).CustomType("AnsiString").Not.Nullable();
            Map(p => p.Approved).Not.Nullable();
            Map(p => p.Closed).Not.Nullable();
            Map(p => p.VoteCount);
            Map(p => p.ViewCount);
            Map(p => p.ReplyCount);
            Map(p => p.LastPostBy).CustomType("AnsiString");
            Map(p => p.LastPostDate);
            Map(p => p.ParentPostId).Nullable();
            HasMany(p => p.Votes).KeyColumn("PostId").Cascade.AllDeleteOrphan().Inverse().Not.LazyLoad();
            HasMany(p => p.Replies).KeyColumn("ParentPostId").LazyLoad().Cascade.AllDeleteOrphan().Inverse();
            References(p => p.Forum,"ForumId").LazyLoad(Laziness.Proxy);
        }
    }
}
