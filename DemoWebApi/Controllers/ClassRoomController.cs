using DemoWebApi.Dtos;
using DemoWebApi.EFCore;
using DemoWebApi.Entities;
using DemoWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DemoWebApi.Controllers
{
    [ApiController]
    [Route("/api/class-room")]
    public class ClassRoomController
    {
        private ApplicationDbContext DbContext { get; }

        public ClassRoomController(ApplicationDbContext context)
        {
            DbContext = context;
        }

        [HttpGet]
        [Route("all")]
        public async Task<List<ClassRoomDto>> GetAllAsync()
        {
            return await DbContext.ClassRooms.Where(x => x.Name == "1B").Select(x => new ClassRoomDto { Id = x.Id, Name = x.Name }).ToListAsync();
        }

        [HttpPost]
        [Route("add")]
        public async Task<bool> AddAsync(ClassRoomCreateModel input)
        {
            var classRoom = new ClassRoom
            {
                Name = input.Name
            };

            await DbContext.ClassRooms.AddAsync(classRoom);

            await DbContext.SaveChangesAsync();

            return true;
        }
    }
}
