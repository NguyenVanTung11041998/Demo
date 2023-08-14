using DemoWebApi.EFCore;
using DemoWebApi.Entities;

namespace DemoWebApi.Repositories.Levels
{
    public class LevelRepository : Repository<Level>, ILevelRepository
    {
        public LevelRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
