using AutoMapper;
using CourseLibrary.API.Dtos;
using CourseLibrary.API.Entities;
using CourseLibrary.API.ResourceParameters;
using CourseLibrary.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace CourseLibrary.API.Controllers
{
    [Route("api/authors")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly ICourseLibraryRepository courseLibraryRepository;
        private readonly IMapper mapper;

        public AuthorsController(ICourseLibraryRepository courseLibraryRepository, IMapper mapper)
        {
            this.courseLibraryRepository = courseLibraryRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        [HttpHead]
        public ActionResult<IEnumerable<AuthorDto>> GetAll([FromQuery]AuthorResourceParameters authorResourceParameters)
        {
            var authorsList = courseLibraryRepository.GetAuthors(authorResourceParameters);
            var authorsListDto = mapper.Map<IEnumerable<AuthorDto>>(authorsList);
            return Ok(authorsListDto);
        }

        [HttpGet("{id:guid}", Name = "GetAuthor")]
        [HttpHead("{id:guid}")]
        public ActionResult<AuthorDto> GetById(Guid id)
        {
            var author = courseLibraryRepository.GetAuthor(id);

            if (author == null)
            {
                return NotFound();
            }

            var authorDto = mapper.Map<AuthorDto>(author);
            return Ok(authorDto);
        }

        [HttpPost]
        public ActionResult<AuthorDto> Create(AuthorCreateDto author)
        {
            // no need to check for null

            var authorEntity = mapper.Map<Author>(author);
            courseLibraryRepository.AddAuthor(authorEntity);
            courseLibraryRepository.Save();

            var authorResult = mapper.Map<AuthorDto>(authorEntity);
            return CreatedAtRoute("GetAuthor", new { id = authorResult.Id }, authorResult);
        }

        [HttpPost]
        public ActionResult<IEnumerable<AuthorDto>> CreateCollection(IEnumerable<AuthorCreateDto> authorsList)
        {
            var authorEntityList = mapper.Map<IEnumerable<Author>>(authorsList);
            foreach (var author in authorEntityList)
            {
                courseLibraryRepository.AddAuthor(author);
            }
            
            courseLibraryRepository.Save();

            return Ok();
        }

        // array key: 1,2,3 - needs custom model binder
        // composite key: key1=value1,key2=value2 - two parameters in method

        //[HttpGet("({ids})")]
        //public IActionResult GetAuthorsCollection([FromRoute] IEnumerable<Guid> ids)
        //{

        //}

        [HttpOptions]
        public IActionResult GetOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS,POST");
            return Ok();
        }
    }
}