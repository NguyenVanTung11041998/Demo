using AutoMapper;
using DemoWebApi.Dtos.Company;
using DemoWebApi.Dtos.Levels;
using DemoWebApi.Entities;
using DemoWebApi.ExceptionHandling;
using DemoWebApi.Extensions;
using DemoWebApi.Helpers;
using DemoWebApi.Repositories.Companies;
using DemoWebApi.Repositories.Levels;
using DemoWebApi.Repositories.Nationality;
using DemoWebApi.Repositories.Users;
using Microsoft.EntityFrameworkCore;
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
            company = await CompanyRepository.AddAsync(company, true);

            if (input.BranchIds != null && input.BranchIds.Count > 0)
            {
                var entities = input.BranchIds.Select(x => new BranchJobCompany
                {
                    BranchJobId = x,
                    CompanyId = company.Id
                });

                // insert vào db
            }

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

            // lấy ra branch job company theo id công ty
            // xóa mảng branch job company cũ đi và insert mảng mới

        }
        public async Task DeleteCompapnyAsync (int id)
        {
            var company = await CompanyRepository.FirstOrDefaultAsync(x => x.Id == id);

            if (company == null) throw new UserFriendlyException(L["DataNotFound"]);
            await CompanyRepository.DeleteAsync(company , true);
        }
        public async Task<List<CompanyDto>> GetAllCompanyAsync()
        {
            var levels = await CompanyRepository.GetAllAsync();

            return Mapper.Map<List<CompanyDto>>(levels);
        }

        public async Task<CompanyDto> GetCompanyByIdAsync(int id)
        {
            var level = await CompanyRepository.FirstOrDefaultAsync(x => x.Id == id);

            return Mapper.Map<CompanyDto>(level);
        }

        public async Task<GridResult<CompanyDto>> GetAllPagingAsync(int page, int pageSize, string keyword)
        {
            var query = CompanyRepository.WhereIf(keyword.HasValue(), x => x.Name.Contains(keyword));

            int totalCount = await query.CountAsync();

            var data = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            var items = Mapper.Map<List<CompanyDto>>(data);

            return new GridResult<CompanyDto>(totalCount, items);
        }

       
    }
}
