using DemoWebApi.Dtos.Company;
using DemoWebApi.Helpers;
using DemoWebApi.Services.Companies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DemoWebApi.Controllers
{
    [Route("/api/company")]
    public class CompanyController : AppBaseController, ICompanyAppService
    {
        private ICompanyAppService companyAppService { get; }
        public CompanyController(ICompanyAppService companyAppService)
        {
            this.companyAppService = companyAppService;
        }
        [HttpPost]
        [Route("add")]
        public async Task AddAsync(CreateCompanyDto input)
        {
            await companyAppService.AddAsync(input);
        }
        [HttpPut]
        [Route("update")]
        public async Task UpdateCompanyAsync(UpdateCompanyDto input)
        {
           await companyAppService.UpdateCompanyAsync(input);
        }
        [HttpDelete]
        [Route("delete")]
        public async Task DeleteCompapnyAsync(int id)
        {
           await companyAppService.DeleteCompapnyAsync(id);
        }
        [HttpGet]
        [Route("all")]
        [Authorize]
        public async Task<List<CompanyDto>> GetAllCompanyAsync()
        {
            return await companyAppService.GetAllCompanyAsync();
        }
        [HttpGet]
        [Route("get-by-id")]
        public async Task<CompanyDto> GetCompanyByIdAsync(int id)
        {
            return await companyAppService.GetCompanyByIdAsync(id);
        }
        [HttpGet]
        [Route("paging")]
        public async Task<GridResult<CompanyDto>> GetAllPagingAsync(int page, int pageSize, string keyword)
        {
            return await companyAppService.GetAllPagingAsync(page, pageSize, keyword);
        }
    }
}
