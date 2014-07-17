
namespace Core.Mapping
{
    using Core.DB;
    using Core.Domain;

    public class OrderDriversMap : EntityConfigurationBase<OrderDrivers>
    {
        public OrderDriversMap()
        {
            this.ToTable("TYandexDriversQuery");
        }
    }
}
