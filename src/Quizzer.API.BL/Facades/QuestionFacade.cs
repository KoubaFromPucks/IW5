// Ignore Spelling: API

using AutoMapper;
using Quizzer.API.DAL.Entities;
using Quizzer.API.DAL.Enums;
using Quizzer.API.DAL.Repository.Interfaces;
using Quizzer.API.DAL.UnitOfWork;
using Quizzer.Common.Models;
using Quizzer.Extensions;

namespace Quizzer.API.BL.Facades;
public class QuestionFacade : FacadeBase<QuestionEntity, QuestionDetailModel> {
    private IQuizChecker _quizChecker;
    public QuestionFacade(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper, IQuizChecker quizChecker) : base(unitOfWorkFactory, mapper) {
        _quizChecker = quizChecker;
    }

    protected override string[] IncludesNavigationPathDetails => new string[] {
        $"{nameof(QuestionEntity.Answers)}" ,
        $"{nameof(QuestionEntity.UserAnswers)}"
    };

    public override QuestionDetailModel? GetDetailById(Guid id) {
        QuestionDetailModel? model = base.GetDetailById(id);

        if (model is null) return null;

        model.IsQuizEditable = _quizChecker.IsQuizEditable(model.QuizId);

        foreach (AnswerDetailModel answer in model.Answers) {
            answer.Type = model.QuestionType;
            answer.IsQuizEditable = model.IsQuizEditable;
        }

        return model;
    }

    public override ICollection<QuestionDetailModel> GetAll() {
        ICollection<QuestionDetailModel> models = base.GetAll();

        foreach (QuestionDetailModel model in models)
            model.IsQuizEditable = _quizChecker.IsQuizEditable(model.QuizId);

        return models;
    }

    public ICollection<QuestionDetailModel> GetQuestionByName(string name, bool exactMatch = false) {
        string nameTrim = name.TrimWhiteAndUnprintableChars();

        if (exactMatch) return GetListByPredicate<QuestionDetailModel>(q => q.Text == nameTrim, IncludesNavigationPathDetails);

        string[] splitWords = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        var resultModels = new List<QuestionDetailModel>();
        bool firstIteration = true;
        foreach (string word in splitWords) {
            string lowerWord = word.ToLower();
            ICollection<QuestionDetailModel> currentModels = GetListByPredicate<QuestionDetailModel>(entity =>
            entity.Text.ToLower().Contains(word.ToLower()));

            if (firstIteration) {
                firstIteration = false;
                resultModels = currentModels.ToList();
            } else {
                resultModels = resultModels.IntersectBy(currentModels.Select(m => m.Id), model => model.Id).ToList();
            }
        }

        foreach (QuestionDetailModel question in resultModels)
            question.IsQuizEditable = _quizChecker.IsQuizEditable(question.QuizId);

        return resultModels;
    }

    public ICollection<QuestionResultModel> GetResultsForUser(Guid quizId, Guid userId) {
        using IUnitOfWork uow = p_unitOfWorkFactory.Create();

        IRepository<QuestionEntity> repository = uow.GetRepository<QuestionEntity>();

        IEnumerable<QuestionEntity> questionEntities = repository.GetByPredicate(q => q.QuizId == quizId, nameof(QuestionEntity.Answers),
            nameof(QuestionEntity.UserAnswers), $"{nameof(QuestionEntity.UserAnswers)}.{nameof(UserAnswerEntity.SelectedAnswers)}");

        var questionModels = new List<QuestionResultModel>();
        foreach (QuestionEntity questionEntity in questionEntities) {

            QuestionResultModel quesitonModel = p_mapper.Map<QuestionResultModel>(questionEntity);

            List<AnswerResultModel> answerModels = MapAnswers(userId, questionEntity);
            quesitonModel.Answers = answerModels;

            quesitonModel.IsAnswered = answerModels.Any(a => a.IsAnswered);

            questionModels.Add(quesitonModel);
        }

        return questionModels;
    }

