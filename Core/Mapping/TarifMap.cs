


namespace Core.Mapping
{
    using Core.DB;
    using Core.Domain;

    public class TarifMap : EntityConfigurationBase<Tarif>
    {
        public TarifMap()
        {
            this.ToTable("Ttarif2");
        }
    }
}
