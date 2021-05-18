using FluentValidation;
using System;

namespace MeowStory.Application.Stories.Queries
{
    public class GetStoryCommentsWithPaginationQueryValidator : AbstractValidator<GetStoryCommentsWithPaginationQuery>
    {
        public GetStoryCommentsWithPaginationQueryValidator()
        {
            RuleFor(x => x.StoryId)
                .NotNull()
                .NotEmpty()
                .Must(s => Guid.TryParse(s, out Guid guid));

            RuleFor(x => x.PageNumber)
                .GreaterThanOrEqualTo(1).WithMessage("PageNumber at least greater than or equal to 1.");

            RuleFor(x => x.PageSize)
                .GreaterThanOrEqualTo(1).WithMessage("PageSize at least greater than or equal to 1.");
        }
    }
}
