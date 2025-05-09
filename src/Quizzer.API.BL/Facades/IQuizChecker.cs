namespace Quizzer.API.BL.Facades;

public interface IQuizChecker {
    public bool IsQuizEditable(Guid quizId);
}