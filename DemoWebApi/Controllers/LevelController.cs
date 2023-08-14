using DemoWebApi.Dtos.Levels;
using DemoWebApi.EFCore;
using DemoWebApi.Entities;
using DemoWebApi.Services.Levels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DemoWebApi.Controllers
{
    [Route("/api/level")]
    public class LevelController : AppBaseController, ILevelAppService
    {
        private ApplicationDbContext DbContext { get; }
        private ILevelAppService LevelAppService { get; }

        public LevelController(ILevelAppService levelAppService)
        {
            LevelAppService = levelAppService;
        }

        [HttpPost]
        [Route("add")]
        public async Task AddAsync(CreateLevelDto input)
        {
            await LevelAppService.AddAsync(input);
        }
        [HttpPut]
        [Route("update")]
        public async Task<bool> UpdateLevel(int id, UpdateLevelDto input)
        {
            var level = await DbContext.Levels.FindAsync(id);
            if (level == null)
            {
                return false;
            }
            level.Name = input.name;
            DbContext.Levels.Update(level);
            await DbContext.SaveChangesAsync();
            return true;
        }
        [HttpDelete]
        [Route("delete")]
        public async Task<bool> DeleteLevel(int id)
        {
            var level = await DbContext.Levels.FindAsync(id);
            if (level == null)
            {
                return false;
            }
            DbContext.Levels.Remove(level);
            await DbContext.SaveChangesAsync();
            return true;
        }
        [HttpGet]
        [Route("GetAllCategory")]
        [Authorize]
        public async Task<List<LevelDto>> GetAllLevelAsync()
        {
            var level = await DbContext.Levels
                .Select(x => new Level
                {
                    Id = x.Id,
                    Name = x.Name,
                }).ToListAsync();
            return level;
        }
        [HttpGet]
        [Route("GetCategoryById")]
        [Authorize]
        public async Task<LevelDto> GetLevelById(int id)
        {
            var level = await DbContext.Levels.Select(x => new Level
            {
                Id = x.Id,
                Name = x.Name,
            }).FirstOrDefaultAsync(c => c.Id == id);

            return level;
        }
    }
}
