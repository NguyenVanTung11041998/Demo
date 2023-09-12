using DemoWebApi.Dtos.Company;

namespace DemoWebApi.Services.Companies
{
    public interface ICompanyAppService
    {
        Task AddAsync(CreateCompanyDto input);
        Task UpdateCompanyAsync (UpdateCompanyDto input);
        Task DeleteCompapnyAsync(int id);
    }
}
