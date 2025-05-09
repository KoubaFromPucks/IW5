using Microsoft.AspNetCore.Components;
using Quizzer.Common.Models;
using Quizzer.Web.BL.Facades;

namespace Quizzer.Web.App.Pages;
public partial class Index {
    [Inject]
    private UserFacade UserFacade { get; set; } = null!;

    private ICollection<UserDetailModel> _userDetailModels = new List<UserDetailModel>();

    protected override async Task OnInitializedAsync() {
        await LoadData();
        await base.OnInitializedAsync();
    }

    private async Task LoadData() {
        try {
            _userDetailModels = await UserFacade.GetAllAsync();
        } catch {
            ShowMessageBox("Could not load the data!", MessageBoxMode.Error);
        }
    }

    public async void SelectUser(Guid userId) {
        try {
            await UserFacade.GetByIdAsync(userId);
        } catch {
            ShowMessageBox("Selected user is not valid!", MessageBoxMode.Error);
            return;
        }

        NavManager.NavigateTo("/QuizListPage/" + userId.ToString());
    }
}