using DemoWebApi.EFCore;
using DemoWebApi.Entities;
using DemoWebApi.Repositories.BrachJob;

namespace DemoWebApi.Repositories.BranchJobCompanies
{
    public class BranchJobCompanyRepository : Repository<BranchJobCompany>, IBranchJobCompanyRepository
    {
        public BranchJobCompanyRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
