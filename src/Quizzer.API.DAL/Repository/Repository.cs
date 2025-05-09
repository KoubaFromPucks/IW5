using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Quizzer.API.DAL.Entities.Interface;
using Quizzer.API.DAL.Repository.Interfaces;
using System.Linq.Expressions;

namespace Quizzer.API.DAL.Repository; 
public class Repository<TEntity> : IRepository<TEntity>
    where TEntity : class, IEntity {

    private readonly DbContext _dbContext;

    public Repository(DbContext dbContext) {
        _dbContext = dbContext;
    }

    public void Delete(Guid entityId) => DbSet().Remove(DbSet().Single(i => i.Id == entityId));

    public bool Exists(Guid entityId) => DbSet().Any(e => e.Id == entityId);

    public IEnumerable<TEntity> GetAll(params string[] navigationPropertyPaths) {
        IQueryable<TEntity> query = IncludeNavigations(navigationPropertyPaths);

        var entites = query.ToList();

        return entites;
    }

    public IEnumerable<TEntity> GetAll() => DbSet().ToList();

    public TEntity? GetById(Guid id, params string[] navigationPropertyPaths) {
        IQueryable<TEntity> querry = IncludeNavigations(navigationPropertyPaths);

        TEntity? entity = querry.SingleOrDefault(e => e.Id == id);
        return entity;
    }

    public IEnumerable<TEntity> GetByPredicate(Expression<Func<TEntity, bool>> predicate, params string[] navigationPropertyPaths) {
        IQueryable<TEntity> query = IncludeNavigations(navigationPropertyPaths);

        return query.Where(predicate).ToList();
    }

    public Guid Insert(TEntity entity) => DbSet().Add(entity).Entity.Id;

    public Guid? Update(TEntity entity) {
        if (!Exists(entity.Id)) return null;

        DbSet().Attach(entity);
        EntityEntry<TEntity> updatedEntity = DbSet().Update(entity);

        return updatedEntity.Entity.Id;
    }

    private DbSet<TEntity> DbSet() => _dbContext.Set<TEntity>();

    private IQueryable<TEntity> IncludeNavigations(IEnumerable<string> strings) {
        IQueryable<TEntity> querry = DbSet();
        foreach (string s in strings) {
            if (string.IsNullOrEmpty(s)) continue;
            querry = querry.Include(s);
        }

        return querry;
    }
}
