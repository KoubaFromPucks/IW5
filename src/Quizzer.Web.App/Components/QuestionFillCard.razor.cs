using Microsoft.AspNetCore.Components;
using Quizzer.Common.Models;
using Quizzer.API.DAL.Enums;
namespace Quizzer.Web.App.Components;

public partial class QuestionFillCard {
    [Parameter]
    public required QuestionDetailModel QuestionDetailModel { get; set; }

    [Parameter]
    public required bool ShowCard { get; set; }

    private Guid SelectedSinglePrivate { get; set; }

    private int[] AnswerOrder { get; set; } = new int[0];

    public Guid SelectedSingle { 
        get { return SelectedSinglePrivate; } 
        set { SelectSingleAnswer(value); } 
    }

    protected override async Task OnInitializedAsync() {
        if (QuestionDetailModel.QuestionType == AnswerFormat.SingleChoice) {
            MarkSelectedAnswer();
        }

        var rnd = new Random();
        AnswerOrder = Enumerable.Range(0, QuestionDetailModel.Answers.Count).OrderBy(_ => rnd.Next()).ToArray();
    }

    private void MarkSelectedAnswer() {
        bool selected = false;
        foreach (AnswerDetailModel answer in QuestionDetailModel.Answers) {
            if (!selected && answer.IsUserSelected) {
                SelectedSinglePrivate = answer.Id;
                selected = true;
                continue;
            }

            answer.IsUserSelected = false;
        }
    }

    private void SelectSingleAnswer(Guid newSelected) {
        SelectedSinglePrivate = newSelected;

        foreach (AnswerDetailModel answer in QuestionDetailModel.Answers)
            answer.IsUserSelected = answer.Id == newSelected;
    }
}
