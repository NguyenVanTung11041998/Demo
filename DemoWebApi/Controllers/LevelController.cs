using DemoWebApi.Dtos.Levels;
using DemoWebApi.Helpers;
using DemoWebApi.Services.Levels;
using Microsoft.AspNetCore.Mvc;

namespace DemoWebApi.Controllers
{
    [Route("/api/level")]
    public class LevelController : AppBaseController, ILevelAppService
    {
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
        public async Task UpdateLevelAsync(UpdateLevelDto input)
        {
            await LevelAppService.UpdateLevelAsync(input);
        }

        [HttpDelete]
        [Route("delete")]
        public async Task DeleteLevelAsync(int id)
        {
            await LevelAppService.DeleteLevelAsync(id);
        }

        [HttpGet]
        [Route("all")]
        public async Task<List<LevelDto>> GetAllLevelAsync()
        {
            return await LevelAppService.GetAllLevelAsync();
        }

        [HttpGet]
        [Route("get-by-id")]
        public async Task<LevelDto> GetLevelByIdAsync(int id)
        {
            return await LevelAppService.GetLevelByIdAsync(id);
        }

        [HttpGet]
        [Route("paging")]
        public async Task<GridResult<LevelDto>> GetAllPagingAsync(int page, int pageSize, string keyword)
        {
            return await LevelAppService.GetAllPagingAsync(page, pageSize, keyword);
        }
    }
}
