using Microsoft.EntityFrameworkCore;
using Quizzer.API.DAL.Seeds;
using Quizzer.API.DAL.Entities;

namespace Quizzer.API.DAL.Tests; 
public class QuizzerTestingDbContext : QuizzerDbContext{

    private readonly Storage? _storage;

    public QuizzerTestingDbContext(DbContextOptions<QuizzerDbContext> contextOptions, Storage? storage)
    : base(contextOptions) { _storage = storage; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);

        if (_storage is not null) {
            modelBuilder.Entity<AnswerEntity>().HasData(_storage.Answers);
            modelBuilder.Entity<CompletedQuizEntity>().HasData(_storage.CompletedQuizzes);
            modelBuilder.Entity<QuestionEntity>().HasData(_storage.Questions);
            modelBuilder.Entity<QuizEntity>().HasData(_storage.Quizzes);
            modelBuilder.Entity<SelectedAnswerEntity>().HasData(_storage.SelectedAnswers);
            modelBuilder.Entity<UserAnswerEntity>().HasData(_storage.UserAnswers);
            modelBuilder.Entity<UserEntity>().HasData(_storage.Users);
        }
    }
}
