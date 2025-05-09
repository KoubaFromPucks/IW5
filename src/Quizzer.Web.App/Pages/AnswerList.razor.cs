using Microsoft.AspNetCore.Components;
using Quizzer.Common.Models;
using Quizzer.Web.BL;
using Quizzer.Web.BL.Facades;

namespace Quizzer.Web.App.Pages;
public partial class AnswerList {
    [Inject]
    private AnswerFacade AnswerFacade { get; set; } = null!;

    private IEnumerable<AnswerDetailModel>? _answers = null;

    [Inject]
    private QuestionFacade QuestionFacade { get; set; } = null!;

    private IEnumerable<QuestionDetailModel>? _questions = null;

    private Guid SelectedQuestionId { get; set; } = Guid.Empty;

    protected override async Task OnInitializedAsync() {
        await base.OnInitializedAsync();
        _answers = await AnswerFacade.GetAllAnswersAsync();
    }

    public async Task LoadQuestions() {
        try {
            _questions = (await QuestionFacade.GetAllAsync()).Where(q => q.IsQuizEditable);
        } catch (ApiException<string> exception) {
            ShowMessageBox(exception.Result, MessageBoxMode.Error);
        } catch (Exception exception) {
            ShowMessageBox(exception.Message, MessageBoxMode.Error);
        }
    }

    public async Task CreateAnswer() {
        if (SelectedQuestionId == Guid.Empty) {
            ShowMessageBox(
                $"question must be selected",
                MessageBoxMode.Error);
            return;
        }

        NavManager.NavigateTo($"/answerDetail/{SelectedQuestionId}/{Guid.Empty}/{LoggedUserId}");
    }
}
