using Microsoft.AspNetCore.Components;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop;
using Quizzer.Common.Models;
using Quizzer.API.DAL.Enums;
using Quizzer.Web.BL.Facades;


namespace Quizzer.Web.App.Pages;

public partial class QuizPlayingPage : IAsyncDisposable {
    [Inject]
    private QuizFacade QuizFacade { get; set; } = null!;

    [Inject]
    private QuestionFacade QuestionFacade { get; set; } = null!;

    [Inject]
    private AnswerFacade AnswerFacade { get; set; } = null!;

    [Parameter]
    public Guid QuizId { get; set; }

    private QuizDetailModel? QuizDetailModel { get; set; }

    private ICollection<QuestionResultModel> _answeredQuestions = new List<QuestionResultModel>();

    private int CurrentQuestionIndex { get; set; } = 0;

    private bool QuizCompleted { get; set; } = false;

    private string _errorMsg = String.Empty;

    private IJSObjectReference? _pageJsModule;

    private bool _loadingData = true;
    private bool CanGoToNextQuestion {
        get { return CurrentQuestionIndex + 1 < QuizDetailModel?.Questions.Count; }
    }

    private bool CanGoToPreviousQuestion {
        get { return CurrentQuestionIndex > 0; }
    }

    protected override async Task OnInitializedAsync() {
        await base.OnInitializedAsync();
        _pageJsModule = await JsRuntime.InvokeAsync<IJSObjectReference>("import", "./js/quizPlayingPage.js");

        if (await IsQuizClosed()) {
            ShowMessageBox("Quiz was already completed!", MessageBoxMode.Error);
            _loadingData = false;
            return;
        }

        try {
            QuizFacade?.StartQuizAsync(QuizId, LoggedUserId);
        } finally { }

        await LoadData();

        if (DateTime.Now < QuizDetailModel?.StartTime || DateTime.Now > QuizDetailModel?.EndTime) {
            ShowMessageBox("Quiz is not running!", MessageBoxMode.Error);
            QuizDetailModel = null;
        }

        _loadingData = false;
    }

    async ValueTask IAsyncDisposable.DisposeAsync() {
        if (_pageJsModule is not null) {
            await _pageJsModule.DisposeAsync();
        }
    }

    private async Task<bool> IsQuizClosed() {
        try {
            await QuizFacade.GetQuizScoreAsync(QuizId, LoggedUserId);
        } catch {
            return false;
        }

        QuizCompleted = true;
        return true;
    }

    private async Task LoadData() {
        try {
            QuizDetailModel = await QuizFacade.GetByIdAsync(QuizId);
            _answeredQuestions = await QuestionFacade.GetResultsForUserAsync(QuizId, LoggedUserId);
        } catch (Exception e) {
            _errorMsg = e.Message;
        }

        if (QuizDetailModel is not null) {
            try {
                foreach (QuestionDetailModel question in QuizDetailModel.Questions) {
                    question.Answers = new List<AnswerDetailModel>(await AnswerFacade.GetAnswersForQuestionAsync(question.Id));
                }
            } catch (Exception e) {
                _errorMsg = _errorMsg.IsNullOrEmpty() ? e.Message : _errorMsg + "\n" + e.Message;
            }
        }

        if (!_errorMsg.IsNullOrEmpty()) {
            ShowMessageBox(_errorMsg, MessageBoxMode.Error);
            QuizDetailModel = null;
        }

        FillAnsweredQuestions();
    }

    private void FillAnsweredQuestions() {
        foreach (QuestionResultModel questionResultModel in _answeredQuestions) {
            QuestionDetailModel? questionDetailModel = QuizDetailModel
                ?.Questions
                .FirstOrDefault(q => q?.Id == questionResultModel.Id, null);

            if (questionDetailModel == null) {
                continue;
            }

            FillQuestionAnswers(questionResultModel, questionDetailModel);
        }
    }

    private void FillQuestionAnswers(QuestionResultModel src, QuestionDetailModel dst) {
        foreach (AnswerResultModel answerResultModel in src.Answers) {
            AnswerDetailModel? answerDetailModel = dst.Answers.FirstOrDefault(a => a?.Id == answerResultModel.Id, null);

            if (answerDetailModel is null) {
                continue;
            }

            if (dst.QuestionType == AnswerFormat.OrderChoice) {
                answerDetailModel.Order = answerResultModel?.SelectedOrder ?? 0;
            } else {
                answerDetailModel.IsUserSelected = answerResultModel.IsAnswered;
            }
        }
    }

    private async Task SaveSingleQuestion(QuestionDetailModel question) {
        Guid newSelected = question.Answers.FirstOrDefault(a => a.IsUserSelected, null)?.Id ?? Guid.Empty;

        if (newSelected == Guid.Empty) {
            return;
        }

        await QuestionFacade.AnswerQuestionAsync(newSelected, LoggedUserId, 0);
    }

