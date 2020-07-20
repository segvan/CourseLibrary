using AutoMapper;
using CourseLibrary.API.Dtos;
using CourseLibrary.API.Entities;

namespace CourseLibrary.API.Mapping
{
    public class CourseProfile : Profile
    {
        public CourseProfile()
        {
            CreateMap<Course, CourseDto>();
            CreateMap<CourseCreateDto, Course>();
            CreateMap<CourseUpdateDto, Course>();
        }
    }
}
