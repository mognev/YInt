
namespace Core.Mapping
{
    using Core.DB;
    using Core.Domain;

    public class DriverMap : EntityConfigurationBase<Driver>
    {
        public DriverMap()
        {
            this.ToTable("tDriver");
        }
    }
}
