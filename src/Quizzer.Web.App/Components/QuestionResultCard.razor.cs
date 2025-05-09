using Microsoft.AspNetCore.Components;
using Quizzer.Common.Models;

namespace Quizzer.Web.App.Components;

public partial class QuestionResultCard {
    [Parameter]
    public required QuestionResultModel QuestionResultModel { get; set; }

    private string GetYesOrNo(bool condition) => condition ? "Yes" : "No";

    private string GetOkOrNokClasses(bool condition) => (condition ? "text-success" : "text-danger") + " fw-bold";
}
