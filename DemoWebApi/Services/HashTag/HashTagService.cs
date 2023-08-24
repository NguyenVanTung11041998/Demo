using AutoMapper;
using DemoWebApi.Dtos.HashTag;
using DemoWebApi.Entities;
using DemoWebApi.ExceptionHandling;
using DemoWebApi.Extensions;
using DemoWebApi.Helpers;
using DemoWebApi.Repositories.Hashtag;
using DemoWebApi.Repositories.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace DemoWebApi.Services.HashTag
{
    public class HashTagService : ApplicationServiceBase, IHashTagService
    {
        private IHashTagRepository HashTagRepository { get; }
        public HashTagService(IConfiguration configuration, IMapper mapper, IStringLocalizer<ApplicationServiceBase> l, IUserRepository userRepository, IHttpContextAccessor httpContext, IHashTagRepository hashTagRepository) : base(configuration, mapper, l, userRepository, httpContext)
        {
            HashTagRepository = hashTagRepository;
         }
        public async Task AddAsync(CreateHashTagDto input)
        {
            var hashTag = new Entities.HashTag
            {
                Name = input.Name,
                HashtagUrl = input.HashtagUrl,
                IsHot = input.IsHot
            };
            await HashTagRepository.AddAsync(hashTag, true);
        }
        public async Task UpdateLevelAsync(UpdateHashTagDto input)
        {
            var hashTag = await HashTagRepository.FirstOrDefaultAsync(x => x.Id == input.Id);

            if (hashTag == null) throw new UserFriendlyException(L["DataNotFound"]);

            hashTag.Name = input.Name;
            hashTag.HashtagUrl = input.HashtagUrl;
            hashTag.IsHot = input.IsHot;

            await HashTagRepository.UpdateAsync(hashTag, true);
        }

        public async Task DeleteLevelAsync(int id)
        {
            var hashTag = await HashTagRepository.FirstOrDefaultAsync(x => x.Id == id);

            if (hashTag == null) throw new UserFriendlyException(L["DataNotFound"]);

            await HashTagRepository.DeleteAsync(hashTag, true);
        }

        public async Task<List<HashTagDto>> GetAllLevelAsync()
        {
            var hashTags = await HashTagRepository.GetAllAsync();

            return Mapper.Map<List<HashTagDto>>(hashTags);
        }

        public async Task<HashTagDto> GetLevelByIdAsync(int id)
        {
            var level = await HashTagRepository.FirstOrDefaultAsync(x => x.Id == id);

            return Mapper.Map<HashTagDto>(level);
        }

        public async Task<GridResult<HashTagDto>> GetAllPagingAsync(int page, int pageSize, string keyword)
        {
            var query = HashTagRepository.WhereIf(keyword.HasValue(), x => x.Name.Contains(keyword));

            int totalCount = await query.CountAsync();

            var data = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            var items = Mapper.Map<List<HashTagDto>>(data);

            return new GridResult<HashTagDto>(totalCount, items);
        }
    }

}
