using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Quizzer.Common.Models;
using Quizzer.Web.BL;
using Quizzer.Web.BL.Facades;

namespace Quizzer.Web.App.Pages;
public partial class QuizEdit {

    [Parameter]
    public Guid Id { get; set; }

    public QuizDetailModel? Quiz;

    [Inject]
    private QuizFacade QuizFacade { get; set; } = null!;
    [Inject]
    private QuestionFacade QuestionFacade { get; set; } = null!;

    protected override async Task OnInitializedAsync() {
        if (Id == Guid.Empty) {
            Quiz = new QuizDetailModel {
                StartTime = DateTime.Now.AddSeconds(60),
                EndTime = DateTime.Now.AddDays(1),
                Id = Guid.Empty,
                Name = "name",
                Description = null,
                IsEditable = true
            };
        } else {
            await LoadData();
        }
        await base.OnInitializedAsync();

        if (!Quiz.IsEditable) {
            ShowMessageBox("Quiz is not editable", MessageBoxMode.Warning);
            return;
        }
    }

    private async Task LoadData() {
        try {
            Quiz = await QuizFacade.GetByIdAsync(Id);
        } catch {
            ShowMessageBox("Could not load the data!", MessageBoxMode.Error);
        }
    }

    public async Task Save() {
        HideMessageBox();

        if (!Quiz.IsEditable) {
            ShowMessageBox("Quiz is no longer editable", MessageBoxMode.Error);
            return;
        }

        try {
            if (Id == Guid.Empty) {
                Id = await QuizFacade.CreateAsync(Quiz);
                Quiz = await QuizFacade.GetByIdAsync(Id);

            } else {
                await QuizFacade.UpdateAsync(Quiz);
            }
            ShowMessageBox("Saved succesfully", MessageBoxMode.Success);
        } catch (ApiException<string> exception) {
            ShowMessageBox(exception.Result, MessageBoxMode.Error);
        } catch (Exception exception) {
            ShowMessageBox(exception.Message, MessageBoxMode.Error);
        }
    }

    public async Task RemoveQuestion(Guid id) {
        if (!Quiz!.IsEditable) {
            ShowMessageBox("Quiz is no longer editable", MessageBoxMode.Error);
            return;
        }

        bool confirmed = await JsRuntime.InvokeAsync<bool>("confirm", "Confirm question deletion.");
        if (!confirmed) return;


        HideMessageBox();

        try {
            await QuestionFacade.DeleteAsync(id);
            await LoadData();
        } catch (ApiException<string> exception) {
            ShowMessageBox(exception.Result, MessageBoxMode.Error);
        } catch (Exception exception) {
            ShowMessageBox(exception.Message, MessageBoxMode.Error);
        }
    }

    public async Task Done() {
        NavManager.NavigateTo($"/QuizListPage/{LoggedUserId}");
    }

    private string GetDisableClass() => Quiz!.IsEditable ? "" : "disabled";
}
