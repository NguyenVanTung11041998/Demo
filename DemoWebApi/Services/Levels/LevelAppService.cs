using AutoMapper;
using DemoWebApi.Dtos.Levels;
using DemoWebApi.Entities;
using DemoWebApi.Repositories.Levels;
using DemoWebApi.Repositories.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<bool> UpdateLevel(int id, UpdateLevelDto input)
        {
            var level = await LevelRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (level == null)
            {
                return false;
            }
            level.Name = input.name;
            await LevelRepository.UpdateAsync(level, true);
            return true;
        }
        public async Task<bool> DeleteLevel(int id)
        {
            var level = await LevelRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (level == null)
            {
                return false;
            }
            await LevelRepository.DeleteAsync(level, true);
            return true;
        }
        public async Task<List<LevelDto>> GetAllLevelAsync()
        {
            var level = await LevelRepository.GetAllAsync();
            return level;
        }
        public async Task<LevelDto> GetLevelById(int id)
        {
            var level = await LevelRepository.FirstOrDefaultAsync(x => x.Id == id);
            return level;

        }
    }
}
