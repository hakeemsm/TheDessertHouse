using FluentNHibernate.Mapping;

namespace TheDessertHouse.Domain.Mappings
{
    public class OrderItemMap:ClassMap<OrderItem>
    {
        public OrderItemMap()
        {
            Table("OrderItems");
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.AddedBy).Not.Nullable();
            Map(x => x.DateAdded).Not.Nullable();
            Map(x => x.ProductId).Not.Nullable();
            Map(x => x.Quantity).Not.Nullable();
            Map(x => x.SKU).CustomType("AnsiString").Not.Nullable();
            Map(x => x.Title).CustomType("AnsiString").Not.Nullable();
            Map(x => x.UnitPrice).Not.Nullable();
            References(x => x.Order, "OrderId");
        }
    }
}
