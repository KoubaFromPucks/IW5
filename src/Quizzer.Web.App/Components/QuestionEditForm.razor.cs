using Microsoft.AspNetCore.Components;
using Quizzer.Common.Models;

namespace Quizzer.Web.App.Components;
public partial class QuestionEditForm {
    [Parameter]
    public QuestionDetailModel Question { get; set; }
    
    [Parameter]
    public EventCallback OnSave { get; set; }
    
    [Parameter]
    public EventCallback OnCancel { get; set; }

    [Parameter]
    public EventCallback OnDelete { get; set; }
    [Parameter]
    public EventCallback OnInvalid { get; set; }

    [Parameter]
    public EventCallback OnDeleteAnswer { get; set; }

    [Parameter]
    public EventCallback OnDone { get; set; }

    private string GetDisableClass() => Question.IsQuizEditable ? "" : "disabled";
}
