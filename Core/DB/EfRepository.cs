
namespace Core.Instructure
{
    #region Using

    using System;
    using System.Linq;
    using System.Data.Entity;
    using Core.Domain;
    using Core.Db.Interfaces;

    #endregion

    /// <summary>
    /// Entity framework repository.
    /// </summary>
    /// <typeparam name="T">Base entity.</typeparam>
    public class EfRepository<T> : IRepository<T> where T : EntityBase
    {
        protected readonly IDbContext _context;
        protected readonly IDbSet<T> _entities;

        public EfRepository(IDbContext context)
        {
            this._context = context;
            this._entities = context.Set<T>();
        }

        public virtual T GetById(Object id)
        {
            T entity = this._entities.Find(id);

            return (entity != null) ? entity : null;
        }

        public void Insert(T entity)
        {
            this._entities.Add(entity);
            var errors = ((DbContext)this._context).GetValidationErrors();
            this._context.SaveChanges();
        }

        public void Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            try
            {
                this._context.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        System.Diagnostics.Trace.TraceInformation(
                            "Property: {0} Error: {1}",
                            validationError.PropertyName,
                            validationError.ErrorMessage);
                    }
                }
            }
        }

        public virtual void Delete(T entity)
        {
            //entity.Deleted = true;

            this._context.SaveChanges();
        }

        public virtual IQueryable<T> Table
        {
            get { return this._entities; }
        }

        public void Dispose()
        {
            if (this._context != null)
            {
                ((DbContext)_context).Dispose();
            }
        }
    }
}
