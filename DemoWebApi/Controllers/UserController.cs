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
    [Route("/api/User")]

    public class UserController
    {
        private ApplicationDbContext DbContext { get; }

        public UserController(ApplicationDbContext context)
        {
            DbContext = context;
        }
        [HttpGet]
        [Route("all")]
        public async Task<List<UserDto>> GetAllUserAsync()
        {
            return await DbContext.Users
                .Select(x => new UserDto
                {
                    UserName = x.UserName,
                    UserEmail=x.UserEmail,
                    Password=x.PassWord
                }).ToListAsync();
        }

        [HttpPost]
        [Route("add")]
        public async Task<bool> AddAsync(UserCreateModel input)
        {
            var Users = new User
            {
                UserName = input.UserName,
                UserEmail = input.UserEmail,
                PassWord = input.PassWord
            };

            await DbContext.Users.AddAsync(Users);

            await DbContext.SaveChangesAsync();

            return true;
        }

    }
}
