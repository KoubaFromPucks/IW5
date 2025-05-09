using Quizzer.API.DAL.Entities;
using Quizzer.API.DAL.Enums;
using System.Globalization;

namespace Quizzer.API.DAL.Seeds;
public class Storage {
    private IList<Guid> _answerGuids = new List<Guid>() {
        new Guid("8CC8198A-A4E0-47CD-A1C1-35DF84FB60E9"),
        new Guid("8CC8198A-A4E0-47CD-A1C1-35DF84FB60E8"),
        new Guid("8CC8198A-A4E0-47CD-A1C1-35DF84FB60E7"),
        new Guid("8CC8198A-A4E0-47CD-A1C1-35DF84FB60E6"),
        new Guid("8CC8198A-A4E0-47CD-A1C1-35DF84FB60E5"),
        new Guid("8CC8198A-A4E0-47CD-A1C1-35DF84FB60E4"),
        new Guid("8CC8198A-A4E0-47CD-A1C1-35DF84FB60E3"),
        new Guid("8CC8198A-A4E0-47CD-A1C1-35DF84FB60E2"),
        new Guid("8CC8198A-A4E0-47CD-A1C1-35DF84FB60E1"),
        new Guid("8CC8198A-A4E0-47CD-A1C1-35DF84FB60E0"),
        new Guid("8CC8198A-A4E0-47CD-A1C1-35DF84FB60EA"),
        new Guid("8CC8198A-A4E0-47CD-A1C1-35DF84FB60EB"),
        new Guid("8CC8198A-A4E0-47CD-A1C1-35DF84FB60EC"),
        new Guid("8CC8198A-A4E0-47CD-A1C1-35DF84FB60ED"),
        new Guid("8CC8198A-A4E0-47CD-A1C1-35DF84FB60EE"),
        new Guid("8CC8198A-A4E0-47CD-A1C1-35DF84FB60EF"),
        new Guid("F390B866-8251-4CB8-B4A5-B97D39B85F40"),
        new Guid("F390B866-8251-4CB8-B4A5-B97D39B85F41"),
        new Guid("F390B866-8251-4CB8-B4A5-B97D39B85F42"),
        new Guid("F390B866-8251-4CB8-B4A5-B97D39B85F43"),
        new Guid("F390B866-8251-4CB8-B4A5-B97D39B85F44"),
        new Guid("F390B866-8251-4CB8-B4A5-B97D39B85AA0"),
        new Guid("F390B866-8251-4CB8-B4A5-B97D39B85AA1"),
        new Guid("F390B866-8251-4CB8-B4A5-B97D39B85AA2"),
        new Guid("F390B866-8251-4CB8-B4A5-B97D39B85AA3"),
        new Guid("F390B866-8251-4CB8-B4A5-B97D39B85AA4"),
        new Guid("F390B866-8251-4CB8-B4A5-B97D39B85AA5"),
        new Guid("F390B866-8251-4CB8-B4A5-B97D39B85AA6"),
        new Guid("F390B866-8251-4CB8-B4A5-B97D39B85AA7")
    };

    private IList<Guid> _completedQuizGuids = new List<Guid>() {
        new Guid("D4AF9B42-FAC7-4C0A-9BA0-568DFFBE527E"),
        new Guid("D4AF9B42-FAC7-4C0A-9BA0-568DFFBE527F"),
        new Guid("D4AF9B42-FAC7-4C0A-9BA0-568DFFBE527A")
    };

    private IList<Guid> _questionGuids  = new List<Guid>() {
        new Guid("6A415146-6849-4B8B-B45A-345DFB59A66A"),
        new Guid("6A415146-6849-4B8B-B45A-345DFB59A66B"),
        new Guid("6A415146-6849-4B8B-B45A-345DFB59A66C"),
        new Guid("6A415146-6849-4B8B-B45A-345DFB59A66D"),
        new Guid("6A415146-6849-4B8B-B45A-345DFB59A66E"),
        new Guid("6A415146-6849-4B8B-B45A-345DFB59A66F"),
        new Guid("6A415146-6849-4B8B-B45A-345DFB59A660"),
        new Guid("6A415146-6849-4B8B-B45A-345DFB59A661"),
        new Guid("6A415146-6849-4B8B-B45A-345DFB59A662"),
        new Guid("6A415146-6849-4B8B-B45A-345DFB59A663")
    };

