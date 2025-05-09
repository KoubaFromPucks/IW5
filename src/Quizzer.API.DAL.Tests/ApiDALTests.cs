using ActiveTracker.DAL.Tests;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Quizzer.API.DAL.Entities;
using Quizzer.API.DAL.Repository.Interfaces;
using Quizzer.API.DAL.Seeds;
using Quizzer.API.DAL.UnitOfWork;

namespace Quizzer.API.DAL.Tests; 

public class ApiDALTests : IAsyncLifetime {

    protected UnitOfWorkFactory UnitOfWorkFactory;
    protected IUnitOfWork UnitOfWork;

    protected IDbContextFactory<QuizzerDbContext> DbContextFactory { get; }
    protected QuizzerDbContext DbContextSUT { get; }

    protected Storage storage;

    public ApiDALTests() {
        DbContextFactory = new QuizzerDbContextTestingFactory(GetType().FullName!, seedTestingData: true);
        DbContextSUT = DbContextFactory.CreateDbContext();
        UnitOfWorkFactory = new UnitOfWorkFactory(DbContextFactory);
        UnitOfWork = UnitOfWorkFactory.Create();//initialize UnitOfWork so it doesn't have to be nullable
        storage = new Storage(true);
    }

    public async Task DisposeAsync() {
        await DbContextSUT.Database.EnsureDeletedAsync();
        await DbContextSUT.DisposeAsync();
        UnitOfWork.Dispose();
    }

    public async Task InitializeAsync() {
        await DbContextSUT.Database.EnsureDeletedAsync();
        await DbContextSUT.Database.EnsureCreatedAsync();
        UnitOfWork = UnitOfWorkFactory.Create();
    }

    [Fact]
    public async Task Update_QuizEntity_Updated() {
        //Arrange
        IRepository<QuizEntity> repo = UnitOfWork.GetRepository<QuizEntity>();
        QuizEntity baseEntity = storage.Quizzes[0];
        QuizEntity updatedEntity = baseEntity with { Name = baseEntity.Name + "Updated" };

        //Act
        Guid? repoEntityId = repo.Update(updatedEntity);
        await UnitOfWork.CommitAsync();

        //Assert
        Assert.NotNull(repoEntityId);
        Assert.Equal(repoEntityId, updatedEntity.Id);
        await using QuizzerDbContext dbx = await DbContextFactory.CreateDbContextAsync();
        QuizEntity dbxEntity = await dbx.Quiezzes.SingleAsync(i => i.Id == repoEntityId);
        DeepAssert.Equal(updatedEntity, dbxEntity);
    }

    [Fact]
    public async Task GetById_Quiz_Included() {
        //Arrange
        IRepository<QuizEntity> repo = UnitOfWork.GetRepository<QuizEntity>();
        QuizEntity baseEntity = storage.Quizzes[0];
        
        //Act
        QuizEntity? retrunEntity = repo.GetById(baseEntity.Id, new string[] {
            nameof(QuizEntity.Questions)
        });

        //Assert
        //Assert.NotEmpty(baseEntity.Questions);//check if test makes sense
        Assert.NotNull(retrunEntity);
        Assert.NotEmpty(retrunEntity.Questions);
    }

    [Fact]
    public async Task Delete_Question_Deleted() {
        //Arrange
        IRepository<QuestionEntity> repo = UnitOfWork.GetRepository<QuestionEntity>();
        QuestionEntity baseEntity = storage.Questions[0];

        //Act
        repo.Delete(baseEntity.Id);

        //Assert
        await using QuizzerDbContext dbx = await DbContextFactory.CreateDbContextAsync();
        QuizEntity? dbxEntity = await dbx.Quiezzes.SingleOrDefaultAsync(i => i.Id == baseEntity.Id);
        Assert.Null(dbxEntity);
    }
}