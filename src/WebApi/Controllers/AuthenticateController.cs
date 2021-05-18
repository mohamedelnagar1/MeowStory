using MeowStory.Application.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MeowStory.WebApi.Controllers
{
    public class AuthenticateController : BaseApiController
    {

        [HttpPost]
        public async Task<ActionResult<TokenResponse>> Post(AuthenticateCommand command)
        {
            return await Mediator.Send(command);
        }


    }
}