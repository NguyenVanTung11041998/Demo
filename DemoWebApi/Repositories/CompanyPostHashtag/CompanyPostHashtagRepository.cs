using DemoWebApi.EFCore;

namespace DemoWebApi.Repositories.CompanyPostHashtag
{
    public class CompanyPostHashtagRepository :Repository<Entities.CompanyPostHashtag>,ICompanyPostHashtagRepository
    {
        public CompanyPostHashtagRepository(ApplicationDbContext context) : base(context) { }
    }
}
