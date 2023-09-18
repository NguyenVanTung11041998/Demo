using AutoMapper;
using DemoWebApi.Dtos.Nationality;
using DemoWebApi.Dtos.Users;
using DemoWebApi.Entities;
using DemoWebApi.ExceptionHandling;
using DemoWebApi.Extensions;
using DemoWebApi.Helpers;
using DemoWebApi.Models;
using DemoWebApi.Repositories.Nationality;
using DemoWebApi.Repositories.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace DemoWebApi.Services.Users
{
    public class UserAppService : ApplicationServiceBase, IUserAppService
    {
        private IConfiguration Config { get; }
        private IUserRepository UserRepository { get; }
        public UserAppService(IConfiguration configuration, IMapper mapper, IStringLocalizer<ApplicationServiceBase> l, UserRepository userRepository, IHttpContextAccessor httpContext) : base(configuration, mapper, l, userRepository, httpContext)
        {
            UserRepository = userRepository;
        }
        public async Task<string> AddAsync(CreateUserDto input)
        {
            var user = new Entities.User
            {
                Email = input.Email,
                Password = input.Password,
                FullName = input.FullName,
                DateOfBirth = input.DateOfBirth,
                Address = input.Address
            };
            if (input.File != null)
            {
                string root = "wwwroot";

                string imageFolder = "Images";

                UploadFileHelper.CreateFolderIfNotExists(root, imageFolder);

                string fileName = await UploadFileHelper.UploadAsync($"{root}/{imageFolder}", input.File);

                input.Path = $"{imageFolder}/{fileName}";

                user.Path = input.Path;
            }
            var check = await UserRepository.AnyAsync(x => x.Email == input.Email);

            if (check == true) throw new UserFriendlyException(L["DataAlreadyExists", input.Email]);

            await UserRepository.AddAsync(user, true);

            string domain = Configuration["Domain"];

            return input.Path.StartsWith("http") ? input.Path : $"{domain}/{input.Path}";

        }
        public async Task<string> LoginAsync(LoginRequest input)
        {
            var user = await UserRepository.FirstOrDefaultAsync(x => x.Email == input.Username && x.Password == input.Password);

            if (user == null) return null;

            return GenerateJSONWebToken(user);
        }
        public async Task<string> UpdateUserAsync(UpdateUserDto input)
        {
            var user = await UserRepository.FirstOrDefaultAsync(x => x.Id == input.Id);

            if (user == null) throw new UserFriendlyException(L["DataNotFound"]);

            user.Email = input.Email;
            user.Password = input.Password;
            user.FullName = input.FullName;
            user.DateOfBirth = input.DateOfBirth;
            user.Address = input.Address;
            if (input.File != null)
            {
                string root = "wwwroot";

                string imageFolder = "Images";

                UploadFileHelper.CreateFolderIfNotExists(root, imageFolder);

                string fileName = await UploadFileHelper.UploadAsync($"{root}/{imageFolder}", input.File);

                input.Path = $"{imageFolder}/{fileName}";

                user.Path = input.Path;
            }
            else if (input.Path.HasValue())
            {
                input.Path = input.Path;
            }

            var check = await UserRepository.AnyAsync(x => x.Email == input.Email && x.Id != input.Id);

            if (check == true) throw new UserFriendlyException(L["DataAlreadyExists"]);

            await UserRepository.UpdateAsync(user, true);

            string domain = Configuration["Domain"];

            return input.Path.StartsWith("http") ? input.Path : $"{domain}/{input.Path}";
        }


        private string GenerateJSONWebToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("Id", user.Id.ToString()),
                new Claim("email", user.Email)
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

