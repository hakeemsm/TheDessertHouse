using FluentNHibernate.Mapping;

namespace TheDessertHouse.Domain.Mappings
{
    public class ProductMap:ClassMap<Product>
    {
        public ProductMap()
        {
            Table("Products");
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.AddedBy).Not.Nullable();
            Map(x => x.DateAdded).Not.Nullable();
            Map(x => x.Description).CustomType("AnsiString");
            Map(x => x.DiscountPercentage);
            Map(x => x.SKU).CustomType("AnsiString").Not.Nullable();
            Map(x => x.UnitPrice).Not.Nullable();
            Map(x => x.UnitsInStock).Not.Nullable();
            Map(x => x.SmallImageUrl).CustomType("AnsiString");
            Map(x => x.FullImageUrl).CustomType("AnsiString");
            Map(x => x.Title).CustomType("AnsiString").Not.Nullable();
            References(x => x.Category, "DepartmentId");
        }
    }
}
