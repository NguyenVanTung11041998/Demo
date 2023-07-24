using DemoWebApi.Dtos;
using DemoWebApi.EFCore;
using DemoWebApi.Entities;
using DemoWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Xml.Linq;


namespace DemoWebApi.Controllers
{
    [ApiController]
    [Route("/api/Student")]

    public class StudentController
    {
        private ApplicationDbContext DbContext { get; }

        public StudentController(ApplicationDbContext context)
        {
            DbContext = context;
        }

        [HttpGet]
        [Route("all")]
        public async Task<List<StudentDto>> GetAllStudentAsync()
        {
            return await DbContext.Students
                .Include(x => x.ClassRoom)
                .Select(x => new StudentDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Address = x.Address,
                    DateOfBirth = x.DateOfBirth,
                    ClassId = x.ClassId,
                    ClassName = x.ClassRoom.Name
                }).ToListAsync();
        }

        [HttpPost]
        [Route("add")]
        public async Task<bool> AddAsync(StudentCreateModel input)
        {
            var Students = new Student
            {
                Name = input.Name,
                Address = input.Address,
                DateOfBirth = input.DateOfBirth,
                ClassId = input.ClassId,
            };

            await DbContext.Students.AddAsync(Students);

            await DbContext.SaveChangesAsync();

            return true;
        }
        [HttpPut]
        [Route("update")]
        public async Task<bool> UpdateAsync(StudentUpdateRequest input)
        {
            // nguyên tắc update
            // B1 get entity từ trong db
            var Students = await DbContext.Students.FirstOrDefaultAsync(x => x.Id == input.Id);

            // check null
            if (Students == null) return false;

            Students.Name = input.Name;

            Students.DateOfBirth = input.DateOfBirth;

            Students.Address = input.Address;

            Students.ClassId = input.ClassId;

            //// update
            //var Student = new Student
            //{
            //    Name = input.Name,
            //    Address = input.Address,
            //    DateOfBirth = input.DateOfBirth,
            //    ClassId = input.ClassId,
            //};

            DbContext.Students.Update(Students);

            await DbContext.SaveChangesAsync();

            return true;
        }
        [HttpGet]
        [Route("get-by-id")]
        public async Task<StudentDto> GetByIdAsync(int id)
        {
            var Students = await DbContext.Students.Select(x => new StudentDto
            {
                Id = x.Id,
                Name = x.Name,
                Address = x.Address,
                DateOfBirth = x.DateOfBirth,
                ClassId = x.ClassId,
            }).FirstOrDefaultAsync(x => x.Id == id);

            return Students;
        }
        [HttpDelete]
        [Route("delete")]
        public async Task<bool> DeleteAsync(int id)
        {
            // nguyên tắc xóa
            // B1 get entity từ trong db
            var Students = await DbContext.Students.FirstOrDefaultAsync(x => x.Id == id);

            // check null
            if (Students == null) return false;

            // xóa khỏi db
            DbContext.Students.Remove(Students);

            await DbContext.SaveChangesAsync();

            return true;
        }

        [HttpGet]
        [Route("all-student-by-class")]
        public async Task<List<StudentDto>> GetAllByClassIdAsync(int classId)
        {
            return await DbContext.ClassRooms.Where(x => x.Id == classId)
                                .Include(x => x.Students)
                                .Select(x => new
                                {
                                    ClassName = x.Name,
                                    ClassId = x.Id,
                                    Students = x.Students.Select(p => new StudentDto
                                    {
                                        Id = p.Id,
                                        Name = p.Name,
                                        ClassName = x.Name,
                                        ClassId = x.Id,
                                        Address = p.Address,
                                        DateOfBirth = p.DateOfBirth
                                    })
                                })
                                .SelectMany(x => x.Students)
                                .ToListAsync();
        }

        [HttpGet]
        [Route("student-of-class")]
        public async Task<List<ClassStudentDto>> GetAllStudentClassAsync()
        {
            return await DbContext.ClassRooms
                            .Include(x => x.Students)
                            .Select(x => new ClassStudentDto
                            {
                                ClassName = x.Name,
                                Students = x.Students.Select(p => new StudentDto
                                {
                                    Id = p.Id,
                                    Name = p.Name,
                                    ClassName = x.Name,
                                    ClassId = x.Id,
                                    Address = p.Address,
                                    DateOfBirth = p.DateOfBirth
                                }).ToList()
                            }).ToListAsync();
        }
    }
}
