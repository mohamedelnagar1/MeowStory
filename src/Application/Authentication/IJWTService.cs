using MeowStory.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeowStory.Application.Authentication
{
    public interface IJWTService
    {
        Task<string> GenerateTokenAsync(string userId);
    }
}
