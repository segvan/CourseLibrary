using AutoMapper;
using CourseLibrary.API.Dtos;
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
        public ActionResult<IEnumerable<AuthorDto>> GetAll()
        {
            var authorsList = courseLibraryRepository.GetAuthors();
            var authorsListDto = mapper.Map<IEnumerable<AuthorDto>>(authorsList);
            return Ok(authorsListDto);
        }

        [HttpGet("{id:guid}")]
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
    }
}