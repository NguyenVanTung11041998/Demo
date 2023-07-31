using DemoWebApi.Dtos;
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


namespace DemoWebApi.Controllers
{
    [ApiController]
    [Route("/api/User")]

    public class UserController
    {
        private ApplicationDbContext DbContext { get; }
        private IConfiguration Config { get; }

        public UserController(ApplicationDbContext context, IConfiguration config)
        {
            DbContext = context;
            Config = config;
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

        [HttpPost]
        [Route("login")]
        public async Task<string> LoginAsync(LoginRequest input)
        {
            var user = await DbContext.Users.FirstOrDefaultAsync(x => x.UserName == input.Username && x.PassWord == input.Password);

            if (user == null) return null;

            return GenerateJSONWebToken(user);
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