    private IList<Guid> _quizGuids  = new List<Guid>() {
        new Guid("228FD582-0581-4C06-A55D-6F4D6110D243"),
        new Guid("228FD582-0581-4C06-A55D-6F4D6110D242"),
        new Guid("228FD582-0581-4C06-A55D-6F4D6110D244")
    };

    private IList<Guid> _selectedAnswerGuids  = new List<Guid>() {
        new Guid("BE3D1D84-AE2F-4996-A6D8-5B652383CC50"),
        new Guid("BE3D1D84-AE2F-4996-A6D8-5B652383CC51"),
        new Guid("BE3D1D84-AE2F-4996-A6D8-5B652383CC52"),
        new Guid("BE3D1D84-AE2F-4996-A6D8-5B652383CC53"),
        new Guid("BE3D1D84-AE2F-4996-A6D8-5B652383CC54"),
        new Guid("BE3D1D84-AE2F-4996-A6D8-5B652383CC55"),
        new Guid("BE3D1D84-AE2F-4996-A6D8-5B652383CC56"),
        new Guid("BE3D1D84-AE2F-4996-A6D8-5B652383CC57"),
        new Guid("BE3D1D84-AE2F-4996-A6D8-5B652383CC58"),
        new Guid("BE3D1D84-AE2F-4996-A6D8-5B652383CC59"),
        new Guid("BE3D1D84-AE2F-4996-A6D8-5B652383CC5A"),
        new Guid("BE3D1D84-AE2F-4996-A6D8-5B652383CC5B"),
        new Guid("BE3D1D84-AE2F-4996-A6D8-5B652383CC5C"),
    };

    private IList<Guid> _userAnswerGuids  = new List<Guid>() {
        new Guid("A6E5C185-71F0-42AE-A9E5-4AC47AACCA70"),
        new Guid("A6E5C185-71F0-42AE-A9E5-4AC47AACCA71"),
        new Guid("A6E5C185-71F0-42AE-A9E5-4AC47AACCA72"),
        new Guid("A6E5C185-71F0-42AE-A9E5-4AC47AACCA73"),
        new Guid("A6E5C185-71F0-42AE-A9E5-4AC47AACCA74"),
        new Guid("A6E5C185-71F0-42AE-A9E5-4AC47AACCA75"),
        new Guid("A6E5C185-71F0-42AE-A9E5-4AC47AACCA76"),
        new Guid("A6E5C185-71F0-42AE-A9E5-4AC47AACCA77"),
        new Guid("A6E5C185-71F0-42AE-A9E5-4AC47AACCA78")
    };

    private IList<Guid> _userGuids = new List<Guid>() {
        new Guid("9B68BC92-DF04-48EE-A7A4-5060E517DFCC"),
        new Guid("9B68BC92-DF04-48EE-A7A4-5060E517DFCD"),
        new Guid("9B68BC92-DF04-48EE-A7A4-5060E517DFCE"),
        new Guid("9B68BC92-DF04-48EE-A7A4-5060E517DFCF"),
        new Guid("9B68BC92-DF04-48EE-A7A4-5060E517DFC0")
    };

    public IList<QuizEntity> Quizzes { get; } = new List<QuizEntity>();
    public IList<CompletedQuizEntity> CompletedQuizzes { get; } = new List<CompletedQuizEntity>();
    public IList<QuestionEntity> Questions { get; } = new List<QuestionEntity>();
    public IList<AnswerEntity> Answers { get; } = new List<AnswerEntity>();
    public IList<SelectedAnswerEntity> SelectedAnswers { get; } = new List<SelectedAnswerEntity>();
    public IList<UserEntity> Users { get; } = new List<UserEntity>();
    public IList<UserAnswerEntity> UserAnswers { get; } = new List<UserAnswerEntity>();

    public Guid QuizBritainId => _quizGuids[0];
    public Guid QuizLotrId => _quizGuids[1];
    public Guid QuizNewId => _quizGuids[2];

    public Guid KurtCompletedQuizBritainId => _completedQuizGuids[0];
    public Guid UlrichCompletedQuizBritainId => _completedQuizGuids[1];
    public Guid UlrichCompletedQuizLotrId => _completedQuizGuids[2];

    public Guid QuizBritainQuestion1Id => _questionGuids[0];
    public Guid QuizBritainQuestion2Id => _questionGuids[1];
    public Guid QuizBritainQuestion3Id => _questionGuids[2];
    public Guid QuizLotrQuestion1Id => _questionGuids[3];
    public Guid QuizLotrQuestion2Id => _questionGuids[4];
    public Guid QuizLotrQuestion3Id => _questionGuids[5];
    public Guid QuizLotrQuestion4Id => _questionGuids[6];
    public Guid QuizNewQuestion1 => _questionGuids[7];
    public Guid QuizNewQuestion2 => _questionGuids[8];
    public Guid QuizNewQuestion3 => _questionGuids[9];

