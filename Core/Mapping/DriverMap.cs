
namespace Core.Mapping
{
    using Core.DB;
    using Core.Domain;

    public class DriverMap : EntityConfigurationBase<Driver>
    {
        public DriverMap()
        {
           
            //this.Map( m => { 
            //    m.Property(x => x.DriverLicense).HasColumnName("UD");
            //    m.Property(x => x.BirthDay).HasColumnName("dtB");
            //}).ToTable("tDriver");
            this.ToTable("tDriver");
        }
    }
}
