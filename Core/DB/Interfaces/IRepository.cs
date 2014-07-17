
namespace Core.Db.Interfaces
{
    using System;
    using System.Linq;

    using Core.Domain;

    /// <summary>
    /// Repository.
    /// </summary>
    /// <typeparam name="T">Type of entity.</typeparam>
    public interface IRepository<T> : IDisposable where T : EntityBase
    {
        /// <summary>
        /// Gets the table.
        /// </summary>
        IQueryable<T> Table { get; }

        /// <summary>
        /// Gets the by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>Entity with current id.</returns>
        //T GetById(Object id);

        /// <summary>
        /// Inserts the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Insert(T entity);

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Update(T entity);

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Delete(T entity);
    }
}
