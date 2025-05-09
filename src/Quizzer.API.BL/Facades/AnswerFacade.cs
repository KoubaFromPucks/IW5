using AutoMapper;
using Quizzer.API.DAL.Entities;
using Quizzer.API.DAL.Enums;
using Quizzer.API.DAL.Repository.Interfaces;
using Quizzer.API.DAL.UnitOfWork;
using Quizzer.Common.Models;
using Quizzer.Extensions;

namespace Quizzer.API.BL.Facades; 
public class AnswerFacade : FacadeBase<AnswerEntity, AnswerDetailModel> {
    private IQuizChecker _quizChecker;
    public AnswerFacade(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper, IQuizChecker quizChecker) : base(unitOfWorkFactory, mapper) {
        _quizChecker = quizChecker;
    }

    public ICollection<AnswerDetailModel> GetAnswersForQuestion(Guid questionId) {
        ICollection <AnswerDetailModel> answers = GetListByPredicate<AnswerDetailModel>(a => a.QuestionId == questionId);

        SetQuizEditable(answers);

        return answers;
    }

    public override AnswerDetailModel? GetDetailById(Guid id) {
        AnswerDetailModel? model = base.GetDetailById(id);

        if (model is null) {
            throw new ArgumentException("Answer does not exist!");
        }

        model.IsQuizEditable = IsQuizEditable(model.QuestionId);

        return model;
    }

    public override ICollection<AnswerDetailModel> GetAll() => GetAllAnswers();
    
    public ICollection<AnswerDetailModel> GetAllAnswers() {
        using IUnitOfWork uow = p_unitOfWorkFactory.Create();

        IRepository<AnswerEntity> repository = uow.GetRepository<AnswerEntity>();

        IEnumerable<AnswerEntity> entities = repository.GetAll();

        ICollection<AnswerDetailModel> models = entities.Select(p_mapper.Map<AnswerDetailModel>).ToList();

        SetQuizEditable(models);

        return models;
    }

    public ICollection<AnswerDetailModel> GetAnswerByText(string text, bool exact = false) {
        var textTrim = text.TrimWhiteAndUnprintableChars();

        if (exact) return GetListByPredicate<AnswerDetailModel>(e => e.Text == textTrim);

        string[] splitWords = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        var resultModels = new List<AnswerDetailModel>();
        bool firstIteration = true;
        foreach (string word in splitWords) {
            var lowerWord = word.ToLower();
            ICollection<AnswerDetailModel> currentModels = GetListByPredicate<AnswerDetailModel>(entity =>
            entity.Text.ToLower().Contains(word.ToLower()));

            if (firstIteration) {
                firstIteration = false;
                resultModels = currentModels.ToList();
            } else {
                resultModels = resultModels.IntersectBy(currentModels.Select(m => m.Id), model => model.Id).ToList();
            }
        }

        SetQuizEditable(resultModels);

        return resultModels;
    }

    public Guid Save(AnswerDetailModel model, Guid questionId) {
        CheckIfQuizEditable(questionId);
        if (model.Order < 0) throw new ArgumentException("Answer order must be positive");

        model.Text = model.Text?.TrimWhiteAndUnprintableChars() ?? string.Empty;
        model.PictureUrl = model.PictureUrl?.TrimWhiteAndUnprintableChars();

        if (string.IsNullOrEmpty(model.Text) && string.IsNullOrEmpty(model.PictureUrl)) {
            throw new ArgumentException("Answer must not be empty");
        }

        using IUnitOfWork uow = p_unitOfWorkFactory.Create();
        IRepository<AnswerEntity> answerRepository = uow.GetRepository<AnswerEntity>();
        IRepository<QuestionEntity> questionRepository = uow.GetRepository<QuestionEntity>();

        QuestionEntity? question = questionRepository.GetById(questionId, nameof(QuestionEntity.Quiz), nameof(QuestionEntity.Answers)) ??
            throw new ArgumentException("Question does not exist");

        if (question.Answers.Any(a => a.Order == model.Order) && question.Type == AnswerFormat.OrderChoice) {
            throw new ArgumentException("Order of answer must be unique");
        }

        if(question.Answers.Any(a => a.IsCorrect && model.IsCorrect) && question.Type == AnswerFormat.SingleChoice) {
            throw new ArgumentException("Single choice question has already a correct answer");
        }

        AnswerEntity? answerEntity = answerRepository.GetById(model.Id);
        if (answerEntity is null) {
            AnswerEntity newEntity = p_mapper.Map<AnswerEntity>(model);
            newEntity.QuestionId = questionId;
            return Save(newEntity);
        }

        return Save(model);
    }

    public void Delete(Guid id) {
        AnswerEntity answerEntity = GetAnswerWithQuiz(id);

        CheckIfQuizEditable(answerEntity.QuestionId);

        Delete(id);
    }

    public void MarkAnswer(Guid answerId, bool isCorrect, DateTime time) {
        using IUnitOfWork uow = p_unitOfWorkFactory.Create();

        IRepository<AnswerEntity> answerRepository = uow.GetRepository<AnswerEntity>();

        AnswerEntity answerEntity = GetAnswerWithQuiz(answerId) ??
            throw new ArgumentException("Answer does not exist");

        CheckIfQuizEditable(answerEntity.QuestionId);

        answerEntity.IsCorrect = isCorrect;
        answerRepository.Update(answerEntity);

        if (answerEntity.Question is null) {
            uow.Commit();
            return;
        }

        IEnumerable<AnswerEntity> otherAnswers = answerRepository.GetByPredicate(
            a => a.QuestionId == answerEntity.QuestionId) ??
            Enumerable.Empty<AnswerEntity>();

        if (isCorrect) HandleSingleChoice();

        uow.Commit();
        return;

        void HandleSingleChoice() {
            if (answerEntity.Question.Type == AnswerFormat.SingleChoice) {
                foreach (AnswerEntity answers in otherAnswers) {
                    answers.IsCorrect = false;
                    answerRepository.Update(answers);
                }
            }
        }
    }

    private AnswerEntity GetAnswerWithQuiz(Guid id) {
        using IUnitOfWork uow = p_unitOfWorkFactory.Create();

        IRepository<AnswerEntity> answerRepository = uow.GetRepository<AnswerEntity>();

        AnswerEntity answerEntity = answerRepository.GetById(id,
            nameof(AnswerEntity.Question), $"{nameof(AnswerEntity.Question)}.{nameof(QuestionEntity.Quiz)}") ??
            throw new ArgumentException("Answer does not exist");

        return answerEntity;
    }

    private bool IsQuizEditable(Guid questionId) {
        IUnitOfWork uow = p_unitOfWorkFactory.Create();
        IRepository<QuestionEntity> questionRepository = uow.GetRepository<QuestionEntity>();

        QuestionEntity? questionEntity = questionRepository.GetById(questionId);
        if (questionEntity is null) {
            throw new ArgumentException("Given question entity does not exists.");
        }

        return _quizChecker.IsQuizEditable(questionEntity.QuizId);
    }

    private void CheckIfQuizEditable(Guid questionId) {
        if (!IsQuizEditable(questionId)) {
            throw new ArgumentException("The Answer can no longer be edited.");
        }
    }

    private void SetQuizEditable(ICollection<AnswerDetailModel> answers) {
        foreach (AnswerDetailModel answer in answers) {
            answer.IsQuizEditable = IsQuizEditable(answer.QuestionId);
        }
    }
}