    public Guid QuizBritainQuestion1Answer1Id => _answerGuids[0];
    public Guid QuizBritainQuestion1Answer2Id => _answerGuids[1];
    public Guid QuizBritainQuestion1Answer3Id => _answerGuids[2];
    public Guid QuizBritainQuestion2Answer1Id => _answerGuids[3];
    public Guid QuizBritainQuestion2Answer2Id => _answerGuids[4];
    public Guid QuizBritainQuestion2Answer3Id => _answerGuids[5];
    public Guid QuizBritainQuestion2Answer4Id => _answerGuids[6];
    public Guid QuizBritainQuestion3Answer1Id => _answerGuids[7];
    public Guid QuizBritainQuestion3Answer2Id => _answerGuids[8];
    public Guid QuizLotrQuestion1Answer1Id => _answerGuids[9];
    public Guid QuizLotrQuestion1Answer2Id => _answerGuids[10];
    public Guid QuizLotrQuestion1Answer3Id => _answerGuids[11];
    public Guid QuizLotrQuestion2Answer1Id => _answerGuids[12];
    public Guid QuizLotrQuestion2Answer2Id => _answerGuids[13];
    public Guid QuizLotrQuestion2Answer3Id => _answerGuids[14];
    public Guid QuizLotrQuestion2Answer4Id => _answerGuids[15];
    public Guid QuizLotrQuestion3Answer1Id => _answerGuids[16];
    public Guid QuizLotrQuestion3Answer2Id => _answerGuids[17];
    public Guid QuizLotrQuestion4Answer1Id => _answerGuids[18];
    public Guid QuizLotrQuestion4Answer2Id => _answerGuids[19];
    public Guid QuizLotrQuestion4Answer3Id => _answerGuids[20];
    public Guid QuizNewQuestion1Answer1Id => _answerGuids[21];
    public Guid QuizNewQuestion1Answer2Id => _answerGuids[22];
    public Guid QuizNewQuestion2Answer1Id => _answerGuids[23];
    public Guid QuizNewQuestion2Answer2Id => _answerGuids[24];
    public Guid QuizNewQuestion2Answer3Id => _answerGuids[25];
    public Guid QuizNewQuestion3Answer1Id => _answerGuids[26];
    public Guid QuizNewQuestion3Answer2Id => _answerGuids[27];
    public Guid QuizNewQuestion3Answer3Id => _answerGuids[28];

    public Guid UserKurtId => _userGuids[0];
    public Guid UserUlrichId => _userGuids[1];
    public Guid UserTolkienId => _userGuids[2];
    public Guid UserNolanId => _userGuids[3];
    public Guid UserHelenkaId => _userGuids[4];

    public Guid KurtSelectedQuizBritainQuestion1Id => _userAnswerGuids[0];
    public Guid KurtSelectedQuizBritainQuestion2Id => _userAnswerGuids[1];
    public Guid KurtSelectedQuizBritainQuestion3Id => _userAnswerGuids[2];
    public Guid UlrichSelectedQuizBritainQuestion1Id => _userAnswerGuids[3];
    public Guid UlrichSelectedQuizBritainQuestion3Id => _userAnswerGuids[4];
    public Guid UlrichSelectedQuizLotrQuestion1Id => _userAnswerGuids[5];
    public Guid UlrichSelectedQuizLotrQuestion2Id => _userAnswerGuids[6];
    public Guid UlrichSelectedQuizLotrQuestion3Id => _userAnswerGuids[7];
    public Guid UlrichSelectedQuizLotrQuestion4Id => _userAnswerGuids[8];

    public Storage(bool seedData) {
        if (seedData) {
            SeedQuizzes();
            SeedUsers();
            SeedQuestions();
            SeedAnswers();
            SeedCompletedQuizzes();
            SeedUserAnswers();
            SeedSelectedAnswers();
        }
    }

