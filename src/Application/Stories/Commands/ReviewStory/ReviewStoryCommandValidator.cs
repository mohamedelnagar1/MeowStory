using FluentValidation;

namespace MeowStory.Application.Stories.Commands.ReviewStory
{
    public class ReviewStoryCommandValidator : AbstractValidator<ReviewStoryCommand>
    {

        public ReviewStoryCommandValidator()
        {
            RuleFor(s => s.Id)
                .NotEmpty();

            RuleFor(s => s.RejectionReason)
                .NotEmpty()
                .When(s => !s.IsApproved);


            RuleFor(s => s.RejectionReason)
                .Null()
                .When(s => s.IsApproved);
        }
    }
}
