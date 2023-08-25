using DemoWebApi.Dtos.BrandJob;
using DemoWebApi.Helpers;

namespace DemoWebApi.Services.BrachJob
{
    public interface IBrachJobService
    {
        Task AddAsync(CreateBrachJobDto input);
        Task DeleteBrachJobAsync(int id);
        Task UpdateBrachJobAsync(UpdateBrachJobDto input);
        Task<List<BrachJobDto>> GetAllBrachJobAsync();
        Task<BrachJobDto> GetBrachJobByIdAsync(int id);
        Task<GridResult<BrachJobDto>> GetAllPagingAsync(int page, int pageSize, string keyword);
    }
}
