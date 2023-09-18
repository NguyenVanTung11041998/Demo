using DemoWebApi.Dtos.Users;
using DemoWebApi.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

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
        [HttpPost]
        [Route("add")]
        public async Task<string> AddAsync(CreateUserDto input)
        {
            return await UserAppService.AddAsync(input);    
        }
        [HttpPost]
        [Route("login")]
        public async Task<string> LoginAsync(LoginRequest input)
        {
            return await UserAppService.LoginAsync(input);
        }
        [HttpPost]
        [Route("update")]
        public async Task<string> UpdateUserAsync(UpdateUserDto input)
        {
            return await UserAppService.UpdateUserAsync(input);
        }
    }
}
