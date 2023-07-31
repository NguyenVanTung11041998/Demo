using DemoWebApi.Dtos;
using DemoWebApi.EFCore;
using DemoWebApi.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DemoWebApi.Controllers
{
    [ApiController]
    [Route("/api/Category")]
    public class CategoryController : Controller
    {
        private ApplicationDbContext DbContext { get; }
        private IConfiguration Config { get; }
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
        public CategoryController(ApplicationDbContext context, IConfiguration config)
        {
            DbContext = context;
            Config = config;
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
        [Route("GetAllUser")]
        [Authorize]
        public async Task<List<CategoryDto>> GetAllUserAsync()
        {
            return await DbContext.Category
                .Select(x => new CategoryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    CreatedUserId = x.CreatedUserId,
                    CreatedDateTime = (DateTime)x.CreatedDateTime
                }).ToListAsync();
        }
        [HttpGet]
        [Route("GetCategoryById")]
        [Authorize]
        public async Task<List<CategoryDto>> GetCategoryById(int id)
        {
            var category = DbContext.Category.Select(x => new CategoryDto
            {
                Id = x.Id,
                Name = x.Name,
                CreatedUserId = x.CreatedUserId,
                CreatedDateTime = (DateTime)x.CreatedDateTime
            }).FirstOrDefault(c => c.Id == id);
           
            return null;
        }
    }
}
