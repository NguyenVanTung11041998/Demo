using AutoMapper;
using DemoWebApi.Dtos.Nationality;
using DemoWebApi.Entities;
using DemoWebApi.ExceptionHandling;
using DemoWebApi.Extensions;
using DemoWebApi.Helpers;
using DemoWebApi.Repositories.Nationality;
using DemoWebApi.Repositories.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace DemoWebApi.Services.Nationality
{
    public class NationalityAppService : ApplicationServiceBase, INationalityAppService
    {
        private INationalityRepository NationalityRepository { get; }

        public NationalityAppService(IConfiguration configuration, IMapper mapper, IStringLocalizer<ApplicationServiceBase> l, IUserRepository userRepository, IHttpContextAccessor httpContext, INationalityRepository nationalityRepository) : base(configuration, mapper, l, userRepository, httpContext)
        {
            NationalityRepository = nationalityRepository;
        }

        public async Task<string> AddAsync(CreateNationalityDto input)
        {
            var nationality = new Entities.Nationality
            {
                Name = input.Name,
            };
            if (input.Path != null)
            {
                string root = "wwwroot";

                string imageFolder = "Images";

                UploadFileHelper.CreateFolderIfNotExists(root, imageFolder);

                string fileName = await UploadFileHelper.UploadAsync($"{root}/{imageFolder}", input.File);

                input.Path = $"{imageFolder}/{fileName}";
            }

            if (nationality != null) throw new UserFriendlyException(L["DataNotFound"]);

            await NationalityRepository.AddAsync(nationality, true);
            string domain = Configuration["Domain"];
            return input.Path.StartsWith("http") ? input.Path : $"{domain}/{input.Path}";

        }

        public async Task<string> UpdateNationalityAsync(UpdateNationalityDto input)
        {
            var nationality = await NationalityRepository.FirstOrDefaultAsync(x => x.Id == input.Id);

            if (nationality == null) throw new UserFriendlyException(L["DataNotFound"]);

            nationality.Name = input.Name;
            if (input.Path != null)
            {
                string root = "wwwroot";

                string imageFolder = "Images";

                UploadFileHelper.CreateFolderIfNotExists(root, imageFolder);

                string fileName = await UploadFileHelper.UploadAsync($"{root}/{imageFolder}", input.File);

                input.Path = $"{imageFolder}/{fileName}";
            }
            else if (input.Path.HasValue())
            {
                input.Path = input.Path;
            }

            if (nationality != null) throw new UserFriendlyException(L["DataNotFound"]);




            await NationalityRepository.UpdateAsync(nationality, true);
            string domain = Configuration["Domain"];
            return input.Path.StartsWith("http") ? input.Path : $"{domain}/{input.Path}";
        }

        public async Task DeleteNationalityAsync(int id)
        {
            var nationality = await NationalityRepository.FirstOrDefaultAsync(x => x.Id == id);

            if (nationality == null) throw new UserFriendlyException(L["DataNotFound"]);

            await NationalityRepository.DeleteAsync(nationality, true);
        }
        public async Task<List<NationalityDto>> GetAllNationalityAsync()
        {
            var nationalitys = await NationalityRepository.GetAllAsync();

            return Mapper.Map<List<NationalityDto>>(nationalitys);
        }

        public async Task<NationalityDto> GetNationalityByIdAsync(int id)
        {
            var nationality = await NationalityRepository.FirstOrDefaultAsync(x => x.Id == id);

            return Mapper.Map<NationalityDto>(nationality);
        }

        public async Task<GridResult<NationalityDto>> GetAllPagingAsync(int page, int pageSize, string keyword)
        {
            var query = NationalityRepository.WhereIf(keyword.HasValue(), x => x.Name.Contains(keyword));

            int totalCount = await query.CountAsync();

            var data = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            var items = Mapper.Map<List<NationalityDto>>(data);

            return new GridResult<NationalityDto>(totalCount, items);
        }
        public async Task<string> UpdatePathAsync(NationalityUpdatePathRequest input)
        {
            var nationalityId = 1;

            var nationality = await NationalityRepository.FirstOrDefaultAsync(x => x.Id == nationalityId);

            if (nationality == null) throw new UserFriendlyException(L["DataNotFound"]);

            if (input.File != null)
            {
                string root = "wwwroot";

                string imageFolder = "Images";

                UploadFileHelper.CreateFolderIfNotExists(root, imageFolder);

                string fileName = await UploadFileHelper.UploadAsync($"{root}/{imageFolder}", input.File);

                nationality.Path = $"{imageFolder}/{fileName}";
            }
            else if (input.Path.HasValue())
            {
                nationality.Path = input.Path;
            }

            await NationalityRepository.UpdateAsync(nationality, true);

            string domain = Configuration["Domain"];

            return nationality.Path.StartsWith("http") ? nationality.Path : $"{domain}/{nationality.Path}";
        }
    }
}
