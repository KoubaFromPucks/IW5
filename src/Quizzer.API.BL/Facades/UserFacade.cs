using AutoMapper;
using Quizzer.API.DAL.Entities;
using Quizzer.API.DAL.Repository.Interfaces;
using Quizzer.API.DAL.UnitOfWork;
using Quizzer.Common.Models;
using Quizzer.Extensions;
using System.Linq.Expressions;

namespace Quizzer.API.BL.Facades; 
public class UserFacade : FacadeBase<UserEntity, UserDetailModel> {
    public UserFacade(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper) : base(unitOfWorkFactory, mapper) {
    }

    public new Guid Save(UserDetailModel model) {
        model.Name = model.Name?.TrimWhiteAndUnprintableChars();

        if (string.IsNullOrEmpty(model.Name)) {
            throw new ArgumentException("Name cannot be empty");
        }

        return base.Save(model);
    }

    public ICollection<UserListModel> GetByName(string name, bool exactMatch) {
        var trimName = name.TrimWhiteAndUnprintableChars();

        if (exactMatch) return GetListByPredicate(e => e.Name == trimName);

        string[] splitWords = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        var resultModels = new List<UserListModel>();
        bool firstIteration = true;
        foreach (string word in splitWords) {
            var lowerWord = word.ToLower();
            ICollection<UserListModel> currentModels = GetListByPredicate<UserListModel>(entity =>
            entity.Name.ToLower().Contains(word.ToLower()));

            if (firstIteration) {
                firstIteration = false;
                resultModels = currentModels.ToList();
            } else {
                resultModels = resultModels.IntersectBy(currentModels.Select(m => m.Id), model => model.Id).ToList();
            }
        }

        return resultModels;
    }

    public new void Delete(Guid id) {
        base.Delete(id);
    }

    private ICollection<UserListModel> GetListByPredicate(Expression<Func<UserEntity, bool>> predicate) {
        using IUnitOfWork uow = p_unitOfWorkFactory.Create();

        IRepository<UserEntity> repository = uow.GetRepository<UserEntity>();

        IEnumerable<UserEntity> entities = repository.GetByPredicate(predicate);

        ICollection<UserListModel> models = entities.Select(p_mapper.Map<UserListModel>).ToList();

        return models;
    }


}
