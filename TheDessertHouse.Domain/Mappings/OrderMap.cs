using FluentNHibernate.Mapping;

namespace TheDessertHouse.Domain.Mappings
{
    public class OrderMap:ClassMap<Order>
    {
        public OrderMap()
        {
            Table("Orders");
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.AddedBy).CustomType("AnsiString").Not.Nullable();
            Map(x => x.CustomerEmail).CustomType("AnsiString").Not.Nullable();
            Map(x => x.DateAdded).Not.Nullable();
            Map(x => x.ShippedDate);
            Map(x => x.Shipping).Not.Nullable();
            Map(x => x.ShippingCity).CustomType("AnsiString").Not.Nullable();
            Map(x => x.ShippingFirstName).CustomType("AnsiString").Not.Nullable();
            Map(x => x.ShippingLastName).Not.Nullable().CustomType("AnsiString");
            Map(x => x.ShippingMethod).CustomType("AnsiString").Not.Nullable();
            Map(x => x.ShippingState).Not.Nullable();
            Map(x => x.ShippingStreet).CustomType("AnsiString").Not.Nullable();
            Map(x => x.ShippingZipCode).CustomType("AnsiString").Not.Nullable();
            Map(x => x.Status).CustomType("AnsiString").Not.Nullable();
            Map(x => x.SubTotal).Not.Nullable();
            Map(x => x.TrackingId).CustomType("AnsiString");
            Map(x => x.TransactionId).CustomType("AnsiString");
            HasMany(x => x.Items).KeyColumn("OrderId").Cascade.AllDeleteOrphan().Inverse();
        }
    }
}
