using DemoWebApi.Dtos.HashTag;
using DemoWebApi.Dtos.Levels;
using DemoWebApi.Helpers;
using DemoWebApi.Services.HashTag;
using DemoWebApi.Services.Levels;
using Microsoft.AspNetCore.Mvc;

namespace DemoWebApi.Controllers
{
    [Route("/api/level")]
    public class HashTagController : AppBaseController, IHashTagService
    {
        private IHashTagService HashTagService { get; }
        public HashTagController(IHashTagService hashTagService)
        {
            HashTagService = hashTagService;
        }
        [HttpPost]
        [Route("add")]
        public async Task AddAsync(CreateHashTagDto input)
        {
            await HashTagService.AddAsync(input);
        }
        [HttpPut]
        [Route("update")]
        public async Task UpdateLevelAsync(UpdateHashTagDto input)
        {
            await HashTagService.UpdateLevelAsync(input);
        }
        [HttpDelete]
        [Route("delete")]
        public async Task DeleteLevelAsync(int id)
        {
            await HashTagService.DeleteLevelAsync(id);
        }
        [HttpGet]
        [Route("all")]
        public async Task<List<HashTagDto>>GetAllLevelAsync()
        {
            return await HashTagService.GetAllLevelAsync();
        }
        [HttpGet]
        [Route("get-by-id")]
        public async Task<HashTagDto> GetLevelByIdAsync(int id)
        {
            return await HashTagService.GetLevelByIdAsync(id);
        }
        [HttpGet]
        [Route("paging")]
        public async Task<GridResult<HashTagDto>> GetAllPagingAsync(int page, int pageSize, string keyword)
        {
            return await HashTagService.GetAllPagingAsync(page, pageSize, keyword);
        }

        
    }
}
