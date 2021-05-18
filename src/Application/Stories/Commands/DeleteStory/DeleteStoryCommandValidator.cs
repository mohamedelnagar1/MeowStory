using FluentValidation;

namespace MeowStory.Application.Stories.Commands.DeleteStory
{
    public class DeleteStoryCommandValidator : AbstractValidator<DeleteStoryCommand>
    {

        public DeleteStoryCommandValidator()
        {
            RuleFor(s => s.Id)
                .NotEmpty();
        }
    }
}
