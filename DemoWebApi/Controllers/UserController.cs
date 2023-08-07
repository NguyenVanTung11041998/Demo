﻿using DemoWebApi.Dtos;
using DemoWebApi.EFCore;
using DemoWebApi.Entities;
using DemoWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Xml.Linq;
using System.Threading.Tasks;
using DemoWebApi.Migrations;
using DemoWebApi.Helpers;
using DemoWebApi.ExceptionHandling;
using Microsoft.Extensions.Localization;

namespace DemoWebApi.Controllers
{
    [ApiController]
    [Route("/api/User")]

    public class UserController : Controller
    {
        
        private ApplicationDbContext DbContext { get; }
        private IConfiguration Config { get; }
        private static string[] SupportedTypes { get; } = new[] { "jpg", "jpeg", "png", "gif" };
        protected IStringLocalizer<UserController> L { get; }

        public UserController(ApplicationDbContext context, IConfiguration config, IStringLocalizer<UserController> l)
        {
            DbContext = context;
            Config = config;
            L = l;
            //for(int i = 2; i<=100; i++)
            //{
            //    DbContext.Users.Add(new User { Id = i, UserName =" Name "+i, UserEmail= "Email "+i, PassWord = "pass"+i });
            //}
        }
        [HttpGet]
        [Route("all")]
        [Authorize]
        public async Task<List<UserDto>> GetAllUserAsync()
        {
            return await DbContext.Users
                .Select(x => new UserDto
                {
                    UserName = x.UserName,
                    UserEmail=x.UserEmail
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

        [HttpPost]
        [Route("login")]
        public async Task<string> LoginAsync(LoginRequest input)
        {
            var user = await DbContext.Users.FirstOrDefaultAsync(x => x.UserName == input.Username && x.PassWord == input.Password);

            if (user == null) return null;

            return GenerateJSONWebToken(user);
        }
        [HttpGet]
        [Route("totalPages")]
        public async Task<GridUserDto> TotalPages(int page, int pageSize, string keyword)
        {
            IQueryable<User> query = DbContext.Users;

            if (!string.IsNullOrWhiteSpace(keyword))
                query = query.Where(x => x.UserName.Contains(keyword));

            int totalCount = await query.CountAsync();

            var list = await query.Skip((page - 1) * pageSize).Take(pageSize)
                            .Select(x => new UserDto
                            {
                                Avatar = x.Avatar,
                                UserEmail = x.UserEmail,
                                UserName = x.UserName
                            }).ToListAsync();

            return new GridUserDto { TotalCount = totalCount, Users = list };
        }
        [HttpPost]
        [Route("UploadFile")]
        public async Task<string> UploadFile([FromForm] UserAvatarDto input)
        {
            var userId = GetUserIdOfCurrentUser(HttpContext);

            var user = await DbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null) throw new UserFriendlyException("NoData");

            if (input.File != null)
            {
                string fileLocation = UploadFileHelper.CreateFolderIfNotExists("wwwroot", "Images");

                var fileExt = Path.GetExtension(input.File.FileName)[1..].ToLower();

                if (!SupportedTypes.Contains(fileExt))
                    throw new UserFriendlyException(L["InvalidFile"]);

                string fileName = await UploadFileHelper.UploadAsync(fileLocation, input.File);

                user.Avatar = $"/Images/{fileName}";

                DbContext.Users.Update(user);
            }

            return user.Avatar;
        }

        private string GenerateJSONWebToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("Id", user.Id.ToString()),
                new Claim("UserName", user.UserName)
            };

            var token = new JwtSecurityToken(Config["Jwt:Issuer"],
              Config["Jwt:Issuer"],
              claims,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static int GetUserIdOfCurrentUser(HttpContext httpContext)
        {
            _ = httpContext.Request.Headers.TryGetValue(HeaderNames.Authorization, out var tokenString);

            var jwtEncodedString = tokenString.ToString()[7..];

            var token = new JwtSecurityToken(jwtEncodedString);

            var claim = token.Claims.FirstOrDefault(x => x.Type == "Id");

            string id = claim.Value;

            return int.Parse(id);
        }
    }
}
