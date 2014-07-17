


namespace Core.Mapping
{
    using Core.DB;
    using Core.Domain;

    public class BlackPhoneMap : EntityConfigurationBase<BlackPhone>
    {
        public BlackPhoneMap()
        {
            this.ToTable("TBlackList");
        }
    }
}
