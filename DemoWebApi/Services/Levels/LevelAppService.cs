﻿using AutoMapper;
using DemoWebApi.Dtos.Levels;
using DemoWebApi.Entities;
using DemoWebApi.ExceptionHandling;
using DemoWebApi.Extensions;
using DemoWebApi.Helpers;
using DemoWebApi.Repositories.Levels;
using DemoWebApi.Repositories.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;

namespace DemoWebApi.Services.Levels
{
    public class LevelAppService : ApplicationServiceBase, ILevelAppService
    {
        private ILevelRepository LevelRepository { get; }

        private IMemoryCache MemoryCache { get; }

        public LevelAppService(IConfiguration configuration, IMapper mapper, IStringLocalizer<ApplicationServiceBase> l, IUserRepository userRepository, IHttpContextAccessor httpContext, ILevelRepository levelRepository, IMemoryCache memoryCache) : base(configuration, mapper, l, userRepository, httpContext)
        {
            LevelRepository = levelRepository;

            MemoryCache = memoryCache;
        }

        public async Task AddAsync(CreateLevelDto input)
        {
            var level = new Level { Name = input.Name };

            await LevelRepository.AddAsync(level, true);
        }

        public async Task UpdateLevelAsync(UpdateLevelDto input)
        {
            var level = await LevelRepository.FirstOrDefaultAsync(x => x.Id == input.Id);

            if (level == null) throw new UserFriendlyException(L["DataNotFound"]);

            level.Name = input.Name;

            await LevelRepository.UpdateAsync(level, true);
        }

        public async Task DeleteLevelAsync(int id)
        {
            var level = await LevelRepository.FirstOrDefaultAsync(x => x.Id == id);

            if (level == null) throw new UserFriendlyException(L["DataNotFound"]);

            await LevelRepository.DeleteAsync(level, true);

            MemoryCache.Remove($"Level_{id}");
        }
        public async Task<List<LevelDto>> GetAllLevelAsync()
        {
            var levels = await LevelRepository.GetAllAsync();

            return Mapper.Map<List<LevelDto>>(levels);
        }

        public async Task<LevelDto> GetLevelByIdAsync(int id)
        {
            string key = $"Level_{id}";

            var cache = MemoryCache.Get<LevelDto>(key);

            if (cache != null) return cache;

            var level = await LevelRepository.FirstOrDefaultAsync(x => x.Id == id);

            var dto = Mapper.Map<LevelDto>(level);

            if (dto == null) return null;

            MemoryCache.Set(key, dto, new MemoryCacheEntryOptions { AbsoluteExpiration = DateTime.Now.AddHours(2) });

            return dto;
        }

        public async Task<GridResult<LevelDto>> GetAllPagingAsync(int page, int pageSize, string keyword)
        {
            var query = LevelRepository.WhereIf(keyword.HasValue(), x => x.Name.Contains(keyword));

            int totalCount = await query.CountAsync();

            var data = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            var items = Mapper.Map<List<LevelDto>>(data);

            return new GridResult<LevelDto>(totalCount, items);
        }
    }
}
