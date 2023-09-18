using AutoMapper;
using DemoWebApi.Dtos.Company;
using DemoWebApi.Dtos.Levels;
using DemoWebApi.Dtos.Nationality;
using DemoWebApi.Dtos.Users;
using DemoWebApi.Entities;

namespace DemoWebApi
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>();

            CreateMap<Level, LevelDto>();

            CreateMap<Nationality, NationalityDto>();

            CreateMap<Company, CompanyDto>();
        }
    }
}