    private async Task SaveMultiQuestionAnswer(AnswerResultModel oldAnswer, AnswerDetailModel currentAnswer) {
        bool wasSelected = oldAnswer?.IsAnswered ?? false;
        if (wasSelected ^ currentAnswer.IsUserSelected) {
            try {
                await QuestionFacade.AnswerQuestionAsync(currentAnswer.Id, LoggedUserId, 0);
                oldAnswer.IsAnswered = !oldAnswer.IsAnswered;
            } catch {
                ShowMessageBox("It was not possible to save the answers.", MessageBoxMode.Error);
            }
        }
    }

    private async Task SaveOrderQuestionAnswer(AnswerResultModel oldAnswer, AnswerDetailModel newAnswer) {
        bool canSaveOrder = (newAnswer.Order > 0 || oldAnswer.SelectedOrder != 0);
        bool orderHasChanged = oldAnswer.SelectedOrder != newAnswer.Order;

        if (canSaveOrder && orderHasChanged) {
            await QuestionFacade.AnswerQuestionAsync(newAnswer.Id, LoggedUserId, newAnswer.Order);
            oldAnswer.SelectedOrder = newAnswer.Order;
        }
    }

    private async Task SaveCurrentQuestion() {
        QuestionDetailModel currentQuestion = QuizDetailModel!.Questions[CurrentQuestionIndex];

        QuestionResultModel? questionResultModel = _answeredQuestions
            .SingleOrDefault(q => q?.Id == currentQuestion.Id, null);


        if (currentQuestion.QuestionType == AnswerFormat.SingleChoice) {
            await SaveSingleQuestion(currentQuestion);
            return;
        }

        foreach (AnswerDetailModel currentAnswer in QuizDetailModel.Questions[CurrentQuestionIndex].Answers) {
            AnswerResultModel oldAnswer = questionResultModel!.Answers.First(a => a.Id == currentAnswer.Id);


            if (currentQuestion.QuestionType == AnswerFormat.OrderChoice) {
                await SaveOrderQuestionAnswer(oldAnswer, currentAnswer);
            } else if (currentQuestion.QuestionType == AnswerFormat.MultiChoice) {
                await SaveMultiQuestionAnswer(oldAnswer, currentAnswer);
            }
        }
    }

    private async Task<bool> IsCurrentOrderQuestionOk() {
        bool ok = true;

        foreach (AnswerDetailModel answer in QuizDetailModel!.Questions[CurrentQuestionIndex].Answers) {
            string msg = "";
            if (answer.Order < 0) {
                msg = "<br>The order of the answer must be a positive value. To ignore order, set the value to 0.";
                ok = false;
            }

            bool unique = QuizDetailModel
                .Questions[CurrentQuestionIndex]
                .Answers
                .Where(a => a.Order == answer.Order)
                .Count() > 1;

            if (answer.Order != 0 && unique) {
                msg += "<br>The order of the answer must be unique.";
                ok = false;
            }

            await _pageJsModule!.InvokeVoidAsync("setTextById", answer.Id.ToString(), msg);
        }
        return ok;
    }

    private async Task NextQuestion() {
        if (QuizDetailModel!.Questions[CurrentQuestionIndex].QuestionType == AnswerFormat.OrderChoice
            && !await IsCurrentOrderQuestionOk()) {

            ShowMessageBox("There were some errors. Check your answers.", MessageBoxMode.Error);
            return;
        }
        HideMessageBox();

        await SaveCurrentQuestion();

        if (!CanGoToNextQuestion) {
            try {
                await QuizFacade.CompleteQuizAsync(QuizId, LoggedUserId);
                ShowMessageBox("Quiz was Succesfully completed", MessageBoxMode.Success);
                QuizDetailModel = null;
                _answeredQuestions = new List<QuestionResultModel>();
            } catch {
                ShowMessageBox("Could not complete the Quiz. Maybe some internal error.", MessageBoxMode.Error);
            }
            return;
        }

        await _pageJsModule!.InvokeVoidAsync("hideById", QuizDetailModel?.Questions[CurrentQuestionIndex].Id);
        await _pageJsModule!.InvokeVoidAsync("showById", QuizDetailModel?.Questions[++CurrentQuestionIndex].Id);
    }

    private async Task PreviousQuestion() {
        await SaveCurrentQuestion();

        CurrentQuestionIndex = CurrentQuestionIndex <= 0 ? 0 : CurrentQuestionIndex - 1;

        await _pageJsModule!.InvokeVoidAsync("hideById", QuizDetailModel?.Questions[CurrentQuestionIndex + 1].Id);
        await _pageJsModule!.InvokeVoidAsync("showById", QuizDetailModel?.Questions[CurrentQuestionIndex].Id);
    }

}