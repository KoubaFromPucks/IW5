using AutoMapper;
using Quizzer.API.DAL.Entities;
using Quizzer.API.DAL.Repository.Interfaces;
using Quizzer.API.DAL.UnitOfWork;
using Quizzer.Common.Models;
using Quizzer.Extensions;

namespace Quizzer.API.BL.Facades; 
public class QuizFacade : FacadeBase<QuizEntity, QuizDetailModel>, IQuizChecker {
    protected override string[] IncludesNavigationPathDetails => new string[] {
        $"{nameof(QuizEntity.Questions)}"
    };

    public QuizFacade(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper) : base(unitOfWorkFactory, mapper) {
    }

    public override QuizDetailModel? GetDetailById(Guid id) {
        QuizDetailModel? quizDetailModel = base.GetDetailById(id);
        if (quizDetailModel is null) {
            return null;
        }

        quizDetailModel.IsEditable = IsQuizEditable(id);
        return quizDetailModel;
    }

    public override ICollection<QuizDetailModel> GetAll() {
        IUnitOfWork uow = p_unitOfWorkFactory.Create();
        IRepository<QuizEntity> quizRepository = uow.GetRepository<QuizEntity>();

        IEnumerable<QuizEntity> entities = quizRepository.GetAll();

        var models = entities.Select(p_mapper.Map<QuizDetailModel>).ToList();

        foreach (QuizDetailModel quiz in models)
            quiz.IsEditable = IsQuizEditable(quiz.Id);

        return models;
    }

    public ICollection<QuizDetailModel> GetAllForUser(Guid userId) {
        ICollection<QuizDetailModel> models = GetAll();

        foreach (QuizDetailModel quiz in models) {
            quiz.IsPlayable = IsQuizPlayableForUser(quiz.Id, userId, quiz.StartTime, quiz.EndTime);
            quiz.CanSeeResults = WasQuizCompletedByUser(userId, quiz.Id);
        }

        return models;
    }

    public bool IsQuizEditable(Guid quizId) {
        IUnitOfWork uow = p_unitOfWorkFactory.Create();
        IRepository<QuizEntity> quizRepository = uow.GetRepository<QuizEntity>();

        QuizEntity? quizEntity = quizRepository.GetById(quizId);
        if (quizEntity is null) {
            return true;
        }

        return uow.GetRepository<CompletedQuizEntity>().GetByPredicate(cq => cq.QuizId == quizId).Count() == 0;
    }

    private bool IsQuizPlayableForUser(Guid quizId, Guid userId, DateTime quizStart, DateTime quizEnd) {
        bool quizRunning = quizStart < DateTime.Now && DateTime.Now < quizEnd;
        return quizRunning && !WasQuizCompletedByUser(userId, quizId);
    }

    private bool WasQuizCompletedByUser(Guid userId, Guid quizId) {
        IUnitOfWork uow = p_unitOfWorkFactory.Create();
        IRepository<CompletedQuizEntity> completedQuizRepository = uow.GetRepository<CompletedQuizEntity>();

        return completedQuizRepository
            .GetByPredicate(qc => qc.QuizId == quizId && qc.UserId == userId && qc.InProgress == false)
            .Count() > 0;
    }

    public Guid Save(QuizDetailModel model, DateTime now) {
        if (!IsQuizEditable(model.Id)) {
            throw new ArgumentException("The quiz can no longer be modified.");
        }

        if (model.StartTime < now) {
            throw new ArgumentException("Quiz cannot start in the past");
        }

        if (model.StartTime > model.EndTime) {
            throw new ArgumentException("Start time is later than end time");
        }

        model.Name = model.Name?.TrimWhiteAndUnprintableChars();

        if (string.IsNullOrEmpty(model.Name)) throw new ArgumentException("Quiz name must not be empty");

        return base.Save(model);
    }

    public new void Delete(Guid id) {
        base.Delete(id);
    }

    public ICollection<QuizListModel> GetQuizByContent(string name, string description, bool exact = false) {
        var nameTrim = name.TrimWhiteAndUnprintableChars();
        var descriptionTrim = description.TrimWhiteAndUnprintableChars();

        if (exact) return GetListByPredicate<QuizListModel>(q => q.Name == nameTrim || q.Description == descriptionTrim);

        string[] splitNames = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        string[] splitDescription = description.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        var namesModels = new List<QuizListModel>();
        bool firstIteration = true;
        foreach (string word in splitNames) {
            var lowerWord = word.ToLower();
            ICollection<QuizListModel> currentModels = GetListByPredicate<QuizListModel>(entity =>
            entity.Name.ToLower().Contains(word.ToLower()));

            if (firstIteration) {
                firstIteration = false;
                namesModels = currentModels.ToList();
            } else {
                namesModels = namesModels.IntersectBy(currentModels.Select(m => m.Id), model => model.Id).ToList();
            }
        }

        var descriptionModels = new List<QuizListModel>();
        firstIteration = true;
        foreach (string word in splitDescription) {
            var lowerWord = word.ToLower();
            ICollection<QuizListModel> currentModels = GetListByPredicate<QuizListModel>(entity =>
            entity.Description != null && entity.Description.ToLower().Contains(word.ToLower()));

            if (firstIteration) {
                firstIteration = false;
                descriptionModels = currentModels.ToList();
            } else {
                descriptionModels = descriptionModels.IntersectBy(currentModels.Select(m => m.Id), model => model.Id).ToList();
            }
        }

        return namesModels.UnionBy(descriptionModels, model => model.Id).ToList();
    }

