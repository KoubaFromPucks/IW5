using Microsoft.AspNetCore.Components;
using Quizzer.Common.Models;
using Quizzer.Web.BL;
using Quizzer.Web.BL.Facades;

namespace Quizzer.Web.App.Pages;
public partial class QuestionList {
    [Inject]
    private QuestionFacade QuestionFacade { get; set; } = null!;

    [Inject]
    private QuizFacade QuizFacade { get; set; } = null!;

    private IEnumerable<QuestionDetailModel>? _questions = null;

    private IEnumerable<QuizDetailModel>? _quizzes = null;

    private Guid SelectedQuizId { get; set; } = Guid.Empty;

    protected override async Task OnInitializedAsync() {
        await base.OnInitializedAsync();
        _questions = await QuestionFacade.GetAllAsync();
    }

    public async Task CreateQuestion() {
        if (SelectedQuizId == Guid.Empty) {
            ShowMessageBox("Quiz must be selected.", MessageBoxMode.Error);
            return;
        }

        NavManager.NavigateTo($"/QuestionDetail/{SelectedQuizId}/{Guid.Empty}/{LoggedUserId}");
    }

    public async Task LoadQuizzes() {
        try {
            _quizzes = (await QuizFacade.GetAllAsync()).Where(q => q.IsEditable); ;
        } catch (ApiException<string> exception) {
            ShowMessageBox(exception.Result, MessageBoxMode.Error);
        } catch (Exception exception) {
            ShowMessageBox(exception.Message, MessageBoxMode.Error);
        }
    }
}
