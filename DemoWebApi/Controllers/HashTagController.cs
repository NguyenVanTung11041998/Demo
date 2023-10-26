using DemoWebApi.Dtos.HashTag;
using DemoWebApi.Dtos.Levels;
using DemoWebApi.Helpers;
using DemoWebApi.Services.HashTag;
using DemoWebApi.Services.Levels;
using Microsoft.AspNetCore.Mvc;

namespace DemoWebApi.Controllers
{
    [Route("/api/HasTag")]
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
        public async Task<HashTagDto> UpdateHashTagAsync(UpdateHashTagDto input)
        {
             return await HashTagService.UpdateHashTagAsync(input);
        }
        
        [HttpDelete]
        [Route("delete")]
        public async Task DeleteHashTagAsync(int id)
        {
             await HashTagService.DeleteHashTagAsync(id);
        }
        [HttpGet]
        [Route("all")]
        public async Task<List<HashTagDto>> GetAllHashTagAsync()
        {
            return await HashTagService.GetAllHashTagAsync();
        }
        [HttpGet]
        [Route("get-by-id")]
        public async Task<HashTagDto> GetHashTagByIdAsync(int id)
        {
            return  await HashTagService.GetHashTagByIdAsync(id);
        }
        [HttpGet]
        [Route("paging")]
        public async Task<GridResult<HashTagDto>> GetAllPagingAsync(int page, int pageSize, string keyword)
        {
            return await HashTagService.GetAllPagingAsync(page, pageSize, keyword);
        }
    }
}
