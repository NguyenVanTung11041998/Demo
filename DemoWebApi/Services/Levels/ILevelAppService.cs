using DemoWebApi.Dtos.Levels;

namespace DemoWebApi.Services.Levels
{
    public interface ILevelAppService
    {
        Task AddAsync(CreateLevelDto input);
        Task DeleteLevel(int id);
        Task UpdateLevel (UpdateLevelDto input);
        Task GetAllLevelAsync ();
        Task GetLevelById(int id);
    }
}
