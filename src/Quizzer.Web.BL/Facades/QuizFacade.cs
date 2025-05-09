using Quizzer.Common.Models;

namespace Quizzer.Web.BL.Facades; 
public class QuizFacade : IAppFacade {
    private readonly IQuizApiClient _quizApiClient;
    public QuizFacade(IQuizApiClient quizApiClient) {
        _quizApiClient = quizApiClient;
    }

    public async Task<ICollection<QuizDetailModel>> GetAllAsync() {
        return await _quizApiClient.QuizGetAsync();
    }

    public async Task<ICollection<QuizDetailModel>> GetAllForUserAsync(Guid userId) {
        return await _quizApiClient.AllForUserAsync(userId);
    }

    public async Task<Guid> UpdateAsync(QuizDetailModel questionDetailModel) {
        return await _quizApiClient.QuizPutAsync(questionDetailModel);
    }

    public async Task<Guid> CreateAsync(QuizDetailModel questionDetailModel) {
        return await _quizApiClient.QuizPostAsync(questionDetailModel);
    }
    
    public async Task<QuizDetailModel> GetByIdAsync(Guid quizId) {
        return await _quizApiClient.QuizGetAsync(quizId);
    }
    
    public async Task<Guid> DeleteAsync(Guid quizId) {
        return await _quizApiClient.QuizDeleteAsync(quizId);
    }

    public async Task<ICollection<QuizListModel>> GetQuizByContentAsync(string name, string description, bool exact) {
        return await _quizApiClient.ByContentAsync(name, description, exact);
    }

    public async Task<double> GetQuizScoreAsync(Guid quizId, Guid userId) {
        return await _quizApiClient.ScoreAsync(quizId, userId);
    }

    public async Task<Guid> StartQuizAsync(Guid quizId, Guid userId) {
        return await _quizApiClient.StartAsync(quizId, userId);
    }

    public async Task<Guid> CompleteQuizAsync(Guid quizId, Guid userId) {
        return await _quizApiClient.CompleteAsync(quizId, userId);
    }
    
    public async Task<QuizResultDetailModel> GetResultAsync(Guid quizId, Guid userId) {
        return await _quizApiClient.ResultAsync(quizId, userId);
    }
}