    public Guid Save(QuestionDetailModel model, Guid quizId, DateTime time) {
        if (!_quizChecker.IsQuizEditable(quizId)) throw new ArgumentException("The question can no longer be edited.");

        model.Text = model.Text?.TrimWhiteAndUnprintableChars() ?? string.Empty;
        if (string.IsNullOrEmpty(model.Text)) throw new ArgumentException("Question text must not be empty");

        using IUnitOfWork uow = p_unitOfWorkFactory.Create();
        IRepository<QuizEntity> quizRepository = uow.GetRepository<QuizEntity>();
        IRepository<QuestionEntity> questionRepository = uow.GetRepository<QuestionEntity>();

        QuestionEntity? entity = questionRepository.GetById(model.Id, nameof(QuestionEntity.Answers));

        if (!quizRepository.Exists(quizId)) throw new ArgumentException("Quiz does not exist");
        if (string.IsNullOrEmpty(model.Text)) throw new ArgumentException("Question text must not be empty");

        if (entity is null) {
            QuestionEntity newEntity = p_mapper.Map<QuestionEntity>(model);
            newEntity.QuizId = quizId;
            return Save(newEntity);
        }

        HandleFormatChange(entity, model.QuestionType);

        return Save(model);
    }

    public new void Delete(Guid id) {
        IUnitOfWork uow = p_unitOfWorkFactory.Create();
        IRepository<QuestionEntity> questionRepository = uow.GetRepository<QuestionEntity>();

        QuestionEntity? questionEntity = questionRepository.GetById(id);
        if (questionEntity is null) {
            throw new ArgumentException("Given question entity does not exists");
        }

        if (!_quizChecker.IsQuizEditable(questionEntity.QuizId)) {
            throw new ArgumentException("The question can no longer be edited.");
        }

        base.Delete(id);
    }

