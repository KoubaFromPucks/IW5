using Microsoft.AspNetCore.Components;
using Quizzer.Common.Models;

namespace Quizzer.Web.App.Components;
public partial class AnswerEditForm {
    [Parameter]
    public EventCallback OnDone { get; set; }

    [Parameter]
    public EventCallback OnSave { get; set; }

    [Parameter]
    public EventCallback OnInvalid { get; set; }

    [Parameter]
    public AnswerDetailModel AnswerDetailModel { get; set; }

    private string GetDisableClass() => AnswerDetailModel.IsQuizEditable ? "" : "disabled";
}
