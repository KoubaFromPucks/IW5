using Quizzer.API.DAL;
using Quizzer.API.DAL.Seeds;

namespace Quizzer.Seeder;
public class QuizzerSeeder {
    QuizzerDbContext _dbContext;
    public QuizzerSeeder(QuizzerDbContext dbContext) {
        _dbContext = dbContext;
    }

    public void ClearDatabase() {
        _dbContext.Answers.RemoveRange(_dbContext.Answers);
        _dbContext.CompletedQuiezzes.RemoveRange(_dbContext.CompletedQuiezzes);
        _dbContext.Questions.RemoveRange(_dbContext.Questions);
        _dbContext.Quiezzes.RemoveRange(_dbContext.Quiezzes);
        _dbContext.SelectedAnswers.RemoveRange(_dbContext.SelectedAnswers);
        _dbContext.UserAnswers.RemoveRange(_dbContext.UserAnswers);
        _dbContext.Users.RemoveRange(_dbContext.Users);

        _dbContext.SaveChanges();
    }

    public void SeedDatabase(Storage storage) {
        _dbContext.Answers.AddRange(storage.Answers);
        _dbContext.CompletedQuiezzes.AddRange(storage.CompletedQuizzes);
        _dbContext.Questions.AddRange(storage.Questions);
        _dbContext.Quiezzes.AddRange(storage.Quizzes);
        _dbContext.SelectedAnswers.AddRange(storage.SelectedAnswers);
        _dbContext.UserAnswers.AddRange(storage.UserAnswers);
        _dbContext.Users.AddRange(storage.Users);

        _dbContext.SaveChanges();
    }
}
