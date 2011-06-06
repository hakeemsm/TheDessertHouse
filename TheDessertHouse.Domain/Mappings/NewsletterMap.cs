using FluentNHibernate.Mapping;

namespace TheDessertHouse.Domain.Mappings
{
    public class NewsletterMap:ClassMap<Newsletter>
    {
        public NewsletterMap()
        {
            Id(p => p.Id).GeneratedBy.Identity();
            Map(p => p.DateAdded).Not.Nullable();
            Map(p => p.DateSent);
            Map(p => p.HtmlBody).Not.Nullable();
            Map(p => p.PlainTextBody).Not.Nullable();
            Map(p => p.Subject).CustomType("AnsiString").Not.Nullable();
            Map(p => p.AddedBy).CustomType("AnsiString").Not.Nullable();
            Map(p => p.Status);
        }
    }
}
