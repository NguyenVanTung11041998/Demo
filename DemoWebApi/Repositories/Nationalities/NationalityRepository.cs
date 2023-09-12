using DemoWebApi.EFCore;
using DemoWebApi.Entities;

namespace DemoWebApi.Repositories.Nationality
{
    public class NationalityRepository : Repository<Entities.Nationality>, INationalityRepository
    {
        public NationalityRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
