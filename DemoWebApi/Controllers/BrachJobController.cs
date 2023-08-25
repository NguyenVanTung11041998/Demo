using DemoWebApi.Dtos.BrandJob;
using DemoWebApi.Helpers;
using DemoWebApi.Services.BrachJob;
using Microsoft.AspNetCore.Mvc;

namespace DemoWebApi.Controllers
{
    [Route("/api/BrachJob")]
    public class BrachJobController : AppBaseController, IBrachJobService
    {
        private IBrachJobService BrachJobService { get; }
        public BrachJobController(IBrachJobService brachJobService)
        {
            BrachJobService = brachJobService;
        }
        [HttpPost]
        [Route("add")]
        public async Task AddAsync(CreateBrachJobDto input)
        {
            await BrachJobService.AddAsync(input);
        }
        [HttpDelete]
        [Route("delete")]
        public async Task DeleteBrachJobAsync(int id)
        {
            await BrachJobService.DeleteBrachJobAsync(id);
        }
        [HttpPut]
        [Route("update")]
        public async Task UpdateBrachJobAsync(UpdateBrachJobDto input)
        {
            await BrachJobService.UpdateBrachJobAsync(input);
        }
        [HttpGet]
        [Route("all")]
        public async Task<List<BrachJobDto>> GetAllBrachJobAsync()
        {
            return await BrachJobService.GetAllBrachJobAsync();
        }
        [HttpGet]
        [Route("get-by-id")]
        public async Task<BrachJobDto> GetBrachJobByIdAsync(int id)
        {
            return await BrachJobService.GetBrachJobByIdAsync(id);
        }
        [HttpGet]
        [Route("paging")]
        public async Task<GridResult<BrachJobDto>> GetAllPagingAsync(int page, int pageSize, string keyword)
        {
            return await BrachJobService.GetAllPagingAsync(page, pageSize, keyword);
        }
    }
}
