using DemoWebApi.Dtos.Levels;
using DemoWebApi.Helpers;

namespace DemoWebApi.Services.Levels
{
    public interface ILevelAppService
    {
        Task AddAsync(CreateLevelDto input);
        Task DeleteLevelAsync(int id);
        Task UpdateLevelAsync(UpdateLevelDto input);
        Task<List<LevelDto>> GetAllLevelAsync();
        Task<LevelDto> GetLevelByIdAsync(int id);
        Task<GridResult<LevelDto>> GetAllPagingAsync(int page, int pageSize, string keyword);
    }
}
