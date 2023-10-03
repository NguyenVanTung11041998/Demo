using DemoWebApi.Dtos.HashTag;
using DemoWebApi.Helpers;

namespace DemoWebApi.Services.HashTag
{
    public interface IHashTagService
    {
        Task AddAsync(CreateHashTagDto input);
        Task DeleteHashTagAsync(int id);
        Task<HashTagDto> UpdateHashTagAsync(UpdateHashTagDto input);
        Task<List<HashTagDto>> GetAllHashTagAsync();
        Task<HashTagDto> GetHashTagByIdAsync(int id);
        Task<GridResult<HashTagDto>> GetAllPagingAsync(int page, int pageSize, string keyword);
    }
}
