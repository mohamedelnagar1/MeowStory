using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeowStory.Application.Authentication
{
    public class TokenResponse
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }
    }
}