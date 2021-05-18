using FluentValidation;

namespace MeowStory.Application.Stories.Commands.SubmitStory
{
    public class SubmitStoryCommandValidator : AbstractValidator<SubmitStoryCommand>
    {

        public SubmitStoryCommandValidator()
        {
            RuleFor(s => s.Id)
                .NotEmpty();
        }
    }
}
