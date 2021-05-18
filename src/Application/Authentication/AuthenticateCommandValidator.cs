using AutoMapper;
using AutoMapper.QueryableExtensions;
using MeowStory.Application.Common.Interfaces;
using MeowStory.Application.Common.Mappings;
using MeowStory.Application.Common.Models;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;

namespace MeowStory.Application.Authentication
{

    public class AuthenticateCommandValidator : AbstractValidator<AuthenticateCommand>
    {
        public AuthenticateCommandValidator()
        {
            RuleFor(x => x.username)
                .NotNull()
                .NotEmpty()
                .MaximumLength(32);

            RuleFor(x => x.password)
                .NotNull()
                .NotEmpty()
                .MaximumLength(32);


        }
    }
}
