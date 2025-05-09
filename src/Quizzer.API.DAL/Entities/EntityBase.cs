using Quizzer.API.DAL.Entities.Interface;

namespace Quizzer.API.DAL.Entities; 
public abstract record EntityBase : IEntity {
    public required Guid Id { get; init; }
}