    private void SeedQuizzes() {
        Quizzes.Add(new QuizEntity {
            Id = QuizBritainId,
            StartTime = DateTime.Parse("15.01.2023 00:00", CultureInfo.CreateSpecificCulture("fr-FR")),
            EndTime = DateTime.Parse("02.11.2024 19:34", CultureInfo.CreateSpecificCulture("fr-FR")),
            Name = "Great Britain",
            Description = "Do you know everything about Great Britain?"
        });

        Quizzes.Add(new QuizEntity {
            Id = QuizLotrId,
            StartTime = DateTime.Parse("14.01.2023 15:32", CultureInfo.CreateSpecificCulture("fr-FR")),
            EndTime = DateTime.Parse("13.08.2023 23:59", CultureInfo.CreateSpecificCulture("fr-FR")),
            Name = "LOTR",
            Description = "What you know about Lord of the rings?"
        });

        Quizzes.Add(new QuizEntity {
            Id = QuizNewId,
            StartTime = DateTime.Parse("14.01.2025 15:32", System.Globalization.CultureInfo.CreateSpecificCulture("fr-FR")),
            EndTime = DateTime.Parse("13.08.2027 23:59", System.Globalization.CultureInfo.CreateSpecificCulture("fr-FR")),
            Name = "New long term quiz",
            Description = "We will see what it brings"
        });
    }

    private void SeedUsers() {
        Users.Add(new UserEntity { Id = UserKurtId, Name = "Kurt Godel", ProfilePictureUrl = null });

        Users.Add(new UserEntity {
            Id = UserUlrichId,
            Name = "Ulrich Lichtenstain",
            ProfilePictureUrl = "https://pbs.twimg.com/media/Dc69SdVWsAAK5Vw?format=jpg&name=4096x4096"
        });

        Users.Add(new UserEntity {
            Id = UserHelenkaId,
            Name = "Helena Vondráčková",
            ProfilePictureUrl = "https://www.play.cz/wp-content/uploads/2020/08/helena_vondrackova_web.jpg"
        });

        Users.Add(new UserEntity {
            Id = UserNolanId,
            Name = "Christopher Nolan",
            ProfilePictureUrl = "https://people.com/thmb/7lgnSJi5gGLOIJsdj8P6BVxHDfY=/1500x0/filters:no_upscale():max_bytes(150000):strip_icc():focal(674x429:676x431)/christopher-nolan-dc23745902014fb6ae107bb65a10c384.jpg"
        });

        Users.Add(new UserEntity {
            Id = UserTolkienId,
            Name = "John Ronald Reuel Tolkien",
            ProfilePictureUrl = "https://naposlech.cz/getattachment/cf1fa495-a698-4d0e-9ec3-e3a57f28053d/J-R-R-Tolkien.aspx?maxsidesize=767"
        });
    }

    private void SeedQuestions() {
        Questions.Add(new QuestionEntity {
            Id = QuizBritainQuestion1Id,
            Type = AnswerFormat.MultiChoice,
            Text = "What is located in Great Britain?",
            QuizId = QuizBritainId
        });

        Questions.Add(new QuestionEntity {
            Id = QuizBritainQuestion2Id,
            Type = AnswerFormat.SingleChoice,
            Text = "Who is the king of great britain?",
            QuizId = QuizBritainId
        });

        Questions.Add(new QuestionEntity {
            Id = QuizBritainQuestion3Id,
            Type = AnswerFormat.SingleChoice,
            Text = "Is Austria part of British commonwealth?",
            QuizId = QuizBritainId
        });

        Questions.Add(new QuestionEntity {
            Id = QuizLotrQuestion1Id,
            Type = AnswerFormat.SingleChoice,
            Text = "Who is ruler of all Valar and Mayar?",
            QuizId = QuizLotrId
        });

        Questions.Add(new QuestionEntity {
            Id = QuizLotrQuestion2Id,
            Type = AnswerFormat.MultiChoice,
            Text = "Who belongs to Valar?",
            QuizId = QuizLotrId
        });

        Questions.Add(new QuestionEntity {
            Id = QuizLotrQuestion3Id, Type = AnswerFormat.SingleChoice, Text = "Was Gandalf Mayar?", QuizId = QuizLotrId
        });

        Questions.Add(new QuestionEntity {
            Id = QuizLotrQuestion4Id,
            Type = AnswerFormat.OrderChoice,
            Text = "Order characters by height (1 = heighest)",
            QuizId = QuizLotrId
        });

        Questions.Add(new QuestionEntity {
            Id = QuizNewQuestion1,
            Type = Enums.AnswerFormat.SingleChoice,
            Text = "Some single choice question",
            QuizId = QuizNewId
        });

        Questions.Add(new QuestionEntity {
            Id = QuizNewQuestion2,
            Type = Enums.AnswerFormat.MultiChoice,
            Text = "Some multi choice question",
            QuizId = QuizNewId
        });

        Questions.Add(new QuestionEntity {
            Id = QuizNewQuestion3,
            Type = Enums.AnswerFormat.OrderChoice,
            Text = "Some order question",
            QuizId = QuizNewId
        });
    }

