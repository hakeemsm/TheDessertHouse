using FluentNHibernate.Mapping;

namespace TheDessertHouse.Domain.Mappings
{
    public class ShippingMethodMap:ClassMap<ShippingMethod>
    {
        public ShippingMethodMap()
        {
            Table("ShippingMethods");
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.AddedBy).CustomType("AnsiString").Not.Nullable();
            Map(x => x.DateAdded).Not.Nullable();
            Map(x => x.Title).CustomType("AnsiString").Not.Nullable();
            Map(x => x.Price).Not.Nullable();
        }
    }
}
