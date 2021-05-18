using FluentValidation;

namespace MeowStory.Application.Stories.Commands.UpdateStory
{
    public class UpdateStoryCommandValidator : AbstractValidator<UpdateStoryCommand>
    {

        public UpdateStoryCommandValidator()
        {
            RuleFor(s => s.Title)
                .MaximumLength(64)
                .NotEmpty();

            RuleFor(s => s.Description)
                .MaximumLength(512)
                .NotEmpty();

            RuleFor(s => s.Content)
                .NotEmpty();
        }
    }
}
