using MeowStory.Application.Common.Models;
using MeowStory.Application.Stories.Commands.CreateStory;
using MeowStory.Application.Stories.Commands.DeleteStory;
using MeowStory.Application.Stories.Commands.ReviewStory;
using MeowStory.Application.Stories.Commands.SubmitStory;
using MeowStory.Application.Stories.Commands.UpdateStory;
using MeowStory.Application.Stories.Commands.ViewStory;
using MeowStory.Application.Stories.Commands.VoteStory;
using MeowStory.Application.Stories.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MeowStory.WebApi.Controllers
{
    
    public class StoriesController : BaseApiController
    {
        [HttpGet("public")]
        public async Task<ActionResult<PaginatedList<StoryItemDto>>> GetPublished([FromQuery] GetPublishedStoriesWithPaginationQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpGet("drafts")]
        [Authorize(Roles ="Author")]
        public async Task<ActionResult<PaginatedList<StoryItemDto>>> GetDrafts([FromQuery] GetDraftStoriesWithPaginationQuery query)
        {
            return await Mediator.Send(query);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<StoryDto>> GetStory(string id)
        {
            await Mediator.Send(new ViewStoryCommand() { Id = id });
            return await Mediator.Send(new GetStoryQuery() { Id = id });
        }


        [HttpGet("{id}/comments")]
        public async Task<ActionResult<PaginatedList<CommentDto>>> GetComments(string id, [FromQuery] GetStoryCommentsWithPaginationQuery query)
        {

            if (id != query.StoryId)
            {
                return BadRequest();
            }

            return await Mediator.Send(query);
        }


        [HttpPost]
        [Authorize(Roles = "Author")]
        public async Task<ActionResult<string>> Create(CreateStoryCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Author")]
        public async Task<ActionResult> Update(string id, UpdateStoryCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }

            await Mediator.Send(command);

            return NoContent();
        }

        [HttpPut("{id}/Review")]
        [Authorize(Roles = "Reviewer")]
        public async Task<ActionResult> Review(string id, ReviewStoryCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }

            await Mediator.Send(command);

            return NoContent();
        }

        [HttpPut("{id}/Submit")]
        [Authorize(Roles = "Author")]
        public async Task<ActionResult> Submit(string id, SubmitStoryCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }

            await Mediator.Send(command);

            return NoContent();
        }

        [HttpPut("{id}/Vote")]
        public async Task<ActionResult> Vote(string id)
        {
            await Mediator.Send(new VoteStoryCommand() { Id = id });

            return NoContent();
        }



        [HttpDelete("{id}")]
        [Authorize(Roles = "Author")]
        public async Task<ActionResult> Delete(string id)
        {
            await Mediator.Send(new DeleteStoryCommand { Id = id });

            return NoContent();
        }
    }
}