    public void AnswerQuestion(Guid answerId, Guid userId, DateTime answerTime, int order = 0) {
        using IUnitOfWork uow = p_unitOfWorkFactory.Create();
        IRepository<SelectedAnswerEntity> selectedAnswerRepository = uow.GetRepository<SelectedAnswerEntity>();
        IRepository<UserAnswerEntity> userAnswerRepository = uow.GetRepository<UserAnswerEntity>();


        AnswerEntity answerEntity = GetAnswerWithQuestionAndQuiz(answerId, uow);

        CheckQuizTimeFrame(answerTime, answerEntity);

        UserAnswerEntity userAnswer = GetUserAnswer(userId, answerTime, uow, answerEntity.QuestionId);

        if (userAnswer.Question is null) throw new ArgumentNullException("Question in user answer is null");

        switch (answerEntity.Question?.Type) {
            case AnswerFormat.SingleChoice:
                HandleSingleChoice();
                break;

            case AnswerFormat.MultiChoice:
                HandleMultiChoice();
                break;

            case AnswerFormat.OrderChoice:
                HandleOrderChoice();
                break;

            default:
                throw new NotImplementedException("This question format handle is not implemented");
        }

        uow.Commit();


        return;

        #region Nested functions

        void HandleSingleChoice() {
            foreach (SelectedAnswerEntity answer in userAnswer.SelectedAnswers) {
                selectedAnswerRepository.Delete(answer.Id);
            }
            SelectedAnswerEntity newAnswer = NewAnswer(answerEntity!.Id, userAnswer.Id);

            userAnswer.AnswerScore = answerEntity.IsCorrect ? 1 : 0;
            userAnswerRepository.Update(userAnswer);

            selectedAnswerRepository.Insert(newAnswer);

        }

        void HandleMultiChoice() {
            SelectedAnswerEntity? answer = userAnswer.SelectedAnswers.SingleOrDefault(
                s => s.AnswerId == answerEntity!.Id);

            // Toggle behavior
            if (answer is null) {
                SelectedAnswerEntity newAnswer = NewAnswer(answerEntity!.Id, userAnswer.Id);
                selectedAnswerRepository.Insert(newAnswer);
            } else {
                selectedAnswerRepository.Delete(answer.Id);
                userAnswer.SelectedAnswers.Remove(answer);
            }

            IRepository<AnswerEntity> answerRepository = uow.GetRepository<AnswerEntity>();

            userAnswer.AnswerScore = 0;
            foreach (SelectedAnswerEntity selectedAnswer in userAnswer.SelectedAnswers)
                userAnswer.AnswerScore += answerRepository!.GetById(selectedAnswer.AnswerId)!.IsCorrect ? 1.0 : -0.5;

            userAnswer.AnswerScore = Math.Max(userAnswer.AnswerScore, 0.0);
            userAnswerRepository.Update(userAnswer);
        }

        void HandleOrderChoice() {
            SelectedAnswerEntity? answer = userAnswer.SelectedAnswers.SingleOrDefault(
              s => s.AnswerId == answerEntity!.Id);

            IEnumerable<SelectedAnswerEntity> otherAnswersWithSameOrder = userAnswer.SelectedAnswers
                .Where(a => a.AnswerId != answerId && a.Order == order);

            foreach (SelectedAnswerEntity otherAnswer in otherAnswersWithSameOrder) {
                otherAnswer.Order = 0;
                selectedAnswerRepository.Update(otherAnswer);
            }

            if (answer is null) {
                if (order == 0) throw new ArgumentException("Cannot create ordered answer without order");

                answer = NewAnswer(answerEntity!.Id, userAnswer.Id);

                selectedAnswerRepository.Insert(answer);
            }

            if (order == 0) {
                selectedAnswerRepository.Delete(answer.Id);
                userAnswer.SelectedAnswers.Remove(answer);
            } else {
                answer.Order = order;
                selectedAnswerRepository.Update(answer);
            }

            IRepository<AnswerEntity> answerRepository = uow.GetRepository<AnswerEntity>();

            AnswerEntity[] correctAnswers = answerRepository.GetByPredicate(a => a.QuestionId == answerEntity.QuestionId)
                .ToArray();

            userAnswer.AnswerScore = 0;
            foreach (AnswerEntity correctAnswer in correctAnswers) {
                Guid sameOrderdAnswerId = userAnswer
                    .SelectedAnswers
                    .SingleOrDefault(a => a.Order == correctAnswer.Order, null)?.AnswerId ?? Guid.Empty;

                userAnswer.AnswerScore += correctAnswer.Id == sameOrderdAnswerId ? 1 : 0;
            }
            userAnswerRepository.Update(userAnswer);
        }

        #endregion
    }

    private void HandleFormatChange(QuestionEntity entity, AnswerFormat newFormat) {
        if (entity.Type == newFormat) return;

        if(newFormat == AnswerFormat.SingleChoice && entity.Type == AnswerFormat.MultiChoice) {
            if(entity.Answers.Count(a => a.IsCorrect) > 1) {
                throw new InvalidOperationException("Single choice question must not have more than one correct answer");
            }
        }

        using IUnitOfWork uow = p_unitOfWorkFactory.Create();

        IRepository<UserAnswerEntity> userAnswerRepository = uow.GetRepository<UserAnswerEntity>();

        IEnumerable<UserAnswerEntity> userAnswers = userAnswerRepository.GetByPredicate(
            e => e.QuestionId == entity.Id, nameof(UserAnswerEntity.Question));

        foreach (UserAnswerEntity userAnswer in userAnswers) {
            userAnswerRepository.Delete(userAnswer.Id);
        }
    }

    private static SelectedAnswerEntity NewAnswer(Guid answerId, Guid userAnswerId) {
        return new SelectedAnswerEntity() {
            Id = Guid.NewGuid(),
            AnswerId = answerId,
            Order = 0,
            UserAnswerId = userAnswerId
        };
    }

