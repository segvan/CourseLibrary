using AutoMapper;
using CourseLibrary.API.Dtos;
using CourseLibrary.API.Entities;
using CourseLibrary.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
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
            return CreatedAtRoute("GetCourse", new {authorId = authorId, courseId = courseEntity.Id}, courseResult);
        }

        [HttpPut("{courseId}")]
        public IActionResult UpdateCourseForAuthor(Guid authorId, Guid courseId, CourseUpdateDto courseDto)
        {
            if (!courseLibraryRepository.AuthorExists(authorId))
            {
                return NotFound();
            }

            var courseEntity = courseLibraryRepository.GetCourse(authorId, courseId);
            if (courseEntity == null)
            {
                // return NotFound();

                courseEntity = mapper.Map<Course>(courseDto);
                courseEntity.Id = courseId;

                courseLibraryRepository.AddCourse(authorId, courseEntity);
                courseLibraryRepository.Save();

                var courseResult = mapper.Map<CourseDto>(courseEntity);
                return CreatedAtRoute("GetCourse", new {authorId = authorId, courseId = courseEntity.Id}, courseResult);
            }

            mapper.Map(courseDto, courseEntity);
            courseLibraryRepository.UpdateCourse(courseEntity);
            courseLibraryRepository.Save();

            return NoContent();
        }

        [HttpPatch("{courseId}")]
        public IActionResult PartialUpdateCourseForAuthor(Guid authorId, Guid courseId,
            JsonPatchDocument<CourseUpdateDto> coursePatch)
        {
            if (!courseLibraryRepository.AuthorExists(authorId))
            {
                return NotFound();
            }

            var courseEntity = courseLibraryRepository.GetCourse(authorId, courseId);
            if (courseEntity == null)
            {
                return NotFound();
            }

            var courseToPatch = mapper.Map<CourseUpdateDto>(courseEntity);
            // Add validation
            coursePatch.ApplyTo(courseToPatch, ModelState);
            if (!TryValidateModel(courseToPatch))
            {
                return ValidationProblem(ModelState);
            }

            mapper.Map(courseToPatch, courseEntity);
            courseLibraryRepository.UpdateCourse(courseEntity);
            courseLibraryRepository.Save();
            return NoContent();
        }

        [HttpDelete("{courseId}")]
        public ActionResult DeleteCourseForAuthor(Guid authorId, Guid courseId)
        {
            if (!courseLibraryRepository.AuthorExists(authorId))
            {
                return NotFound();
            }

            var courseEntity = courseLibraryRepository.GetCourse(authorId, courseId);
            if (courseEntity == null)
            {
                return NotFound();
            }

            courseLibraryRepository.DeleteCourse(courseEntity);
            courseLibraryRepository.Save();

            return NoContent();
        }

        public override ActionResult ValidationProblem([ActionResultObjectValue] ModelStateDictionary modelStateDictionary)
        {
            var options = HttpContext.RequestServices.GetRequiredService<IOptions<ApiBehaviorOptions>>();
            return (ActionResult)options.Value.InvalidModelStateResponseFactory(ControllerContext);
        }
    }
}