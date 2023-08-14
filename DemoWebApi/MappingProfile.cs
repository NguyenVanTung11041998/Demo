using AutoMapper;
using DemoWebApi.Dtos.Users;
using DemoWebApi.Entities;

namespace DemoWebApi
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>();
        }
    }
}
