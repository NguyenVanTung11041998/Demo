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
        public async Task<List<ClassRoomDto>> GetAllClassRoomAsync()
        {
            return await DbContext.ClassRooms.Select(x => new ClassRoomDto { Id = x.Id, Name = x.Name }).ToListAsync();
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

        [HttpPut]
        [Route("update")]
        public async Task<bool> UpdateAsync(ClassRoomUpdateRequest input)
        {
            // nguyên tắc update
            // B1 get entity từ trong db
            var classRoom = await DbContext.ClassRooms.FirstOrDefaultAsync(x => x.Id == input.Id);

            // check null
            if (classRoom == null) return false;

            // update
            classRoom.Name = input.Name;

            DbContext.ClassRooms.Update(classRoom);

            await DbContext.SaveChangesAsync();

            return true;
        }

        [HttpGet]
        [Route("get-by-id")]
        public async Task<ClassRoomDto> GetByIdAsync(int id)
        {
            var classRoom = await DbContext.ClassRooms.Select(x => new ClassRoomDto
            {
                Id = x.Id,
                Name = x.Name
            }).FirstOrDefaultAsync(x => x.Id == id);

            return classRoom;
        }

        [HttpDelete]
        [Route("delete")]
        public async Task<bool> DeleteAsync(int id)
        {
            // nguyên tắc xóa
            // B1 get entity từ trong db
            var classRoom = await DbContext.ClassRooms.FirstOrDefaultAsync(x => x.Id == id);

            // check null
            if (classRoom == null) return false;

            // xóa khỏi db
            DbContext.ClassRooms.Remove(classRoom);

            await DbContext.SaveChangesAsync();

            return true;
        }
        
    }
}
