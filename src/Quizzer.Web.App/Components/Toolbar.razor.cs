using Microsoft.AspNetCore.Components;
using Quizzer.Common.Models;

namespace Quizzer.Web.App.Components;
public partial class Toolbar {
    [Parameter]
    public UserDetailModel? LoggedUser { get; set; } = null;
}
