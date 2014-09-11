

using System.Collections.Generic;

namespace Core.DB
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.SqlClient;
    using System.Linq;
    using Core.Db;
    using Core.Db.Interfaces;
    using Core.Domain;
    using Core.Mapping;

    public class NavgatorTaxiObjectContext : DbContext, IDbContext
    {
        public new IDbSet<TEntity> Set<TEntity>() where TEntity : EntityBase
        {
            return base.Set<TEntity>();
        }

        public DbEntityEntry GetDbEntityEntry<TEntity>(TEntity entity) where TEntity : EntityBase
        {
            return this.Entry<TEntity>(entity);
        }

        public void ExecuteStoredProcedure(String storedProcedureName, params StoredProcedureParameter[] parameters)
        {
            String sql = String.Join(
                " ",
                storedProcedureName,
                String.Join(", ", parameters.Select(s => String.Concat("@", s.ParameterName))));

            SqlParameter[] sqlParameters = parameters
                .Select(s => new SqlParameter(s.ParameterName, s.Value))
                .ToArray();

            this.Database.ExecuteSqlCommand(sql, sqlParameters);
        }

        public IEnumerable<TEntity> ExecuteStoredProcedure<TEntity>(String storedProcedureName, params StoredProcedureParameter[] parameters)
            where TEntity : EntityBase
        {
            //TODO add SQL parametrs
            return this.Database.SqlQuery<TEntity>("exec " + storedProcedureName);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new DriverMap());
            modelBuilder.Configurations.Add(new TarifMap());
            modelBuilder.Configurations.Add(new BlackPhoneMap());
            modelBuilder.Configurations.Add(new OrderMap());
            modelBuilder.Configurations.Add(new OrderDriversMap());
        }
    }
}
