using MeowStory.Domain.Common;
using System.Threading.Tasks;

namespace MeowStory.Application.Common.Interfaces
{
    public interface IDomainEventService
    {
        Task Publish(DomainEvent domainEvent);
    }
}
