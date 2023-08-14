using DemoWebApi.EFCore;
using DemoWebApi.Entities;

namespace DemoWebApi.Repositories.Users
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
