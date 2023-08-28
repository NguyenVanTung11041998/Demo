using DemoWebApi.Dtos.Users;

namespace DemoWebApi.Services.Users
{
    public interface IUserAppService
    {
        Task<string> UpdateAvatarAsync(UserUpdateAvatarRequest input);
    }
}
