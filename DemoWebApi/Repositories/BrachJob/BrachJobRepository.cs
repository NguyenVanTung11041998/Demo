using DemoWebApi.EFCore;
using DemoWebApi.Entities;

namespace DemoWebApi.Repositories.BrachJob
{
    public class BrachJobRepository : Repository<BranchJob>, IBrachJobRepository
    {
        public BrachJobRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
