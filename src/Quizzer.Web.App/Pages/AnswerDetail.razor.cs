using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Quizzer.Common.Models;
using Quizzer.Web.BL;
using Quizzer.Web.BL.Facades;

namespace Quizzer.Web.App.Pages;

public partial class AnswerDetail {
#nullable disable

    [Parameter]
    public Guid AnswerId { get; set; }

    [Parameter]
    public Guid QuestionId { get; set; }

    [Inject]
    public AnswerFacade AnswerFacade { get; set; }

    [Inject]
    public QuestionFacade QuestionFacade { get; set; }

    public AnswerDetailModel Answer { get; set; } = null;

    private QuestionDetailModel? Question { get; set; } = null;

#nullable enable
    protected override async Task OnInitializedAsync() {
        await base.OnInitializedAsync();
        await LoadData();

        if (!Answer.IsQuizEditable) {
            ShowMessageBox("Answer can not be edited.", MessageBoxMode.Warning);
        }
    }

    private async Task LoadData() {
        try {
            Question = await QuestionFacade.GetByIdAsync(QuestionId);

            if (AnswerId == Guid.Empty) {
                Answer = new AnswerDetailModel {
                    Id = Guid.Empty,
                    QuestionId = QuestionId,
                    Text = "answer text",
                    IsCorrect = false,
                };

                Answer.IsQuizEditable = Question.IsQuizEditable;
            } else {
                Answer = await AnswerFacade.GetByIdAsync(AnswerId);
            }
            
            Answer.Type = Question.QuestionType;
        } catch {
            ShowMessageBox("Could not load the data.", MessageBoxMode.Error);
        }
    } 

    public async Task Submit() {
        HideMessageBox();

        if (!Answer.IsQuizEditable) {
            ShowMessageBox("Answer is not editable", MessageBoxMode.Error);
            return;
        }

        try {
            if (AnswerId == Guid.Empty) {
                AnswerId = await AnswerFacade.CreateAsync(Answer, QuestionId);
            } else {
                await AnswerFacade.UpdateAsync(Answer, QuestionId);
            }
            ShowMessageBox("Saved succesfully", MessageBoxMode.Success);
        } catch (ApiException<string> exception) {
            ShowMessageBox(exception.Result, MessageBoxMode.Error);
        } catch (Exception exception) {
            ShowMessageBox(exception.Message, MessageBoxMode.Error);
        }
    }

    public async Task Done() {
        NavManager.NavigateTo($"/QuestionDetail/{Guid.Empty}/{QuestionId}/{LoggedUserId}");
    }

}
