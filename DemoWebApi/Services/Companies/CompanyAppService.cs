using AutoMapper;
using DemoWebApi.Dtos.Company;
using DemoWebApi.Entities;
using DemoWebApi.ExceptionHandling;
using DemoWebApi.Repositories.Companies;
using DemoWebApi.Repositories.Nationality;
using DemoWebApi.Repositories.Users;
using Microsoft.Extensions.Localization;
using System.Numerics;
using System.Xml.Linq;

namespace DemoWebApi.Services.Companies
{
    public class CompanyAppService : ApplicationServiceBase, ICompanyAppService
    {
        private ICompanyRepository CompanyRepository { get; }
        public CompanyAppService(IConfiguration configuration, IMapper mapper, IStringLocalizer<ApplicationServiceBase> l, IUserRepository userRepository, IHttpContextAccessor httpContext, ICompanyRepository companyRepository) : base(configuration, mapper, l, userRepository, httpContext)
        {
            CompanyRepository = companyRepository;
        }
        public async Task AddAsync(CreateCompanyDto input)
        {
            var company = new Company
            {
                Name = input.Name,
                Description = input.Description,
                Phone = input.Phone,
                Email = input.Email,
                LocationDescription = input.LocationDescription,
                Location = input.Location,
                FullNameCompany = input.FullNameCompany,
                Website = input.Website,
                MinScale = input.MinScale,
                MaxScale = input.MaxScale,
                Treatment = input.Treatment,
                IsHot = input.IsHot,
                LastUpdateIsHotTime = input.LastUpdateIsHotTime,
                CompanyUrl = input.CompanyUrl,
                NationalityId = input.NationalityId,
                UserId = 1,

            };
            await CompanyRepository.AddAsync(company, true);
        }
        public async Task UpdateCompanyAsync(UpdateCompanyDto input)
        {
            var company = await CompanyRepository.FirstOrDefaultAsync(x => x.Id == input.Id);

            if (company == null) throw new UserFriendlyException(L["DataNotFound"]);

            company.Name = input.Name;
            company.Description = input.Description;
            company.Phone = input.Phone;
            company.Email = input.Email;
            company.LocationDescription = input.LocationDescription;
            company.Location = input.Location;
            company.FullNameCompany = input.FullNameCompany;
            company.Website = input.Website;
            company.MinScale = input.MinScale;
            company.MaxScale = input.MaxScale;
            company.Treatment = input.Treatment;
            company.IsHot = input.IsHot;
            company.LastUpdateIsHotTime = input.LastUpdateIsHotTime;
            company.CompanyUrl = input.CompanyUrl;
            company.NationalityId = input.NationalityId;
            company.UserId = input.UserId;

            await CompanyRepository.UpdateAsync(company, true);

        }
        public async Task DeleteCompapnyAsync (int id)
        {
            var company = await CompanyRepository.FirstOrDefaultAsync(x => x.Id == id);

            if (company == null) throw new UserFriendlyException(L["DataNotFound"]);
            await CompanyRepository.DeleteAsync(company , true);
        }
    }
}
