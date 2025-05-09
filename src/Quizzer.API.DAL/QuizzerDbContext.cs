using Microsoft.EntityFrameworkCore;
using Quizzer.API.DAL.Entities;

namespace Quizzer.API.DAL;
public class QuizzerDbContext : DbContext {
    public DbSet<QuizEntity> Quiezzes => Set<QuizEntity>();
    public DbSet<CompletedQuizEntity> CompletedQuiezzes => Set<CompletedQuizEntity>();
    public DbSet<QuestionEntity> Questions => Set<QuestionEntity>();
    public DbSet<AnswerEntity> Answers => Set<AnswerEntity>();
    public DbSet<SelectedAnswerEntity> SelectedAnswers => Set<SelectedAnswerEntity>();
    public DbSet<UserEntity> Users => Set<UserEntity>();
    public DbSet<UserAnswerEntity> UserAnswers => Set<UserAnswerEntity>();

    public QuizzerDbContext(DbContextOptions<QuizzerDbContext> options)
        : base(options) {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<QuizEntity>()
            .HasMany<CompletedQuizEntity>()
            .WithOne(completedQuiz => completedQuiz.Quiz)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<QuizEntity>()
            .HasMany<QuestionEntity>()
            .WithOne(question => question.Quiz)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserEntity>()
            .HasMany<CompletedQuizEntity>()
            .WithOne(completedQuiz => completedQuiz.User)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserEntity>()
            .HasMany<UserAnswerEntity>()
            .WithOne(userAnswer => userAnswer.User)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<QuestionEntity>()
            .HasMany<AnswerEntity>()
            .WithOne(answer => answer.Question)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<QuestionEntity>()
            .HasMany<UserAnswerEntity>()
            .WithOne(userAnswer => userAnswer.Question)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<AnswerEntity>()
            .HasMany<SelectedAnswerEntity>()
            .WithOne(selectedAnswer => selectedAnswer.Answer)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<UserAnswerEntity>()
            .HasMany<SelectedAnswerEntity>()
            .WithOne(selectedAnswer => selectedAnswer.UserAnswer)
            .OnDelete(DeleteBehavior.Restrict);

        //Foreign key explicit definitions

        modelBuilder.Entity<QuestionEntity>()
            .HasOne(question => question.Quiz)
            .WithMany(quiz => quiz.Questions)
            .HasForeignKey(question => question.QuizId);

        modelBuilder.Entity<AnswerEntity>()
            .HasOne(answer => answer.Question)
            .WithMany(question => question.Answers)
            .HasForeignKey(answer => answer.QuestionId);

        modelBuilder.Entity<SelectedAnswerEntity>()
            .HasOne(selectedAnswer => selectedAnswer.Answer)
            .WithMany(answer => answer.SelectedAnswers)
            .HasForeignKey(selectedAnswer => selectedAnswer.AnswerId);

        modelBuilder.Entity<SelectedAnswerEntity>()
            .HasOne(selectedAnswer => selectedAnswer.UserAnswer)
            .WithMany(userAnswer => userAnswer.SelectedAnswers)
            .HasForeignKey(selectedAnswer => selectedAnswer.UserAnswerId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserAnswerEntity>()
            .HasOne(userAnswer => userAnswer.Question)
            .WithMany(question => question.UserAnswers)
            .HasForeignKey(userAnswer => userAnswer.QuestionId);

        modelBuilder.Entity<UserAnswerEntity>()
            .HasOne(userAnswer => userAnswer.User)
            .WithMany(user => user.UserAnswers)
            .HasForeignKey(userAnswer => userAnswer.UserId);

        modelBuilder.Entity<CompletedQuizEntity>()
            .HasOne(completedQuiz => completedQuiz.User)
            .WithMany(user => user.CompletedQuizzes)
            .HasForeignKey(completedQuiz => completedQuiz.UserId);

        modelBuilder.Entity<CompletedQuizEntity>()
            .HasOne(completedQuiz => completedQuiz.Quiz)
            .WithMany(quiz => quiz.CompletedQuizzes)
            .HasForeignKey(completedQuiz => completedQuiz.QuizId);
    }
}
