using MeowStory.Application.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using MeowStory.Application.Authentication;

namespace MeowStory.Infrastructure.Authentication
{
    public class IdentityService : IIdentityService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;
        private readonly IAuthorizationService _authorizationService;
        private readonly IJWTService _tokenService;

        public IdentityService(
                        SignInManager<ApplicationUser> signInManager,
                        UserManager<ApplicationUser> userManager,
                        IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory,
                        IAuthorizationService authorizationService,
                        IJWTService tokenService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
            _authorizationService = authorizationService;
            _tokenService = tokenService;

        }

        public async Task<string> GetUserNameAsync(string userId)
        {
            var user = await _userManager.Users.FirstAsync(u => u.Id == userId);

            return user.UserName;
        }

        public async Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password)
        {
            var user = new ApplicationUser
            {
                UserName = userName,
                Email = userName,
            };

            var result = await _userManager.CreateAsync(user, password);

            return (result.ToApplicationResult(), user.Id);
        }

        public async Task<bool> IsInRoleAsync(string userId, string role)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

            return await _userManager.IsInRoleAsync(user, role);
        }


        public async Task<Result> DeleteUserAsync(string userId)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

            if (user != null)
            {
                return await DeleteUserAsync(user);
            }

            return Result.Success();
        }

        public async Task<Result> DeleteUserAsync(ApplicationUser user)
        {
            var result = await _userManager.DeleteAsync(user);

            return result.ToApplicationResult();
        }

        private async Task<ApplicationUser> ValidateUserAsync(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, password, true, false);

                if (result.Succeeded)
                {
                    return user;
                }
            }

            return null;
        }

        public async Task<AuthenticateResponse> SignInAsync(string username, string password)
        {
            var user = await ValidateUserAsync(username, password);

            if (user == null)
                return new AuthenticateResponse(false);

            var token = await _tokenService.GenerateTokenAsync(user.Id);
            var role = (await _userManager.GetRolesAsync(user))[0];
            var tokenResponse = new TokenResponse()
            {
                Id = user.Id,
                EmailAddress = user.Email,
                UserName = user.UserName,
                Token = token,
                Role = role
            };

            return new AuthenticateResponse(true, tokenResponse);

        }
    }
}
