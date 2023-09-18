using DemoWebApi.Dtos.Users;
using Microsoft.AspNetCore.Mvc;

namespace DemoWebApi.Services.Users
{
    public interface IUserAppService
    {
        Task<string> AddAsync(CreateUserDto input);
        Task<string> LoginAsync(LoginRequest input);
        Task<string> UpdateUserAsync(UpdateUserDto input);
    }
}
