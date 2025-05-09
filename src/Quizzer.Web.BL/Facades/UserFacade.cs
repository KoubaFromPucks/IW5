using Quizzer.Common.Models;

namespace Quizzer.Web.BL.Facades; 
public class UserFacade : IAppFacade {
    private IUserApiClient _userApiClient;

    public UserFacade(IUserApiClient userApiClient) {
        _userApiClient = userApiClient;
    }

    public async Task<ICollection<UserDetailModel>> GetAllAsync() {
        return await _userApiClient.UserGetAsync();
    }

    public async Task<Guid> UpdateAsync(UserDetailModel userDetailModel) {
        return await _userApiClient.UserPutAsync(userDetailModel);
    }

    public async Task<Guid> CreateAsync(UserDetailModel userDetailModel) {
        return await _userApiClient.UserPostAsync(userDetailModel);
    }

    public async Task<UserDetailModel> GetByIdAsync(Guid quizId) {
        return await _userApiClient.UserGetAsync(quizId);
    }

    public async Task<Guid> DeleteAsync(Guid quizId) {
        return await _userApiClient.UserDeleteAsync(quizId);
    }

    public async Task<ICollection<UserListModel>> GetByNameAsync(string name, bool exact) {
        return await _userApiClient.ByNameAsync(name, exact);
    }
}
