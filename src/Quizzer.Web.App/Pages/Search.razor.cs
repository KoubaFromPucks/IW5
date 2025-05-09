using Microsoft.AspNetCore.Components;
using Quizzer.Common.Models;
using Quizzer.Web.BL;
using Quizzer.Web.BL.Facades;

namespace Quizzer.Web.App.Pages;
public partial class Search {
    [Inject]
    private UserFacade UserFacade { get; set; } = null!;
    [Inject]
    private QuizFacade QuizFacade { get; set; } = null!;
    [Inject]
    private QuestionFacade QuestionFacade { get; set; } = null!;
    [Inject]
    private AnswerFacade AnswerFacade { get; set; } = null!;

    //private ICollection<UserDetailModel> _users = new List<UserDetailModel>();
    private ICollection<UserListModel> _users = new List<UserListModel>();
    private ICollection<QuizListModel> _quizzesName = new List<QuizListModel>();
    private ICollection<QuizListModel> _quizzesDesc = new List<QuizListModel>();
    private ICollection<QuestionDetailModel> _questions = new List<QuestionDetailModel>();
    private IEnumerable<AnswerDetailModel> _answers = new List<AnswerDetailModel>();

    public string SearchStr { get; set; } = "";

    public bool Exact { get; set; }
    public bool ShowUser { get; set; } = true;
    public bool ShowQuizName { get; set; } = true;
    public bool ShowQuizDesc { get; set; } = true;
    public bool ShowQuestion { get; set; } = true;
    public bool ShowAnswer { get; set; } = true;

    protected override async Task OnInitializedAsync() {
        await base.OnInitializedAsync();
    }

    public async Task DoSearch() {
        HideMessageBox();
        if (SearchStr.Trim() == "") {
            ShowMessageBox("Can't search empty string", MessageBoxMode.Error);
            return;
        }
        try {
            if (ShowUser) {
                _users = await UserFacade.GetByNameAsync(SearchStr, Exact);
            }
            if (ShowQuizName) {
                _quizzesName = await QuizFacade.GetQuizByContentAsync(SearchStr, "X_X", Exact);
            }
            if (ShowQuizDesc) {
                _quizzesDesc = await QuizFacade.GetQuizByContentAsync("X_X", SearchStr, Exact);
            }
            if (ShowQuestion) {
                _questions = await QuestionFacade.GetQuestionByNameAsync(SearchStr, Exact);
            }
            if (ShowAnswer) {
                _answers = await AnswerFacade.GetAnswerByTextAsync(SearchStr, Exact);
            }
        } catch (ApiException<string> exception) {
            ShowMessageBox(exception.Result, MessageBoxMode.Error);
        } catch (Exception exception) {
            ShowMessageBox(exception.Message, MessageBoxMode.Error);
        }
    }
}
