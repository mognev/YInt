
namespace Core.DB
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using Core.Domain;

    public abstract class EntityConfigurationBase<TEntity> : EntityTypeConfiguration<TEntity>
        where TEntity : EntityBase
    {
        public EntityConfigurationBase()
        {
            //HasKey(k => k.Id);
            //Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }
}
