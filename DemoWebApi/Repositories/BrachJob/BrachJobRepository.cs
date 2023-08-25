using DemoWebApi.EFCore;
using DemoWebApi.Entities;
using DemoWebApi.Repositories.Hashtag;

namespace DemoWebApi.Repositories.BrachJob
{
    public class BrachJobRepositpry : Repository<BranchJob>, IBrachJobRepository
    {
        public BrachJobRepositpry(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
