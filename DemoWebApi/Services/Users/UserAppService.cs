using AutoMapper;
using DemoWebApi.Dtos.Users;
using DemoWebApi.ExceptionHandling;
using DemoWebApi.Extensions;
using DemoWebApi.Helpers;
using DemoWebApi.Repositories.Users;
using Microsoft.Extensions.Localization;

namespace DemoWebApi.Services.Users
{
    public class UserAppService : ApplicationServiceBase, IUserAppService
    {
        public UserAppService(IConfiguration configuration, IMapper mapper, IStringLocalizer<ApplicationServiceBase> l, IUserRepository userRepository, IHttpContextAccessor httpContext) : base(configuration, mapper, l, userRepository, httpContext)
        {
        }

        public async Task<string> UpdateAvatarAsync(UserUpdateAvatarRequest input)
        {
            var userId = 1;

            var user = await UserRepository.FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null) throw new UserFriendlyException(L["DataNotFound"]);

            if (input.File != null)
            {
                string root = "wwwroot";

                string imageFolder = "Images";

                UploadFileHelper.CreateFolderIfNotExists(root, imageFolder);

                string fileName = await UploadFileHelper.UploadAsync($"{root}/{imageFolder}", input.File);

                user.Avatar = $"{imageFolder}/{fileName}";
            }
            else if (input.LinkAvatar.HasValue())
            {
                user.Avatar = input.LinkAvatar;
            }

            await UserRepository.UpdateAsync(user, true);

            string domain = Configuration["Domain"];

            return user.Avatar.StartsWith("http") ? user.Avatar : $"{domain}/{user.Avatar}";
        }
    }
}
