using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Quizzer.Common.Models;
using Quizzer.Web.BL;
using Quizzer.Web.BL.Facades;

namespace Quizzer.Web.App.Pages;

public partial class UserEdit {
#nullable disable

    [Parameter]
    public Guid UserId { get; set; }

    [Inject]
    public UserFacade UserFacade { get; set; }

    [Inject]
    public QuizFacade QuizFacade { get; set; }

    public UserDetailModel CurrentUser { get; set; }


#nullable enable
    protected override async Task OnInitializedAsync() {
        if (UserId == Guid.Empty) {
            CurrentUser = new UserDetailModel { 
                Id = Guid.Empty, 
                Name = "name", 
                ProfilePictureUrl = null };
        } else {
            CurrentUser = await UserFacade.GetByIdAsync(UserId);
        }
        await base.OnInitializedAsync();
    }

    public async Task Submit() {
        HideMessageBox();
        try {
            if (UserId == Guid.Empty) {
                UserId = await UserFacade.CreateAsync(CurrentUser);
                CurrentUser = await UserFacade.GetByIdAsync(UserId);
            } else {
                await UserFacade.UpdateAsync(CurrentUser);
            }
            ShowMessageBox("Saved succesfully", MessageBoxMode.Success);
        } catch (ApiException<string> exception) {
            ShowMessageBox(exception.Result, MessageBoxMode.Error);
        } catch (Exception exception) {
            ShowMessageBox(exception.Message, MessageBoxMode.Error);
        }
    }

    public async Task Done() {
        NavManager.NavigateTo($"/users/{LoggedUserId}");
    }

}
