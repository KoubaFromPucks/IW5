using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Quizzer.Common.Models;
using Quizzer.Web.BL.Facades;

namespace Quizzer.Web.App.Pages; 
public class PageBase : ComponentBase {

    [Parameter]
    public Guid LoggedUserId { get; set; }

    [Inject]
    private UserFacade UserFacade { get; set; } = null!;
    public UserDetailModel? LoggedUser=null;

    protected string _messageboxClass = "";
    protected string _messageBoxText = "";
    protected bool _messageBoxVisible = false;

    protected enum MessageBoxMode { Error, Success, Warning}
    protected override async Task OnInitializedAsync() {
        try {
            LoggedUser = await UserFacade.GetByIdAsync(LoggedUserId); //TODO error handling
        } catch {
            LoggedUser = null;
        }

        await base.OnInitializedAsync();
    }

    protected void ShowMessageBox(string errorMessage, MessageBoxMode mode) {
        _messageBoxText = errorMessage;
        if (mode == MessageBoxMode.Success) {
            _messageboxClass = "alert-success";
        } else if (mode == MessageBoxMode.Error) {
            _messageboxClass = "alert-danger";
        } else if (mode == MessageBoxMode.Warning) {
            _messageboxClass = "alert-warning";
        }

        _messageBoxVisible = true;
        StateHasChanged();
    }

    protected void HideMessageBox() {
        _messageBoxVisible = false;
        StateHasChanged();
    }
}
