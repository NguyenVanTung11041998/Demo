using DemoWebApi.Dtos.Levels;

namespace DemoWebApi.Services.Levels
{
    public interface ILevelAppService
    {
        Task AddAsync(CreateLevelDto input);
    }
}
