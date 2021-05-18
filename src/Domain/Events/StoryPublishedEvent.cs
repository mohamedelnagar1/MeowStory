using MeowStory.Domain.Common;
using MeowStory.Domain.Entities;

namespace MeowStory.Domain.Events
{
    public class StoryPublishedEvent : DomainEvent
    {
        public StoryPublishedEvent(Story story)
        {
            Story = story;
        }

        public Story Story { get; private set; }
    }
}