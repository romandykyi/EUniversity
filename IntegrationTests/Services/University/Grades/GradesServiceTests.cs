using EUniversity.Core.Dtos.University.Grades;
using EUniversity.Core.Models.University.Grades;
using EUniversity.Infrastructure.Services.University;

namespace EUniversity.IntegrationTests.Services.University.Grades
{
    public class GradesServiceTests : CrudServicesTest
        <IGradesService, Grade, int, ViewGradeDto, ViewGradeDto, CreateGradeDto, CreateGradeDto>
    {
        /// <inheritdoc />
        protected override void AssertThatWasUpdated(Grade actualEntity, CreateGradeDto updateDto)
        {
            Assert.Multiple(() =>
            {
                Assert.That(actualEntity.Name, Is.EqualTo(updateDto.Name));
                Assert.That(actualEntity.Score, Is.EqualTo(updateDto.Score));
            });
        }

        /// <inheritdoc />
        protected override int GetNonExistentId() => -1;

        /// <inheritdoc />
        protected override Grade GetTestEntity()
        {
            return new()
            {
                Name = "3",
                Score = 3
            };
        }

        /// <inheritdoc />
        protected override CreateGradeDto GetValidCreateDto()
        {
            return new("5", 5);
        }

        /// <inheritdoc />
        protected override CreateGradeDto GetValidUpdateDto()
        {
            return new("4", 4);
        }
    }
}
