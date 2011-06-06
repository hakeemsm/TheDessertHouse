using FluentNHibernate.Mapping;

namespace TheDessertHouse.Domain.Mappings
{
    public class ForumPostVoteMap:ClassMap<ForumPostVote>
    {
        public ForumPostVoteMap()
        {
            //CompositeId().ComponentCompositeIdentifier(x=>x.Id).KeyProperty(x => x.Id.AddedBy).KeyProperty(x => x.Id.ForumPostId);
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.AddedBy).Not.Nullable().CustomType("AnsiString");
            Map(x => x.AddedByIP).Not.Nullable().CustomType("AnsiString");
            Map(x => x.DateAdded).Not.Nullable();
            Map(x => x.Direction).Not.Nullable();
            References(x => x.Post, "PostId");
        }
    }
}
