using DemoWebApi.Dtos.Nationality;
using DemoWebApi.Helpers;
using DemoWebApi.Services.Nationality;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace DemoWebApi.Controllers
{
    [Route("/api/nationality")]
    public class NationalityController : AppBaseController, INationalityAppService
    {
        private INationalityAppService NationalityAppService { get; }

        public NationalityController(INationalityAppService nationalityAppService)
        {
            NationalityAppService = nationalityAppService;
        }
        [HttpPost]
        [Route("add")]
        public async Task<string> AddAsync([FromForm] CreateNationalityDto input)
        {
           return await NationalityAppService.AddAsync( input);

        }
        [HttpDelete]
        [Route("delete")]
        public async Task DeleteNationalityAsync(int id)
        {
            await NationalityAppService.DeleteNationalityAsync(id);
        }
        [HttpPut]
        [Route("update")]
        public async Task<string> UpdateNationalityAsync([FromForm] UpdateNationalityDto input)
        {
            return await NationalityAppService.UpdateNationalityAsync(input);
        }
        [HttpGet]
        [Route("all")]
        public async Task<List<NationalityDto>> GetAllNationalityAsync()
        {
            return await NationalityAppService.GetAllNationalityAsync();
        }
        [HttpGet]
        [Route("get-by-id")]
        public async Task<NationalityDto> GetNationalityByIdAsync(int id)
        {
            return await NationalityAppService.GetNationalityByIdAsync(id);
        }
        [HttpGet]
        [Route("paging")]
        public async Task<GridResult<NationalityDto>> GetAllPagingAsync(int page, int pageSize, string keyword)
        {
            return await NationalityAppService.GetAllPagingAsync(page, pageSize, keyword);
        }
    }
}