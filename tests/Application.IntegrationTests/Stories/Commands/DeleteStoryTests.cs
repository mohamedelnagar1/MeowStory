using FluentAssertions;
using MeowStory.Application.Common.Exceptions;
using MeowStory.Domain.Entities;
using NUnit.Framework;
using System.Threading.Tasks;
using MeowStory.Application.Stories.Commands.VoteStory;
using MeowStory.Application.Stories.Commands.DeleteStory;
using MeowStory.Application.Stories.Commands.CreateStory;

namespace MeowStory.Application.IntegrationTests.Stories.Commands
{
    using static Testing;
    public class DeleteStoryTests
    {
        [Test]
        public async Task ShouldNotDeleteNotExistStory()
        {
            await RunAsAuthorAsync();

            var commandVote = new VoteStoryCommand()
            {
                Id = "xyz"
            };

            FluentActions.Invoking(() => SendAsync(commandVote)).Should().Throw<NotFoundException>();
        }


        [Test]
        public async Task ShouldDeleteStory()
        {
             await RunAsAuthorAsync();

            var command = new CreateStoryCommand
            {
                Title = "New Story",
                Description = "New Story Description",
            };

            var storyId = await SendAsync(command);

            var commandDelete = new DeleteStoryCommand()
            {
                Id = storyId
            };

            await SendAsync(commandDelete);


            var story = await FindAsync<Story>(storyId);


            story.Should().BeNull();
        }
    }
}
