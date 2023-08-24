using DemoWebApi.Dtos.HashTag;
using DemoWebApi.Helpers;

namespace DemoWebApi.Services.HashTag
{
    public interface IHashTagService
    {
        Task AddAsync(CreateHashTagDto input);
        Task DeleteLevelAsync(int id);
        Task UpdateLevelAsync(UpdateHashTagDto input);
        Task<List<HashTagDto>> GetAllLevelAsync();
        Task<HashTagDto> GetLevelByIdAsync(int id);
        Task<GridResult<HashTagDto>> GetAllPagingAsync(int page, int pageSize, string keyword);
    }
}
