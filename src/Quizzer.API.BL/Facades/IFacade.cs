using Quizzer.API.DAL.Entities.Interface;
using Quizzer.Common.Models;

namespace Quizzer.API.BL.Facades; 
public interface IFacade<TEntity, TDetailModel>
    where TEntity : class, IEntity
    where TDetailModel : class, IModel {
}