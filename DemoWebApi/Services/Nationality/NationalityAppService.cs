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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

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
            if (input.File != null)
            {
                string root = "wwwroot";

                string imageFolder = "Images";

                UploadFileHelper.CreateFolderIfNotExists(root, imageFolder);

                string fileName = await UploadFileHelper.UploadAsync($"{root}/{imageFolder}", input.File);

                input.Path = $"{imageFolder}/{fileName}";
                
                nationality.Path = input.Path ;
            }
            var check = await NationalityRepository.AnyAsync(x => x.Name == input.Name);

            if (check == true ) throw new UserFriendlyException(L["DataAlreadyExists",input.Name]);

            await NationalityRepository.AddAsync(nationality, true);

            string domain = Configuration["Domain"];

            return input.Path.StartsWith("http") ? input.Path : $"{domain}/{input.Path}";

        }

        public async Task<string> UpdateNationalityAsync(UpdateNationalityDto input)
        {
            var nationality = await NationalityRepository.FirstOrDefaultAsync(x => x.Id == input.Id);

            if (nationality == null) throw new UserFriendlyException(L["DataNotFound"]);

            nationality.Name = input.Name;
            if (input.File != null)
            {
                string root = "wwwroot";

                string imageFolder = "Images";

                UploadFileHelper.CreateFolderIfNotExists(root, imageFolder);

                string fileName = await UploadFileHelper.UploadAsync($"{root}/{imageFolder}", input.File);

                input.Path = $"{imageFolder}/{fileName}";

                nationality.Path = input.Path;
            }
            else if (input.Path.HasValue())
            {
                input.Path = input.Path;
            }

            var check = await NationalityRepository.AnyAsync(x => x.Name == input.Name && x.Id != input.Id);

            if (check == true ) throw new UserFriendlyException(L["DataAlreadyExists"]);

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
    }
}
