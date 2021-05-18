using MeowStory.Application.Common.Models;
using System.Threading.Tasks;

namespace MeowStory.Application.Authentication
{
    public interface IIdentityService
    {
        Task<string> GetUserNameAsync(string userId);

        Task<bool> IsInRoleAsync(string userId, string role);

        Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password);

        Task<Result> DeleteUserAsync(string userId);

        Task<AuthenticateResponse> SignInAsync(string username, string password);
    }
}
