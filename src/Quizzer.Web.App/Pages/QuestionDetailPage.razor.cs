using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Quizzer.Common.Models;
using Quizzer.Web.BL;
using Quizzer.Web.BL.Facades;

namespace Quizzer.Web.App.Pages;

public partial class QuestionDetailPage {
    [Inject]
    private QuestionFacade QuestionFacade { get; set; } = null!;

    [Inject]
    private AnswerFacade AnswerFacade { get; set; } = null!;

    [Inject]
    private QuizFacade QuizFacade { get; set; } = null!;

    [Parameter]
    public Guid QuestionId { get; set; } = Guid.Empty;

    [Parameter]
    public Guid QuizId { get; set; } = Guid.Empty;

    private QuizDetailModel? Quiz { get; set; } = null;

    public QuestionDetailModel Question { get; set; } 
        = new() { Id = Guid.Empty, Text = "question text"};


    protected override async Task OnInitializedAsync() {
        if (QuestionId != Guid.Empty) {
            await LoadData();
        } else {
            try {
                Quiz = await QuizFacade.GetByIdAsync(QuizId);

                Question.QuizId = QuizId;
                Question.IsQuizEditable = Question.IsQuizEditable;

            } catch {
                ShowMessageBox("Could not load the data!", MessageBoxMode.Error);
            }
        }

        if (Question.Id == Guid.Empty) {
            Question.IsQuizEditable = Quiz?.IsEditable ?? false;
        }

        await base.OnInitializedAsync();
        StateHasChanged();
    }

    private async Task LoadData() {
        try {
            Question = await QuestionFacade.GetByIdAsync(QuestionId);

            if (!Question.IsQuizEditable) {
                ShowMessageBox("Question can not be edited", MessageBoxMode.Warning);
            }
        } catch {
            ShowMessageBox("Could not load the data!", MessageBoxMode.Error);
        }
    }

    public async Task Update() {
        HideMessageBox();

        if (!Question.IsQuizEditable) {
            ShowMessageBox("Answer is not editable", MessageBoxMode.Error);
            return;
        }

        try {
            if (QuestionId == Guid.Empty) {
                QuestionId = await QuestionFacade.CreateAsync(QuizId, Question);
                await LoadData();
            } else {
                await QuestionFacade.UpdateAsync(Question.QuizId, Question);
            }

            ShowMessageBox("Question succesfully updated.", MessageBoxMode.Success);
        } catch (ApiException<string> exception) {
            ShowMessageBox(exception.Result, MessageBoxMode.Error);
        } catch (Exception exception){
            ShowMessageBox(exception.Message, MessageBoxMode.Error);
        }
    }

    public async Task Delete() {
        if (!Question.IsQuizEditable) {
            ShowMessageBox("Answer is not editable", MessageBoxMode.Error);
            return;
        }

        HideMessageBox();

        try {
            await QuestionFacade.DeleteAsync(QuestionId);
        } catch (ApiException<string> e) {
            ShowMessageBox(e.Result, MessageBoxMode.Error);

            ShowMessageBox($"Could delete question! {e.Result}", MessageBoxMode.Error);
        } catch (Exception e) {
            ShowMessageBox($"Could delete question! {e.Message}", MessageBoxMode.Error);
        }
    }

    public async Task Done() {
        NavManager.NavigateTo($"/quizEdit/{(Question.Id == Guid.Empty ? QuizId : Question.QuizId)}/{LoggedUserId}");
    }

    public async Task DeleteAnswer(Guid answerId) {
        if (!Question.IsQuizEditable) {
            ShowMessageBox("Question is no longer editable.", MessageBoxMode.Error);
            return;
        }

        bool confirmed = await JsRuntime.InvokeAsync<bool>("confirm", "Confirm answer deletion.");
        if (!confirmed) return;

        try {
            await AnswerFacade.DeleteAsync(answerId);
            await LoadData();
        } catch (ApiException<string> exception) {
            ShowMessageBox(exception.Result, MessageBoxMode.Error);
        } catch (Exception exception) {
            ShowMessageBox(exception.Message, MessageBoxMode.Error);
        }
    }

    private string GetDisableClass() => Question.IsQuizEditable ? "" : "disabled";
    
}
