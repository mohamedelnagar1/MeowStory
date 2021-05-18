using MeowStory.Domain.Common;
using MeowStory.Domain.Entities;

namespace MeowStory.Domain.Events
{
    public class StorySubmittedEvent : DomainEvent
    {
        public StorySubmittedEvent(Story story)
        {
            Story = story;
        }

        public Story Story { get; private set; }
    }
}