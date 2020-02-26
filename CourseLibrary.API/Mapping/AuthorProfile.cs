using AutoMapper;
using CourseLibrary.API.Dtos;
using CourseLibrary.API.Entities;
using CourseLibrary.API.Extensions;

namespace CourseLibrary.API.Mapping
{
    public class AuthorProfile : Profile
    {
        public AuthorProfile()
        {
            CreateMap<Author, AuthorDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(x => $"{x.FirstName} {x.LastName}"))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(x => x.DateOfBirth.GetCurrentAge()));

            CreateMap<AuthorCreateDto, Author>();
        }
    }
}
