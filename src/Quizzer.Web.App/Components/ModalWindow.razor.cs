using Microsoft.AspNetCore.Components;

namespace Quizzer.Web.App.Components;
public partial class ModalWindow {
    [Parameter]
    public EventCallback OnConfirm { get; set; }

    [Parameter]
    public EventCallback OnClose { get; set; }

    [Parameter]
    public string Title { get; set; } = "";

    [Parameter]
    public string TextBody { get; set; } = "";

    [Parameter]
    public string FirstButtonText { get; set; } = "";

    [Parameter]
    public string SecondButtonText { get; set; } = "";

    public bool ModalVisible { get; set; } = false;

    public void Close() {
        ModalVisible = false;

        if (OnClose.HasDelegate) {
            OnClose.InvokeAsync();
        }

        StateHasChanged();
    }

    private void Confirm() {
        ModalVisible = false;

        if (OnConfirm.HasDelegate) {
            OnConfirm.InvokeAsync();
        }

        StateHasChanged();
    }

    public void ShowModal() {
        ModalVisible = true;
        StateHasChanged();
    }
}
