using FluentValidation;

namespace MeowStory.Application.Stories.Commands.CreateStory
{
    public class CreateStoryCommandValidator : AbstractValidator<CreateStoryCommand>
    {
        public CreateStoryCommandValidator()
        {
            RuleFor(s => s.Title)
                .MaximumLength(64)
                .NotEmpty();

            RuleFor(s => s.Description)
                .MaximumLength(512)
                .NotEmpty();
        }
    }
}