    public ICollection<QuizListModel> GetQuizBetween(DateTime start, DateTime end, bool includeMarginValues = true) {
        return GetListByPredicate<QuizListModel>(quiz =>
            (quiz.StartTime > start || (quiz.StartTime == start && includeMarginValues)) &&
            (quiz.EndTime < end || (quiz.EndTime == end && includeMarginValues)));
    }

    public ICollection<QuizListModel> GetQuizAfter(DateTime startTime, bool includeMarginValues = true) {
        return GetQuizBetween(startTime, DateTime.MaxValue, includeMarginValues);
    }

    public ICollection<QuizListModel> GetQuizBefore(DateTime endTime, bool includeMarginValues = true) {
        return GetQuizBetween(DateTime.MinValue, endTime, includeMarginValues);
    }

    public double GetQuizScore(Guid quizId, Guid userId) {
        using IUnitOfWork uow = p_unitOfWorkFactory.Create();

        IRepository<CompletedQuizEntity> repository = uow.GetRepository<CompletedQuizEntity>();

        CompletedQuizEntity? entity = repository.GetByPredicate(e => e.QuizId == quizId && e.UserId == userId,
            nameof(CompletedQuizEntity.Quiz), nameof(CompletedQuizEntity.User)).SingleOrDefault();

        if (entity is null || entity.InProgress) return double.NaN;

        return entity.Score;
    }

    public void StartQuiz(Guid userId, Guid quizId, DateTime time) {
        using IUnitOfWork uow = p_unitOfWorkFactory.Create();

        IRepository<QuizEntity> quizRepository = uow.GetRepository<QuizEntity>();
        IRepository<UserEntity> userRepository = uow.GetRepository<UserEntity>();
        IRepository<CompletedQuizEntity> completedQuizRepository = uow.GetRepository<CompletedQuizEntity>();

        QuizEntity quiz = quizRepository.GetById(quizId) ??
            throw new ArgumentException("Quiz does not exist");

        UserEntity user = userRepository.GetById(userId) ??
            throw new ArgumentException("User does not exist");

        CompletedQuizEntity? completedQuiz = completedQuizRepository.GetByPredicate(
            q => q.UserId == userId && q.QuizId == quizId).SingleOrDefault();

        if (completedQuiz is not null) throw new InvalidOperationException("Quiz has been started");

        if (time < quiz.StartTime || quiz.EndTime < time) throw new InvalidOperationException("Quiz is not opened");

        completedQuizRepository.Insert(new CompletedQuizEntity() {
            Id = Guid.NewGuid(),
            Score = 0,
            InProgress = true,
            QuizId = quizId,
            UserId = userId
        });

        uow.Commit();
    }

    public void CompleteQuiz(Guid quizId, Guid userId) {
        using IUnitOfWork uow = p_unitOfWorkFactory.Create();
        IRepository<CompletedQuizEntity> completedQuizRepository = uow.GetRepository<CompletedQuizEntity>();

        CompletedQuizEntity? completedQuiz = completedQuizRepository.GetByPredicate(
            q => q.QuizId == quizId && q.UserId == userId,
            nameof(CompletedQuizEntity.Quiz), nameof(CompletedQuizEntity.User)).SingleOrDefault()
            ?? throw new InvalidOperationException("Quiz has not been started");

        if (!completedQuiz.InProgress) throw new InvalidOperationException("Quiz has been completed");

        completedQuiz.InProgress = false;

        IRepository<UserAnswerEntity> userAnswerRepository = uow.GetRepository<UserAnswerEntity>();
        IEnumerable<UserAnswerEntity> answers = userAnswerRepository.GetByPredicate(
            a => a.UserId == userId && a.Question!.QuizId == quizId,
            nameof(UserAnswerEntity.Question), $"{nameof(UserAnswerEntity.Question)}.{nameof(QuestionEntity.Quiz)}");

        completedQuiz.Score = answers.Select(a => a.AnswerScore).Sum();
        completedQuizRepository.Update(completedQuiz);

        uow.Commit();
    }

    public QuizResultDetailModel GetResultModel(Guid quizId, Guid userId, DateTime time) {
        using IUnitOfWork uow = p_unitOfWorkFactory.Create();

        IRepository<QuizEntity> quizRepository = uow.GetRepository<QuizEntity>();

        QuizEntity? quizEntity = quizRepository.GetById(quizId, nameof(QuizEntity.CompletedQuizzes)) ??
            throw new ArgumentException("Quiz does not exist");
        CompletedQuizEntity? quizResult = quizEntity.CompletedQuizzes.FirstOrDefault(q => q.UserId == userId);

        if (quizResult is null || quizResult.InProgress) throw new ArgumentException("Quiz was not completed");

        QuizResultDetailModel model = p_mapper.Map<QuizResultDetailModel>(quizEntity);

        model.UserScore = quizResult.Score;
        model.MaxScore = quizEntity.CompletedQuizzes.Max(q => q.Score);

        return model;
    }
}
