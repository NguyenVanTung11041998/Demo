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
        [Route("CreateCategory")]
        [Authorize]
        public async Task<bool> CreateCategory(CategoryCreateModel input)
        {
            int createdUserId = GetUserIdOfCurrentUser(HttpContext);
            var categorys = new Category
            {
                Name = input.Name,
                CreatedUserId = createdUserId,
                CreatedDateTime = DateTime.Now
            };
            await DbContext.Category.AddAsync(categorys);
            await DbContext.SaveChangesAsync();
            return true;
        }
        [HttpPut]
        [Route("UpdateCategory")]
        [Authorize]
        public async Task<bool> UpdateCategory(int categoryId, CategoryUpdateModel input)
        {
            var category = await DbContext.Category.FindAsync(categoryId);
            if (category == null)
            {
                return false; 
            }
            category.Name = input.Name;
            await DbContext.SaveChangesAsync();
            return true; 
        }
        [HttpDelete]
        [Route("DeleteCategory")]
        [Authorize]
        public async Task<bool> DeleteCategory(int categoryId)
        {
            var category = await DbContext.Category.FindAsync(categoryId);
            if (category == null)
            {
                return false;
            }
            DbContext.Category.Remove(category); 
            await DbContext.SaveChangesAsync();
            return true; 
        }
        [HttpGet]
        [Route("GetAllCategory")]
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
        public async Task<CategoryDto> GetCategoryById(int id)
        {
            var category = await DbContext.Category.Select(x => new CategoryDto
            {
                Id = x.Id,
                Name = x.Name,
                CreatedUserId = x.CreatedUserId,
                CreatedDateTime = x.CreatedDateTime
            }).FirstOrDefaultAsync(c => c.Id == id);
           
            return category;
        }
    }
}
