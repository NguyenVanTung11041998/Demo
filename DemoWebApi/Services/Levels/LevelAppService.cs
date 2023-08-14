using AutoMapper;
using DemoWebApi.Dtos.Levels;
using DemoWebApi.Entities;
using DemoWebApi.Repositories.Levels;
using DemoWebApi.Repositories.Users;
using Microsoft.Extensions.Localization;

namespace DemoWebApi.Services.Levels
{
    public class LevelAppService : ApplicationServiceBase, ILevelAppService
    {
        private ILevelRepository LevelRepository { get; }

        public LevelAppService(IConfiguration configuration, IMapper mapper, IStringLocalizer<ApplicationServiceBase> l, IUserRepository userRepository, IHttpContextAccessor httpContext, ILevelRepository levelRepository) : base(configuration, mapper, l, userRepository, httpContext)
        {
            LevelRepository = levelRepository;
        }

        public async Task AddAsync(CreateLevelDto input)
        {
            var level = new Level { Name = input.Name };

            await LevelRepository.AddAsync(level, true);
        }
    }
}
