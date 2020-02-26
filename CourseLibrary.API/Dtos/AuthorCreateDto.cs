using System;

namespace CourseLibrary.API.Dtos
{
    public class AuthorCreateDto
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTimeOffset DateOfBirth { get; set; }

        public string MainCategory { get; set; }
    }
}
