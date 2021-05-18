using AutoMapper;
using AutoMapper.QueryableExtensions;
using MeowStory.Application.Common.Interfaces;
using MeowStory.Application.Common.Mappings;
using MeowStory.Application.Common.Models;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MeowStory.Application.Common.Exceptions;
using FluentValidation.Results;

namespace MeowStory.Application.Authentication
{
    public class AuthenticateCommand : IRequest<TokenResponse>
    {
        public string username { get; set; }
        public string password { get; set; }
    }

    public class AuthenticateCommandHandler : IRequestHandler<AuthenticateCommand, TokenResponse>
    {
        private readonly IIdentityService _identityService;


        public AuthenticateCommandHandler(IIdentityService identityService)
        {
             _identityService = identityService;
        }

        public async Task<TokenResponse> Handle(AuthenticateCommand request, CancellationToken cancellationToken)
        {
            var authenticateResponse = await this._identityService.SignInAsync(request.username, request.password);
            if (authenticateResponse.Succeeded)
                return authenticateResponse.TokenResponse;

            throw new ValidationException(new ValidationFailure[] { new ValidationFailure("", "Invalid username or password") });
        }
    }
}
