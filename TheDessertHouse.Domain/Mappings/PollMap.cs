using FluentNHibernate.Mapping;

namespace TheDessertHouse.Domain.Mappings
{
    public class PollMap:ClassMap<Poll>
    {
        public PollMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.AddedBy).Length(50).Not.Nullable();
            Map(x => x.DateAdded).Not.Nullable();
            Map(x => x.DateArchived).Nullable();
            Map(x => x.IsArchived).Not.Nullable();
            Map(x => x.IsCurrent).Not.Nullable();
            Map(x => x.Path).Length(200).Not.Nullable();
            Map(x => x.PollQuestion).Length(2000).Not.Nullable();
            HasMany(x => x.Options).KeyColumn("PollId").Cascade.AllDeleteOrphan().Inverse();
        }
    }
}
