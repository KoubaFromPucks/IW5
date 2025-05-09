using AutoMapper;
using Quizzer.API.DAL.Entities.Interface;
using Quizzer.API.DAL.Repository.Interfaces;
using Quizzer.API.DAL.UnitOfWork;
using Quizzer.Common.Models;
using System.Linq.Expressions;

namespace Quizzer.API.BL.Facades;
public abstract class FacadeBase<TEntity, TDetailModel> : IFacade<TEntity, TDetailModel>
    where TEntity : class, IEntity
    where TDetailModel : class, IModel {

    protected readonly IUnitOfWorkFactory p_unitOfWorkFactory;
    protected readonly IMapper p_mapper;

    protected FacadeBase(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper) {
        p_unitOfWorkFactory = unitOfWorkFactory;
        p_mapper = mapper;
    }

    protected virtual string[] IncludesNavigationPathDetails => Array.Empty<string>();

    public virtual bool Exists(Guid id) {
        using IUnitOfWork uow = p_unitOfWorkFactory.Create();

        IRepository<TEntity> repo = uow.GetRepository<TEntity>();

        return repo.Exists(id);
    }

    public virtual TDetailModel? GetDetailById(Guid id) {
        using IUnitOfWork uow = p_unitOfWorkFactory.Create();

        IRepository<TEntity> repo = uow.GetRepository<TEntity>();

        TEntity? entity = repo.GetById(id, IncludesNavigationPathDetails);

        if (entity is null) return null;

        return p_mapper.Map<TDetailModel>(entity);
    }

    protected ICollection<TModel> GetListByPredicate<TModel>(Expression<Func<TEntity, bool>> predicate, params string[] navigationPaths)
        where TModel : IModel {

        using IUnitOfWork uow = p_unitOfWorkFactory.Create();

        IRepository<TEntity> repository = uow.GetRepository<TEntity>();

        IEnumerable<TEntity> entities = repository.GetByPredicate(predicate, navigationPaths);

        ICollection<TModel> models = entities.Select(p_mapper.Map<TModel>).ToList();
        return models;
    }

    protected virtual Guid Save(TDetailModel model) {
        TEntity entity = p_mapper.Map<TEntity>(model);
        return Save(entity);
    }

    protected virtual Guid Save(TEntity entity) {
        using IUnitOfWork uow = p_unitOfWorkFactory.Create();

        IRepository<TEntity> repository = uow.GetRepository<TEntity>();

        Guid id;

        if (repository.Exists(entity.Id)) {
            id = repository.Update(entity) ?? Guid.Empty; // should always return a valid guid
        } else {
            id = repository.Insert(entity);
        }

        uow.Commit();

        return id;
    }

    protected virtual void Delete(Guid id) {
        using IUnitOfWork uow = p_unitOfWorkFactory.Create();
        IRepository<TEntity> repository = uow.GetRepository<TEntity>();

        repository.Delete(id);

        uow.Commit();
    }

    public virtual ICollection<TDetailModel> GetAll() {
        IUnitOfWork uow = p_unitOfWorkFactory.Create();

        IRepository<TEntity> repository = uow.GetRepository<TEntity>();

        IEnumerable<TEntity> entities = repository.GetAll();

        var models = entities.Select(p_mapper.Map<TDetailModel>).ToList();
        return models;
    }
}