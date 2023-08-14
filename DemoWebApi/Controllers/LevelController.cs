using DemoWebApi.Dtos.Levels;
using DemoWebApi.EFCore;
using DemoWebApi.Entities;
using DemoWebApi.Repositories.Levels;
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
        public async Task<bool> UpdateLevel(UpdateLevelDto input)
        {
            await LevelAppService.UpdateLevel(input, true);
            return true;
        }
        [HttpDelete]
        [Route("delete")]
        public async Task<bool> DeleteLevel(int id)
        {
            await LevelAppService.DeleteLevel(id);
            return true;
        }
        [HttpGet]
        [Route("GetAllCategory")]
        public async Task<List<LevelDto>> GetAllLevelAsync()
        {
            var level = await DbContext.Levels
                .Select(x => new LevelDto
                {
                    id = x.Id,
                    name = x.Name,
                }).ToListAsync();
            return level;
        }
        [HttpGet]
        [Route("GetCategoryById")]
        public async Task<LevelDto> GetLevelById(int id)
        {
            var level = await DbContext.Levels.Select(x => new LevelDto
            {
                id = x.Id,
                name = x.Name,
            }).FirstOrDefaultAsync(c => c.id == id);

            return level;
        }
    }
}
