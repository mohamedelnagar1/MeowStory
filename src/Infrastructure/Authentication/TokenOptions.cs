using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeowStory.Infrastructure.Authentication
{
    public class TokenOptions
    {
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int AccessExpiration { get; set; }
        public int RefreshExpiration { get; set; }
    }
}