using DemoWebApi.EFCore;

namespace DemoWebApi.Repositories.Companies
{
    public class CompanyRepository :Repository<Entities.Company> , ICompanyRepository
    {
        public CompanyRepository(ApplicationDbContext context) : base(context) { }
    }
}
