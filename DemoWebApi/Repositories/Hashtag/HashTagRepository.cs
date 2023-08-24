using DemoWebApi.EFCore;
using DemoWebApi.Entities;

namespace DemoWebApi.Repositories.Hashtag
{
    public class HashTagRepository : Repository<HashTag>, IHashTagRepository
    {
        public HashTagRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}