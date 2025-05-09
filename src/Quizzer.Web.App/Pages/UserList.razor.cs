using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Quizzer.Common.Models;
using Quizzer.Web.BL;
using Quizzer.Web.BL.Facades;

namespace Quizzer.Web.App.Pages;
public partial class UserList {
    [Inject]
    private UserFacade UserFacade { get; set; } = null!;
    private ICollection<UserDetailModel>? _users = null;

    protected override async Task OnInitializedAsync() {
        await base.OnInitializedAsync();
        await LoadUsers();
    }

    private async Task LoadUsers() {
        _users = await UserFacade.GetAllAsync();
    }

    public async Task DeleteUser(Guid userId) {
        bool confirmed = await JsRuntime.InvokeAsync<bool>("confirm", "Confirm user deletion.");
        if (!confirmed) return;

        if (userId == LoggedUserId) {
            await JsRuntime.InvokeVoidAsync("alert", "Can't delete active user!");
            return;
        }

        try {
            await UserFacade.DeleteAsync(userId);
        } catch (ApiException<string> exception) {
            await JsRuntime.InvokeVoidAsync("showMessageBox", exception.Result, false);
        }

        await LoadUsers();
    }
}
