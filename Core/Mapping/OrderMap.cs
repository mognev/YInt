
namespace Core.Mapping
{
    using Core.DB;
    using Core.Domain;

    public class OrderMap : EntityConfigurationBase<Order>
    {
        public OrderMap()
        {
            this.ToTable("TYandexOrder");
            this.Property(t => t.Lon).HasPrecision(18, 11);
            this.Property(t => t.Lat).HasPrecision(18, 11);
            this.Property(t => t.D_Lon).HasPrecision(18, 11);
            this.Property(t => t.D_Lat).HasPrecision(18, 11);
        }
    }
}
