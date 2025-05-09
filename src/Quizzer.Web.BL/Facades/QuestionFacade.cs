using Quizzer.Common.Models;

namespace Quizzer.Web.BL.Facades; 
public class QuestionFacade : IAppFacade {
    private readonly IQuestionApiClient _questionApiClient;

    public QuestionFacade(IQuestionApiClient questionApiClient) {
        _questionApiClient = questionApiClient;
    }

    public async Task<ICollection<QuestionDetailModel>> GetAllAsync() {
        return await _questionApiClient.QuestionGetAsync();
    }

    public async Task<QuestionDetailModel> GetByIdAsync(Guid quiestionId) {
        return await _questionApiClient.QuestionGetAsync(quiestionId);
    }

    public async Task<Guid> UpdateAsync(Guid quizId, QuestionDetailModel questionDetailModel) {
        return await _questionApiClient.QuestionPutAsync(quizId, questionDetailModel);
    }

    public async Task<Guid> CreateAsync(Guid quizId, QuestionDetailModel questionDetailModel) {
        return await _questionApiClient.QuestionPostAsync(quizId, questionDetailModel);
    }

    public async Task<ICollection<QuestionDetailModel>> GetQuestionByNameAsync(string name, bool exact) {
        return await _questionApiClient.ByNameAsync(name, exact);
    }

    public async Task<ICollection<QuestionResultModel>> GetResultsForUserAsync(Guid quizId, Guid userId) {
        return await _questionApiClient.ResultsForUserAsync(quizId, userId);
    }

    public async Task<Guid> AnswerQuestionAsync(Guid answerId, Guid userId, int order) { 
        return await _questionApiClient.AnswerQuestionAsync(answerId, userId, order);
    }

    public async Task<Guid> DeleteAsync(Guid questionId) {
        return await _questionApiClient.QuestionDeleteAsync(questionId);
    }
}