    private void SeedAnswers() {
        Answers.Add(new AnswerEntity {
            Id = QuizBritainQuestion1Answer1Id,
            Text = "Big Ben",
            PictureUrl =
                "https://upload.wikimedia.org/wikipedia/commons/thumb/f/f2/Elizabeth_Tower%2C_Palace_of_Westminster_-_west_view.jpg/320px-Elizabeth_Tower%2C_Palace_of_Westminster_-_west_view.jpg",
            IsCorrect = true,
            Order = 0,
            QuestionId = QuizBritainQuestion1Id
        });

        Answers.Add(new AnswerEntity {
            Id = QuizBritainQuestion1Answer2Id,
            Text = "Stonehenge",
            PictureUrl =
                "https://www.english-heritage.org.uk/siteassets/home/visit/places-to-visit/stonehenge/history/significance/stonehenge-from-north-east.jpg?w=1440&h=612&mode=crop&scale=both&quality=100&anchor=NoFocus&WebsiteVersion=20230920162321",
            IsCorrect = true,
            Order = 0,
            QuestionId = QuizBritainQuestion1Id
        });

        Answers.Add(new AnswerEntity {
            Id = QuizBritainQuestion1Answer3Id,
            Text = "Eiffel tower",
            PictureUrl =
                "https://upload.wikimedia.org/wikipedia/commons/thumb/8/85/Tour_Eiffel_Wikimedia_Commons_%28cropped%29.jpg/800px-Tour_Eiffel_Wikimedia_Commons_%28cropped%29.jpg",
            IsCorrect = false,
            Order = 0,
            QuestionId = QuizBritainQuestion1Id
        });

        Answers.Add(new AnswerEntity {
            Id = QuizBritainQuestion2Answer1Id,
            Text = "Charles III",
            PictureUrl = null,
            IsCorrect = true,
            Order = 0,
            QuestionId = QuizBritainQuestion2Id
        });

        Answers.Add(new AnswerEntity {
            Id = QuizBritainQuestion2Answer2Id,
            Text = "William",
            PictureUrl = null,
            IsCorrect = false,
            Order = 0,
            QuestionId = QuizBritainQuestion2Id
        });

        Answers.Add(new AnswerEntity {
            Id = QuizBritainQuestion2Answer3Id,
            Text = "Petr Pavel",
            PictureUrl = null,
            IsCorrect = false,
            Order = 0,
            QuestionId = QuizBritainQuestion2Id
        });

        Answers.Add(new AnswerEntity {
            Id = QuizBritainQuestion2Answer4Id,
            Text = "Karel Gott",
            PictureUrl = null,
            IsCorrect = false,
            Order = 0,
            QuestionId = QuizBritainQuestion2Id
        });

        Answers.Add(new AnswerEntity {
            Id = QuizBritainQuestion3Answer1Id,
            Text = "No",
            PictureUrl = null,
            IsCorrect = true,
            Order = 0,
            QuestionId = QuizBritainQuestion3Id
        });

        Answers.Add(new AnswerEntity {
            Id = QuizBritainQuestion3Answer2Id,
            Text = "Yes",
            PictureUrl = null,
            IsCorrect = false,
            Order = 0,
            QuestionId = QuizBritainQuestion3Id
        });

        Answers.Add(new AnswerEntity {
            Id = QuizLotrQuestion1Answer1Id,
            Text = "Gandalf",
            PictureUrl = null,
            IsCorrect = false,
            Order = 0,
            QuestionId = QuizLotrQuestion1Id
        });

        Answers.Add(new AnswerEntity {
            Id = QuizLotrQuestion1Answer2Id,
            Text = "Glum",
            PictureUrl = null,
            IsCorrect = false,
            Order = 0,
            QuestionId = QuizLotrQuestion1Id
        });

        Answers.Add(new AnswerEntity {
            Id = QuizLotrQuestion1Answer3Id,
            Text = "Eru Ilúvatar",
            PictureUrl = null,
            IsCorrect = true,
            Order = 0,
            QuestionId = QuizLotrQuestion1Id
        });

        Answers.Add(new AnswerEntity {
            Id = QuizLotrQuestion2Answer1Id,
            Text = "Frodo Bagins",
            PictureUrl = null,
            IsCorrect = false,
            Order = 0,
            QuestionId = QuizLotrQuestion2Id
        });

        Answers.Add(new AnswerEntity {
            Id = QuizLotrQuestion2Answer2Id,
            Text = "Aulë",
            PictureUrl = null,
            IsCorrect = true,
            Order = 0,
            QuestionId = QuizLotrQuestion2Id
        });

        Answers.Add(new AnswerEntity {
            Id = QuizLotrQuestion2Answer3Id,
            Text = "Varda",
            PictureUrl = null,
            IsCorrect = true,
            Order = 0,
            QuestionId = QuizLotrQuestion2Id
        });

        Answers.Add(new AnswerEntity {
            Id = QuizLotrQuestion2Answer4Id,
            Text = "Gandalf",
            PictureUrl = null,
            IsCorrect = false,
            Order = 0,
            QuestionId = QuizLotrQuestion2Id
        });

        Answers.Add(new AnswerEntity {
            Id = QuizLotrQuestion3Answer1Id,
            Text = "",
            PictureUrl =
                "https://upload.wikimedia.org/wikipedia/commons/thumb/c/c1/Yes_._logo.svg/1200px-Yes_._logo.svg.png",
            IsCorrect = true,
            Order = 0,
            QuestionId = QuizLotrQuestion3Id
        });

        Answers.Add(new AnswerEntity {
            Id = QuizLotrQuestion3Answer2Id,
            Text = "",
            PictureUrl = "https://content.artofmanliness.com/uploads/2022/02/no-header.jpg",
            IsCorrect = false,
            Order = 0,
            QuestionId = QuizLotrQuestion3Id
        });

        Answers.Add(new AnswerEntity {
            Id = QuizLotrQuestion4Answer1Id,
            Text = "",
            PictureUrl = "https://qph.cf2.quoracdn.net/main-qimg-fe92b847c428a86efc8697fd3eff4d89-lq",
            IsCorrect = false,
            Order = 1,
            QuestionId = QuizLotrQuestion4Id
        });

        Answers.Add(new AnswerEntity {
            Id = QuizLotrQuestion4Answer2Id,
            Text = "",
            PictureUrl =
                "https://cdn.administrace.tv/2023/04/07/hd/cd3196f22bb50792f401c5ec743be1dc.jpg",
            IsCorrect = false,
            Order = 2,
            QuestionId = QuizLotrQuestion4Id
        });

        Answers.Add(new AnswerEntity {
            Id = QuizLotrQuestion4Answer3Id,
            Text = "",
            PictureUrl =
                "https://a1cf74336522e87f135f-2f21ace9a6cf0052456644b80fa06d4f.ssl.cf2.rackcdn.com/images/characters/large/800/Pippin-Took.The-Lord-of-the-Rings-The-Fellowship-of-the-Ring.webp",
            IsCorrect = false,
            Order = 3,
            QuestionId = QuizLotrQuestion4Id
        });

        Answers.Add(new AnswerEntity {
            Id = QuizNewQuestion1Answer1Id,
            Text = "Not correct",
            PictureUrl = "",
            IsCorrect = false,
            Order = 0,
            QuestionId = QuizNewQuestion1
        });

        Answers.Add(new AnswerEntity {
            Id = QuizNewQuestion1Answer2Id,
            Text = "Correct",
            PictureUrl = "",
            IsCorrect = true,
            Order = 0,
            QuestionId = QuizNewQuestion1
        });

        Answers.Add(new AnswerEntity {
            Id = QuizNewQuestion2Answer1Id,
            Text = "Correct",
            PictureUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/c/c1/Yes_._logo.svg/1200px-Yes_._logo.svg.png",
            IsCorrect = true,
            Order = 0,
            QuestionId = QuizNewQuestion2
        });

        Answers.Add(new AnswerEntity {
            Id = QuizNewQuestion2Answer2Id,
            Text = "",
            PictureUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/1/1c/No-Symbol.png/480px-No-Symbol.png",
            IsCorrect = false,
            Order = 0,
            QuestionId = QuizNewQuestion2
        });

        Answers.Add(new AnswerEntity {
            Id = QuizNewQuestion2Answer3Id,
            Text = "Yes",
            PictureUrl = "",
            IsCorrect = true,
            Order = 0,
            QuestionId = QuizNewQuestion2
        });

        Answers.Add(new AnswerEntity {
            Id = QuizNewQuestion3Answer1Id,
            Text = "Order answer 1",
            PictureUrl = "",
            IsCorrect = true,
            Order = 3,
            QuestionId = QuizNewQuestion3
        });

        Answers.Add(new AnswerEntity {
            Id = QuizNewQuestion3Answer2Id,
            Text = "Order answer 2",
            PictureUrl = "",
            IsCorrect = true,
            Order = 1,
            QuestionId = QuizNewQuestion3
        });

        Answers.Add(new AnswerEntity {
            Id = QuizNewQuestion3Answer3Id,
            Text = "Order answer 1",
            PictureUrl = "",
            IsCorrect = true,
            Order = 2,
            QuestionId = QuizNewQuestion3
        });
    }

