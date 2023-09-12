using DemoWebApi.Dtos.Company;
using DemoWebApi.Services.Companies;
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
    }
}
