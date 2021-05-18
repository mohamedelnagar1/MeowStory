using FluentValidation;
using System;

namespace MeowStory.Application.Stories.Queries
{
    public class GetStoryQueryValidator : AbstractValidator<GetStoryQuery>
    {
        public GetStoryQueryValidator()
        {
            RuleFor(x => x.Id)
                .NotNull()
                .NotEmpty()
                .Must(s => Guid.TryParse(s, out Guid guid));
        }
    }
}
