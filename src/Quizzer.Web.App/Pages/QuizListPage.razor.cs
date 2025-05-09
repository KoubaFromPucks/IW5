
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Quizzer.Common.Models;
using Quizzer.Web.App.Components;
using Quizzer.Web.BL;
using Quizzer.Web.BL.Facades;

namespace Quizzer.Web.App.Pages;

public partial class QuizListPage {
#nullable disable
    [Inject]
    public QuizFacade QuizFacade { get; set; }

    public ICollection<QuizDetailModel> Quizes { get; set; } = null;

    private string ModalUrl { get; set; } = "";

    private ModalWindow DetailWarningWindow { get; set; }

#nullable enable
    protected override async Task OnParametersSetAsync() {
        Quizes = await QuizFacade.GetAllForUserAsync(LoggedUserId);

        await base.OnInitializedAsync();
    }

    public async Task DeleteQuiz(QuizDetailModel quiz) {
        bool confirmed = await JsRuntime.InvokeAsync<bool>("confirm", "Confirm quiz deletion.");
        if (!confirmed) return;

        try {
            await QuizFacade.DeleteAsync(quiz.Id);
            Quizes.Remove(quiz);
        } catch (ApiException<string> exception) {
            ShowMessageBox(exception.Result, MessageBoxMode.Error);
        } catch (Exception exception) {
            ShowMessageBox(exception.Message, MessageBoxMode.Error);
        }
    }

    private void ShowModalOrNavigate(QuizDetailModel quiz, string navigatePath) {
        ModalUrl = navigatePath;

        if (quiz.IsPlayable.GetValueOrDefault(false)) {
            DetailWarningWindow.ShowModal();
        } else {
            NavManager.NavigateTo(navigatePath);
        }
    }
}
