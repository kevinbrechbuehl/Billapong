namespace Billapong.DataAccess.Repository
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using Model;

    /// <summary>
    /// Generic interface to implement the repository pattern.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IRepository<TEntity> : IDisposable where TEntity : class, IEntity
    {
        /// <summary>
        /// Inserts the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Add(TEntity entity);

        /// <summary>
        /// Removes the specified entity by id.
        /// </summary>
        /// <param name="id">The id.</param>
        void Remove(object id);

        /// <summary>
        /// Removes the specified entity.
        /// </summary>
        /// <param name="entityToDelete">The entity to delete.</param>
        void Remove(TEntity entityToDelete);

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entityToUpdate">The entity to update.</param>
        void Update(TEntity entityToUpdate);

        /// <summary>
        /// Gets the entites from the data provider.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="orderBy">The sort order.</param>
        /// <param name="includeProperties">The include properties which should be eager loaded.</param>
        /// <returns>List of entities</returns>
        IQueryable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");

        /// <summary>
        /// Gets an entity by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>The entity with this id</returns>
        TEntity GetById(object id);

        /// <summary>
        /// Saves changes to the database.
        /// </summary>
        void Save();
    }
}
