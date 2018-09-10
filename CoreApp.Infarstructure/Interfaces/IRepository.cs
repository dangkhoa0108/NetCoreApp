using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CoreApp.Infrastructure.Interfaces
{
    public interface IRepository<T, K> where T:class
    {
        /// <summary>
        /// Get object T with Id K, after do Function with params object T and return object, with other entity
        /// </summary>
        /// <param name="id">ID has Generic object</param>
        /// <param name="includeProperties">Object return (optional)</param>
        /// <returns>Object T</returns>
        T FindById(K id, params Expression<Func<T, object>>[] includeProperties);

        /// <summary>
        /// Get object T with Id K, after do Function with params object T and return object and predicate, with other entity
        /// </summary>
        /// <param name="predicate">Condition</param>
        /// <param name="includeProperties">Object return (optional)</param>
        /// <returns></returns>
        T FindSingle(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);

        /// <summary>
        /// Get IQueryable object T after do Function with params object T and return object, with other entity
        /// </summary>
        /// <param name="includeProperties">Object return (optional)</param>
        /// <returns></returns>
        IQueryable<T> FindAll(params Expression<Func<T, object>>[] includeProperties);

        /// <summary>
        /// Get IQueryable object T after do Function with params object T and return object and predicate, with other entity
        /// </summary>
        /// <param name="predicate">Condition</param>
        /// <param name="includeProperties">Object return (optional)</param>
        /// <returns></returns>
        IQueryable<T> FindAll(Expression<Func<T, bool>> predicate ,params Expression<Func<T, object>>[] includeProperties);

        /// <summary>
        /// Add new entity T
        /// </summary>
        /// <param name="entity">Entity input</param>
        void Add(T entity);

        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity">Entity input</param>
        void Update(T entity);

        /// <summary>
        /// Remove entity with entity
        /// </summary>
        /// <param name="entity">Entity input</param>
        void Remove(T entity);

        /// <summary>
        /// Remove Entity with id
        /// </summary>
        /// <param name="id">ID</param>
        void Remove(K id);

        /// <summary>
        /// Remove multiple entity
        /// </summary>
        /// <param name="entities">List Entity</param>
        void RemoveMultiple(List<T> entities);
    }
}
