using MeowStory.Application.Common.Interfaces;
using System;

namespace MeowStory.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.UtcNow;
    }
}
