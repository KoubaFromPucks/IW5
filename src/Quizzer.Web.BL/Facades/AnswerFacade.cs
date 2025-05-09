using Quizzer.Common.Models;

namespace Quizzer.Web.BL.Facades; 
public class AnswerFacade : IAppFacade {
    private readonly IAnswerApiClient _answerApiClient;

    public AnswerFacade(IAnswerApiClient answerApiClient) {
        _answerApiClient = answerApiClient;
    }

    public async Task<ICollection<AnswerDetailModel>> GetAllAnswersAsync() {
        return await _answerApiClient.AnswerGetAsync();
    }

    public async Task<IEnumerable<AnswerDetailModel>> GetAnswerByTextAsync(string text, bool exact = false) {
        return await _answerApiClient.ByTextAsync(text, exact);
    }

    public async Task<Guid> UpdateAsync(AnswerDetailModel model, Guid questionId) {
        return await _answerApiClient.AnswerPutAsync(questionId, model);
    }

    public async Task<Guid> CreateAsync(AnswerDetailModel model, Guid questionId) {
        return await _answerApiClient.AnswerPostAsync(questionId, model);
    }

    public async Task<Guid> DeleteAsync(Guid id) {
        return await _answerApiClient.AnswerDeleteAsync(id);
    }


    public async Task<AnswerDetailModel> GetByIdAsync(Guid id) {
        return await _answerApiClient.AnswerGetAsync(id);
    }

    public async Task<IEnumerable<AnswerDetailModel>> GetAnswersForQuestionAsync(Guid questionId) {
        return await _answerApiClient.ByQuestionAsync(questionId);
    }
}
