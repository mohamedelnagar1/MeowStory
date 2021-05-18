using MeowStory.Domain.Common;
using MeowStory.Domain.Entities;

namespace MeowStory.Domain.Events
{
    public class StoryRejectedEvent : DomainEvent
    {
        public StoryRejectedEvent(Story story)
        {
            Story = story;
        }

        public Story Story { get; private set; }
        public string RejectionReason { get => Story.RejectionReason; }
    }
}