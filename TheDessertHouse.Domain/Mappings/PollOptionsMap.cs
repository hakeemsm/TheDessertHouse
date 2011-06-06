using FluentNHibernate.Mapping;

namespace TheDessertHouse.Domain.Mappings
{
    public class PollOptionsMap:ClassMap<PollOptions>
    {
        public PollOptionsMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.DateAdded).Not.Nullable();
            Map(x => x.OptionText).Length(200).Not.Nullable();
            Map(x => x.Votes).Not.Nullable();
            References(x => x.ForPoll, "PollId").Not.LazyLoad();
        }
    }
}