    private void SeedCompletedQuizzes() {
        CompletedQuizzes.Add(new CompletedQuizEntity {
            Id = KurtCompletedQuizBritainId, Score = 5.0, QuizId = QuizBritainId, UserId = UserKurtId, InProgress = false
        });

        CompletedQuizzes.Add(new CompletedQuizEntity {
            Id = UlrichCompletedQuizBritainId, Score = 0.0, QuizId = QuizBritainId, UserId = UserUlrichId, InProgress = false
        });

        CompletedQuizzes.Add(new CompletedQuizEntity {
            Id = UlrichCompletedQuizLotrId, Score = 7.0, QuizId = QuizLotrId, UserId = UserUlrichId, InProgress = false
        });
    }

    private void SeedUserAnswers() {
        // user 0, quiz 0
        UserAnswers.Add(new UserAnswerEntity {
            Id = KurtSelectedQuizBritainQuestion1Id,
            AnswerTime = DateTime.Parse("15.01.2023 19:34", CultureInfo.CreateSpecificCulture("fr-FR")),
            UserId = UserKurtId,
            QuestionId = QuizBritainQuestion1Id,
            AnswerScore = 3.0
        });

        UserAnswers.Add(new UserAnswerEntity {
            Id = KurtSelectedQuizBritainQuestion2Id,
            AnswerTime = DateTime.Parse("15.01.2023 19:34", CultureInfo.CreateSpecificCulture("fr-FR")),
            UserId = UserKurtId,
            QuestionId = QuizBritainQuestion2Id,
            AnswerScore = 1.0
        });

        UserAnswers.Add(new UserAnswerEntity {
            Id = KurtSelectedQuizBritainQuestion3Id,
            AnswerTime = DateTime.Parse("15.01.2023 19:34", CultureInfo.CreateSpecificCulture("fr-FR")),
            UserId = UserKurtId,
            QuestionId = QuizBritainQuestion3Id,
            AnswerScore = 1.0
        });


        // user 1, quiz 0
        UserAnswers.Add(new UserAnswerEntity {
            Id = UlrichSelectedQuizBritainQuestion1Id,
            AnswerTime = DateTime.Parse("16.01.2023 19:34", CultureInfo.CreateSpecificCulture("fr-FR")),
            UserId = UserUlrichId,
            QuestionId = QuizBritainQuestion1Id,
            AnswerScore = 0.0
        });

        UserAnswers.Add(new UserAnswerEntity {
            Id = UlrichSelectedQuizBritainQuestion3Id,
            AnswerTime = DateTime.Parse("16.01.2023 10:40", CultureInfo.CreateSpecificCulture("fr-FR")),
            UserId = UserUlrichId,
            QuestionId = QuizBritainQuestion3Id,
            AnswerScore = 0.0
        });

        // user 1, quiz 1
        UserAnswers.Add(new UserAnswerEntity {
            Id = UlrichSelectedQuizLotrQuestion1Id,
            AnswerTime = DateTime.Parse("18.01.2023 10:42", CultureInfo.CreateSpecificCulture("fr-FR")),
            UserId = UserUlrichId,
            QuestionId = QuizLotrQuestion1Id,
            AnswerScore = 1.0
        });

        UserAnswers.Add(new UserAnswerEntity {
            Id = UlrichSelectedQuizLotrQuestion2Id,
            AnswerTime = DateTime.Parse("18.01.2023 10:43", CultureInfo.CreateSpecificCulture("fr-FR")),
            UserId = UserUlrichId,
            QuestionId = QuizLotrQuestion2Id,
            AnswerScore = 4.0
        });

        UserAnswers.Add(new UserAnswerEntity {
            Id = UlrichSelectedQuizLotrQuestion3Id,
            AnswerTime = DateTime.Parse("18.01.2023 19:48", CultureInfo.CreateSpecificCulture("fr-FR")),
            UserId = UserUlrichId,
            QuestionId = QuizLotrQuestion3Id,
            AnswerScore = 1.0
        });

        UserAnswers.Add(new UserAnswerEntity {
            Id = UlrichSelectedQuizLotrQuestion4Id,
            AnswerTime = DateTime.Parse("18.01.2023 19:50", CultureInfo.CreateSpecificCulture("fr-FR")),
            UserId = UserUlrichId,
            QuestionId = QuizLotrQuestion4Id,
            AnswerScore = 1.0
        });
    }

