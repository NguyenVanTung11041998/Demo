using DemoWebApi.Dtos.Levels;
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
    }
}
