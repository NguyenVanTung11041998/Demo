using DemoWebApi.Dtos.Users;
using DemoWebApi.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DemoWebApi.Controllers
{
    [Route("/api/user")]
    public class UserController : AppBaseController, IUserAppService
    {
        private IUserAppService UserAppService { get; }

        public UserController(IUserAppService userAppService)
        {
            UserAppService = userAppService;
        }

        [HttpPut]
        //[Authorize]
        [Route("update-avatar")]
        public async Task<string> UpdateAvatarAsync([FromForm] UserUpdateAvatarRequest input)
        {
            return await UserAppService.UpdateAvatarAsync(input);
        }
    }
}
