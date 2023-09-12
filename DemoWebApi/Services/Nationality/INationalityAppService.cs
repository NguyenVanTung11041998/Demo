using DemoWebApi.Dtos.Nationality;
using DemoWebApi.Helpers;

namespace DemoWebApi.Services.Nationality
{
    public interface INationalityAppService
    {
        Task<string> AddAsync( CreateNationalityDto input);
        Task DeleteNationalityAsync(int id);
        Task<string> UpdateNationalityAsync(UpdateNationalityDto input);
        Task<List<NationalityDto>> GetAllNationalityAsync();
        Task<NationalityDto> GetNationalityByIdAsync(int id);
        Task<GridResult<NationalityDto>> GetAllPagingAsync(int page, int pageSize, string keyword);
    }
}
