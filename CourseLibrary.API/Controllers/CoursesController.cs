using AutoMapper;
using CourseLibrary.API.Dtos;
using CourseLibrary.API.Entities;
using CourseLibrary.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace CourseLibrary.API.Controllers
{
    [Route("api/authors/{authorId}/courses")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseLibraryRepository courseLibraryRepository;
        private readonly IMapper mapper;

        public CoursesController(ICourseLibraryRepository courseLibraryRepository, IMapper mapper)
        {
            this.courseLibraryRepository = courseLibraryRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        [HttpHead]
        public ActionResult<IEnumerable<CourseDto>> GetCourses(Guid authorId)
        {
            if (!courseLibraryRepository.AuthorExists(authorId))
            {
                return NotFound();
            }

            var coursesList = courseLibraryRepository.GetCourses(authorId);
            var coursesListDto = mapper.Map<IEnumerable<CourseDto>>(coursesList);
            return Ok(coursesListDto);
        }


        [HttpGet("{courseId}", Name = "GetCourse")]
        [HttpHead("{courseId}")]
        public ActionResult<CourseDto> GetCourse(Guid authorId, Guid courseId)
        {
            if (!courseLibraryRepository.AuthorExists(authorId))
            {
                return NotFound();
            }

            var course = courseLibraryRepository.GetCourse(authorId, courseId);

            if (course == null)
            {
                return NotFound();
            }

            var courseDto = mapper.Map<CourseDto>(course);
            return Ok(courseDto);
        }

        [HttpPost]
        public ActionResult<CourseDto> CreateCourseForAuthor(Guid authorId, CourseCreateDto courseDto)
        {
            if (!courseLibraryRepository.AuthorExists(authorId))
            {
                return NotFound();
            }

            var courseEntity = mapper.Map<Course>(courseDto);
            courseLibraryRepository.AddCourse(authorId, courseEntity);
            courseLibraryRepository.Save();

            var courseResult = mapper.Map<CourseDto>(courseEntity);
            return CreatedAtRoute("GetCourse", new { authorId = authorId, courseId = courseEntity.Id }, courseResult);
        }
    }
}