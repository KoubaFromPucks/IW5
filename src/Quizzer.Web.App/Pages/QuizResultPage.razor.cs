using Microsoft.AspNetCore.Components;
using Quizzer.Common.Models;
using Quizzer.Web.BL.Facades;

namespace Quizzer.Web.App.Pages;
public partial class QuizResultPage {
    [Inject]
    private QuizFacade QuizFacade { get; set; } = null!;

    [Inject]
    private QuestionFacade QuestionFacade { get; set; } = null!;

    [Parameter]
    public Guid QuizId { get; set; }

    [Parameter]
    public Guid UserId { get; set; }

    private QuizResultDetailModel? QuizResultDetailModel { get; set; }

    private ICollection<QuestionResultModel> QuestionResultModels { get; set; } = new List<QuestionResultModel>();

    protected override async Task OnInitializedAsync() {
        await LoadData();

        await base.OnInitializedAsync();
    }

    private async Task LoadData() {
        try {
            QuizResultDetailModel = await QuizFacade.GetResultAsync(QuizId, UserId);
        } catch (Exception e) {
            ShowMessageBox(e.Message, MessageBoxMode.Error);
            return;
        }

        try {
            QuestionResultModels = await QuestionFacade.GetResultsForUserAsync(QuizId, UserId);
        } catch (Exception e) {
            ShowMessageBox(e.Message, MessageBoxMode.Error);
        }
    }
}