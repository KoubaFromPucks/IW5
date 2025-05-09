using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Quizzer.API.DAL;
using Quizzer.API.DAL.Entities;
using Quizzer.API.DAL.Seeds;
using Quizzer.Common.Models;
using Quizzer.Seeder;
using System.Net;
using System.Text;

namespace Quizzer.API.IntegrationTests;

public class EndToEndTests
        : IClassFixture<WebApplicationFactory<Program>> {
    private readonly WebApplicationFactory<Program> _factory;
    private Storage _storage;
    QuizzerSeeder _seeder;
    QuizzerDbContext _dbContext;

    public EndToEndTests() {
        _factory = new CustomWebApplicationFactory<Program>();
        _storage = new Storage(true);

        IConfigurationRoot configuration =
            new ConfigurationBuilder().AddUserSecrets<QuizzerDbContext>(optional: true).Build();
        var optionsBuilder = new DbContextOptionsBuilder<QuizzerDbContext>();
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("TestingDatabase"));

        _dbContext = new QuizzerDbContext(optionsBuilder.Options);
        _seeder = new QuizzerSeeder(_dbContext);
    }

    private void PrepareDatabase() {
        _dbContext.Database.EnsureCreated();
        _seeder.ClearDatabase();
        _seeder.SeedDatabase(_storage);
    }

    private HttpClient GetClient() {
        PrepareDatabase();
        return _factory.CreateClient();
    }

    [Theory]
    [InlineData("/api/Answer/1234")]
    [InlineData("/api/User/1234")]
    [InlineData("/api/Quiz/1234")]
    [InlineData("/api/Question/1234")]
    public async Task GetById_InvalidId_StatusNotFound(string url) {
        //Arange
        HttpClient client = GetClient();

        // Act
        HttpResponseMessage response = await client.GetAsync(url);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }


    [Theory]
    [InlineData("/api/Answer", typeof(ICollection<AnswerDetailModel>), typeof(AnswerDetailModel))]
    [InlineData("/api/Question", typeof(ICollection<QuestionDetailModel>), typeof(QuestionDetailModel))]
    [InlineData("/api/User", typeof(ICollection<UserDetailModel>), typeof(UserDetailModel))]
    [InlineData("/api/Quiz", typeof(ICollection<QuizDetailModel>), typeof(QuizDetailModel))]
    public async Task GetAll_ReturnsNotEmptyIEnumerableOfModels(string url, Type responseType, Type modelType) {
        // Arrange
        HttpClient client = GetClient();

        // Act
        HttpResponseMessage response = await client.GetAsync(url);

        // Assert
        response.EnsureSuccessStatusCode();

        var resObject = await response.Content.ReadFromJsonAsync(responseType);
        Assert.NotNull(resObject);//got response

        var collection = resObject as IEnumerable<object>;
        Assert.NotNull(collection);//cast is succesfull
        Assert.NotEmpty(collection);

        Assert.Equal(modelType, collection.First().GetType());//model is correct type
    }

    [Fact]
    public async Task GetById_Answer_ReturnsCorrect() {
        // Arrange
        HttpClient client = GetClient();
        AnswerEntity baseEntity = _storage.Answers[0];

        // Act
        HttpResponseMessage response = await client.GetAsync($"/api/Answer/{baseEntity.Id}");

        // Assert
        response.EnsureSuccessStatusCode();

        AnswerDetailModel? resObject = await response.Content.ReadFromJsonAsync<AnswerDetailModel>();
        Assert.NotNull(resObject);//got response

        Assert.Equal(baseEntity.Id, resObject.Id);
        Assert.Equal(baseEntity.Text, resObject.Text);
        Assert.Equal(baseEntity.PictureUrl, resObject.PictureUrl);
    }

    [Fact]
    public async Task GetByQuestion_Answers_Returns() {
        // Arrange
        HttpClient client = GetClient();
        QuestionEntity question = _storage.Questions[0];
        string url = $"/api/Answer/ByQuestion/{question.Id}";

        // Act
        HttpResponseMessage response = await client.GetAsync(url);

        // Assert
        response.EnsureSuccessStatusCode();

        ICollection<AnswerDetailModel>? resObject = await response.Content.ReadFromJsonAsync<ICollection<AnswerDetailModel>>();

        Assert.NotNull(resObject);//got response
        Assert.Equal(resObject.Count, _storage.Answers.Count(x => x.QuestionId == question.Id));
    }

    [Fact]
    public async Task GetByText_Answer_Returns() {
        // Arrange
        HttpClient client = GetClient();
        AnswerEntity baseAnswer = _storage.Answers[0];
        string url = $"/api/Answer/ByText/{baseAnswer.Text}/true";

        // Act
        HttpResponseMessage response = await client.GetAsync(url);

        // Assert
        response.EnsureSuccessStatusCode();

        ICollection<AnswerDetailModel>? resObject = await response.Content.ReadFromJsonAsync<ICollection<AnswerDetailModel>>();

        Assert.NotNull(resObject);//got response
        Assert.Equal(1, resObject.Count(m => m.Id == baseAnswer.Id));
        Assert.True(resObject.Count < _storage.Answers.Count);
    }

    [Fact]
    public async Task GetByName_User_Returns() {
        // Arrange
        HttpClient client = GetClient();
        UserEntity baseEntity = _storage.Users[0];
        string url = $"/api/User/ByName/{baseEntity.Name}/true";

        // Act
        HttpResponseMessage response = await client.GetAsync(url);

        // Assert
        response.EnsureSuccessStatusCode();

        ICollection<UserDetailModel>? resObject = await response.Content.ReadFromJsonAsync<ICollection<UserDetailModel>>();

        Assert.NotNull(resObject);//got response
        Assert.Equal(1, resObject.Count(m => m.Id == baseEntity.Id));
        Assert.True(resObject.Count < _storage.Users.Count);
    }


    [Fact]
    public async Task GetScore_Quiz_Returns() {
        // Arrange
        HttpClient client = GetClient();
        CompletedQuizEntity entity = _storage.CompletedQuizzes[0];
        string url = $"/api/Quiz/Score/{entity.QuizId}/{entity.UserId}";

        // Act
        HttpResponseMessage response = await client.GetAsync(url);

        // Assert
        response.EnsureSuccessStatusCode();

        double? resObject = await response.Content.ReadFromJsonAsync<double>();

        Assert.NotNull(resObject);//got response
        Assert.True(double.Equals(resObject, entity.Score));
    }

    [Fact]
    public async Task Update_User_ReturnsUpdated() {
        // Arrange
        HttpClient client = GetClient();

        UserEntity baseEntity = _storage.Users[0];

        var updatedModel = new UserDetailModel() {
            Id = baseEntity.Id,
            Name = baseEntity.Name + "Updated",
            ProfilePictureUrl = baseEntity.ProfilePictureUrl
        };

        var requestBody = new StringContent(
            JsonConvert.SerializeObject(updatedModel),
            Encoding.UTF8,
            "application/json"
        );

        // Act
        HttpResponseMessage response = await client.PutAsync($"/api/User", requestBody);

        // Assert
        response.EnsureSuccessStatusCode();

        Guid? resObject = await response.Content.ReadFromJsonAsync<Guid>();
        Assert.NotNull(resObject);//got response

        Assert.Equal(resObject, baseEntity.Id);
    }

    [Fact]
    public async Task Update_UserNonExisting_Fails() {
        // Arrange
        HttpClient client = GetClient();
        var newUser = new UserDetailModel() {
            Id = Guid.NewGuid(),
            Name = "My New User",
            ProfilePictureUrl = null
        };

        var requestBody = new StringContent(
            JsonConvert.SerializeObject(newUser),
            Encoding.UTF8,
            "application/json"
        );

        // Act
        HttpResponseMessage response = await client.PutAsync($"/api/User", requestBody);

        //Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Create_UserAlreadyCreated_Fails() {
        // Arrange
        HttpClient client = GetClient();
        UserEntity baseEntity = _storage.Users[0];

        var existingUser = new UserDetailModel() {
            Id = baseEntity.Id,
            Name = baseEntity.Name,
            ProfilePictureUrl = baseEntity.ProfilePictureUrl
        };

        var requestBody = new StringContent(
            JsonConvert.SerializeObject(existingUser),
            Encoding.UTF8,
            "application/json"
        );

        // Act
        HttpResponseMessage response = await client.PostAsync($"/api/User", requestBody);

        //Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Create_User_Returns() {
        // Arrange
        HttpClient client = GetClient();
        var newUser = new UserDetailModel() {
            Id = Guid.NewGuid(),
            Name = "My New User",
            ProfilePictureUrl = null
        };

        var requestBody = new StringContent(
            JsonConvert.SerializeObject(newUser),
            Encoding.UTF8,
            "application/json"
        );

        // Act
        HttpResponseMessage response = await client.PostAsync($"/api/User", requestBody);

        // Assert
        response.EnsureSuccessStatusCode();

        Guid? resObject = await response.Content.ReadFromJsonAsync<Guid>();
        Assert.NotNull(resObject);//got response

        Assert.Equal(resObject, newUser.Id);
    }
}