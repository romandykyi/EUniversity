using EUniversity.Core.Dtos.University;
using EUniversity.Core.Models.University;
using EUniversity.Core.Services.University;

namespace EUniversity.IntegrationTests.Services.University
{
    public class CoursesServiceTests :
        CrudServicesTest<ICoursesService, Course, int, PreviewCourseDto, ViewCourseDto, CreateCourseDto, CreateCourseDto>
    {
        /// <inheritdoc />
        protected override void AssertThatWasUpdated(Course actualEntity, CreateCourseDto updateDto)
        {
            Assert.Multiple(() =>
            {
                Assert.That(actualEntity.Name, Is.EqualTo(updateDto.Name));
                Assert.That(actualEntity.Description, Is.EqualTo(updateDto.Description));
            });
        }

        /// <inheritdoc />
        protected override int GetNonExistentId()
        {
            return -1;
        }

        /// <inheritdoc />
        protected override Course GetTestEntity()
        {
            return new()
            {
                Name = "Chemistry",
                Description = "Walter's White cooking course"
            };
        }

        /// <inheritdoc />
        protected override CreateCourseDto GetValidCreateDto()
        {
            return new("Physics", "g/(pi^2) = 1");
        }

        /// <inheritdoc />
        protected override CreateCourseDto GetValidUpdateDto()
        {
            return new("Math", "Not to be confused with meth");
        }
    }
}