    private void SeedSelectedAnswers() {
        // user 0, quiz 0
        SelectedAnswers.Add(new SelectedAnswerEntity {
            Id = _selectedAnswerGuids[0],
            Order = 0,
            UserAnswerId = KurtSelectedQuizBritainQuestion1Id,
            AnswerId = QuizBritainQuestion1Answer1Id
        });

        SelectedAnswers.Add(new SelectedAnswerEntity {
            Id = _selectedAnswerGuids[1],
            Order = 0,
            UserAnswerId = KurtSelectedQuizBritainQuestion1Id,
            AnswerId = QuizBritainQuestion1Answer2Id
        });

        SelectedAnswers.Add(new SelectedAnswerEntity {
            Id = _selectedAnswerGuids[2],
            Order = 0,
            UserAnswerId = KurtSelectedQuizBritainQuestion2Id,
            AnswerId = QuizBritainQuestion2Answer1Id
        });

        SelectedAnswers.Add(new SelectedAnswerEntity {
            Id = _selectedAnswerGuids[3],
            Order = 0,
            UserAnswerId = KurtSelectedQuizBritainQuestion3Id,
            AnswerId = QuizBritainQuestion3Answer1Id
        });

        // user 1, quiz 0
        SelectedAnswers.Add(new SelectedAnswerEntity {
            Id = _selectedAnswerGuids[4],
            Order = 0,
            UserAnswerId = UlrichSelectedQuizBritainQuestion1Id,
            AnswerId = QuizBritainQuestion1Answer3Id
        });

        SelectedAnswers.Add(new SelectedAnswerEntity {
            Id = _selectedAnswerGuids[5],
            Order = 0,
            UserAnswerId = UlrichSelectedQuizBritainQuestion3Id,
            AnswerId = QuizBritainQuestion3Answer2Id
        });

        // user 1, quiz 1
        SelectedAnswers.Add(new SelectedAnswerEntity {
            Id = _selectedAnswerGuids[6],
            Order = 0,
            UserAnswerId = UlrichSelectedQuizLotrQuestion1Id,
            AnswerId = QuizLotrQuestion1Answer3Id
        });

        SelectedAnswers.Add(new SelectedAnswerEntity {
            Id = _selectedAnswerGuids[7],
            Order = 0,
            UserAnswerId = UlrichSelectedQuizLotrQuestion2Id,
            AnswerId = QuizLotrQuestion2Answer2Id
        });

        SelectedAnswers.Add(new SelectedAnswerEntity {
            Id = _selectedAnswerGuids[8],
            Order = 0,
            UserAnswerId = UlrichSelectedQuizLotrQuestion2Id,
            AnswerId = QuizLotrQuestion2Answer3Id
        });

        SelectedAnswers.Add(new SelectedAnswerEntity {
            Id = _selectedAnswerGuids[9],
            Order = 0,
            UserAnswerId = UlrichSelectedQuizLotrQuestion3Id,
            AnswerId = QuizLotrQuestion3Answer1Id
        });

        SelectedAnswers.Add(new SelectedAnswerEntity {
            Id = _selectedAnswerGuids[10],
            Order = 3,
            UserAnswerId = UlrichSelectedQuizLotrQuestion4Id,
            AnswerId = QuizLotrQuestion4Answer1Id
        });

        SelectedAnswers.Add(new SelectedAnswerEntity {
            Id = _selectedAnswerGuids[11],
            Order = 2,
            UserAnswerId = UlrichSelectedQuizLotrQuestion4Id,
            AnswerId = QuizLotrQuestion4Answer2Id
        });

        SelectedAnswers.Add(new SelectedAnswerEntity {
            Id = _selectedAnswerGuids[12],
            Order = 1,
            UserAnswerId = UlrichSelectedQuizLotrQuestion4Id,
            AnswerId = QuizLotrQuestion4Answer3Id
        });
    }
}