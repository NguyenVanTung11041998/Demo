using DemoWebApi.Dtos.Company;
using DemoWebApi.Dtos.Levels;
using DemoWebApi.Helpers;

namespace DemoWebApi.Services.Companies
{
    public interface ICompanyAppService
    {
        Task AddAsync(CreateCompanyDto input);
        Task UpdateCompanyAsync (UpdateCompanyDto input);
        Task DeleteCompapnyAsync(int id);
        Task<List<CompanyDto>> GetAllCompanyAsync();
        Task<CompanyDto> GetCompanyByIdAsync(int id);
        Task<GridResult<CompanyDto>> GetAllPagingAsync(int page, int pageSize, string keyword);

    }
}
