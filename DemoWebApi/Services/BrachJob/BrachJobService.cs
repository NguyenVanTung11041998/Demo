using AutoMapper;
using DemoWebApi.Dtos.BrandJob;
using DemoWebApi.Dtos.HashTag;
using DemoWebApi.Entities;
using DemoWebApi.ExceptionHandling;
using DemoWebApi.Extensions;
using DemoWebApi.Helpers;
using DemoWebApi.Repositories.BrachJob;
using DemoWebApi.Repositories.Hashtag;
using DemoWebApi.Repositories.Users;
using DemoWebApi.Services.HashTag;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace DemoWebApi.Services.BrachJob
{
    public class BrachJobService : ApplicationServiceBase, IBrachJobService
    {
        private IBrachJobRepository BrachJobRepository { get; }
        public BrachJobService(IConfiguration configuration, IMapper mapper, IStringLocalizer<ApplicationServiceBase> l, IUserRepository userRepository, IHttpContextAccessor httpContext, IBrachJobRepository brachJobRepository) : base(configuration, mapper, l, userRepository, httpContext)
        {
            BrachJobRepository = brachJobRepository;
        }
        public async Task AddAsync(CreateBrachJobDto input)
        {
            var brachJob = new BranchJob
            {
                Name = input.Name,
                BranchJobUrl = input.BranchJobUrl
            };
            await BrachJobRepository.AddAsync(brachJob, true);
        }
        public async Task UpdateBrachJobAsync(UpdateBrachJobDto input)
        {
            var brachJob = await BrachJobRepository.FirstOrDefaultAsync(x => x.Id == input.Id);

            if (brachJob == null) throw new UserFriendlyException(L["DataNotFound"]);

            brachJob.Name = input.Name;
            brachJob.BranchJobUrl = input.BranchJobUrl;

            await BrachJobRepository.UpdateAsync(brachJob, true);
        }

        public async Task DeleteBrachJobAsync(int id)
        {
            var brachJob = await BrachJobRepository.FirstOrDefaultAsync(x => x.Id == id);

            if (brachJob == null) throw new UserFriendlyException(L["DataNotFound"]);

            await BrachJobRepository.DeleteAsync(brachJob, true);
        }

        public async Task<List<BrachJobDto>> GetAllBrachJobAsync()
        {
            var brachJob = await BrachJobRepository.GetAllAsync();

            return Mapper.Map<List<BrachJobDto>>(brachJob);
        }

        public async Task<BrachJobDto> GetBrachJobByIdAsync(int id)
        {
            var brachJob = await BrachJobRepository.FirstOrDefaultAsync(x => x.Id == id);

            return Mapper.Map<BrachJobDto>(brachJob);
        }

        public async Task<GridResult<BrachJobDto>> GetAllPagingAsync(int page, int pageSize, string keyword)
        {
            var query = BrachJobRepository.WhereIf(keyword.HasValue(), x => x.Name.Contains(keyword));

            int totalCount = await query.CountAsync();

            var data = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            var items = Mapper.Map<List<BrachJobDto>>(data);

            return new GridResult<BrachJobDto>(totalCount, items);
        }
    }
}