    private static void CheckQuizTimeFrame(DateTime answerTime, AnswerEntity answerEntity) {
        QuizEntity quiz = answerEntity.Question!.Quiz!;

        // Answering in invalid time frame
        if (answerTime < quiz.StartTime || answerTime > quiz.EndTime) {
            throw new ArgumentException("Quiz is not opened");
        }
    }

    private static AnswerEntity GetAnswerWithQuestionAndQuiz(Guid answerId, IUnitOfWork uow) {
        IRepository<AnswerEntity> answerRepository = uow.GetRepository<AnswerEntity>();
        AnswerEntity? answerEntity = answerRepository.GetById(answerId,
            nameof(AnswerEntity.Question), $"{nameof(AnswerEntity.Question)}.{nameof(QuestionEntity.Quiz)}");

        return answerEntity is null ? throw new ArgumentException("Answer Id is not valid") : answerEntity;
    }

    private static UserAnswerEntity GetUserAnswer(Guid userId, DateTime answerTime, IUnitOfWork uow, Guid questionId) {
        IRepository<UserAnswerEntity> userAnswerRepository = uow.GetRepository<UserAnswerEntity>();

        IEnumerable<UserAnswerEntity>? userAnswerEntities = userAnswerRepository.GetByPredicate(
            e => e.QuestionId == questionId && e.UserId == userId,
            nameof(UserAnswerEntity.SelectedAnswers));

        UserAnswerEntity? userAnswerEntity = CheckForUserAnswerSingletonRecord(userAnswerEntities, userId);

        if (userAnswerEntity is not null) return userAnswerEntity;

        userAnswerEntity = new UserAnswerEntity() {
            AnswerTime = answerTime,
            Id = Guid.NewGuid(),
            QuestionId = questionId,
            UserId = userId,
            SelectedAnswers = new List<SelectedAnswerEntity>()
        };

        userAnswerRepository.Insert(userAnswerEntity);
        return userAnswerEntity;
    }


    private List<AnswerResultModel> MapAnswers(Guid userId, QuestionEntity questionEntity) {
        var answerModels = new List<AnswerResultModel>();
        IEnumerable<UserAnswerEntity> userAnswersForQuestion = questionEntity.UserAnswers;

        foreach (AnswerEntity answerEntity in questionEntity.Answers) {
            AnswerResultModel answerModel = p_mapper.Map<AnswerResultModel>(answerEntity);

            answerModel.IsAnswered = UserAnswered(userId, userAnswersForQuestion, answerEntity.Id);
            answerModel.SelectedOrder = GetSelectedOrder(userAnswersForQuestion, userId, answerEntity.Id);

            answerModels.Add(answerModel);
        }

        return answerModels;
    }

    private static bool UserAnswered(Guid userId, IEnumerable<UserAnswerEntity> currUserAnswers, Guid answerId) {
        UserAnswerEntity? userAnswer = CheckForUserAnswerSingletonRecord(currUserAnswers, userId);

        return userAnswer?.SelectedAnswers.Where(sa => sa.AnswerId == answerId).Any() ?? false;
    }

    private static UserAnswerEntity? CheckForUserAnswerSingletonRecord(IEnumerable<UserAnswerEntity> userAnswers, Guid userId) {
        UserAnswerEntity? answersForUser;

        try {
            answersForUser = userAnswers.SingleOrDefault(cu => cu.UserId == userId);
        } catch (InvalidOperationException e) {
            throw new ArgumentException("Database integrity is corrupted", e);
        }

        return answersForUser;
    }

    private static int GetSelectedOrder(IEnumerable<UserAnswerEntity> userAnswers, Guid userId, Guid answerId) {
        UserAnswerEntity? userAnswer = CheckForUserAnswerSingletonRecord(userAnswers, userId);

        if (userAnswer is null) return 0;

        try {
            SelectedAnswerEntity? answerOrder = userAnswer.SelectedAnswers.SingleOrDefault(sa => sa.AnswerId == answerId);
            return answerOrder?.Order ?? 0;
        } catch (InvalidOperationException e) {
            throw new ArgumentException("Database integrity is corrupted", e);
        }
    }
}