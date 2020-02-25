using System;

namespace CourseLibrary.API.Extensions
{
    public static class DateTimeOffsetExtensions
    {
        public static int GetCurrentAge(this DateTimeOffset dateOfBirth)
        {
            var nowUtc = DateTime.UtcNow;
            var dateOfBirthUtc = dateOfBirth.UtcDateTime;
            var years = nowUtc.Year - dateOfBirthUtc.Year;
            if (nowUtc < dateOfBirthUtc.AddYears(years))
            {
                years--;
            }

            return years;
        }
    }
}
