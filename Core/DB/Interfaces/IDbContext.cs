
using System.Collections.Generic;

namespace Core.Db.Interfaces
{
    using System;
    using Core.Domain;

    public interface IDbContext
    {
        System.Data.Entity.IDbSet<TEntity> Set<TEntity>() where TEntity : EntityBase;
        Int32 SaveChanges();
        System.Data.Entity.Infrastructure.DbEntityEntry GetDbEntityEntry<TEntity>(TEntity entity)
            where TEntity : EntityBase;
        void ExecuteStoredProcedure(String storedProcedureName, params StoredProcedureParameter[] parameters);
        IEnumerable<TEntity> ExecuteStoredProcedure<TEntity>(String storedProcedureName, 
                                                      params StoredProcedureParameter[] parameters) where TEntity : EntityBase;
    }
}
