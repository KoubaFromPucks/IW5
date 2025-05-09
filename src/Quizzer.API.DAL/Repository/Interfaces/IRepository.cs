using Quizzer.API.DAL.Entities.Interface;
using System.Linq.Expressions;

namespace Quizzer.API.DAL.Repository.Interfaces {
    public interface IRepository<TEntity>
        where TEntity : class, IEntity {
        /// <param name="navigationPropertyPath">A string of '.' separated navigation property names to be included.</param>
        /// <returns>Found entity by it's ID, or null when there is no entity with specified ID</returns>
        TEntity? GetById(Guid id, params string[] navigationPropertyPath);

        /// <param name="navigationPropertyPath">A string of '.' separated navigation property names to be included.</param>
        IEnumerable<TEntity> GetByPredicate(Expression<Func<TEntity, bool>> predicate, params string[] navigationPropertyPath);

        /// <param name="navigationPropertyPaths">A collection of strings, which are in form of '.' separated navigation property names to be included.</param>
        IEnumerable<TEntity> GetAll(params string[] navigationPropertyPaths);

        void Delete(Guid entityId);

        bool Exists(Guid entityId);

        /// <returns>New entity reference with inserted entity data</returns>
        Guid Insert(TEntity entity);

        /// <summary>
        /// Updates data on existing entity in database
        /// </summary>
        /// <param name="entity">Existing entity</param>
        /// <returns>Updated entity ID</returns>
        Guid? Update(TEntity entity);
    }
}
