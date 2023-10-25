using EUniversity.Core.Dtos.University.Grades;
using EUniversity.Core.Models.University.Grades;
using EUniversity.Infrastructure.Services.University;

namespace EUniversity.IntegrationTests.Services.University.Grades;

public class GradesServiceTests : CrudServicesTest
    <IGradesService, Grade, int, GradeViewDto, GradeViewDto, GradeCreateDto, GradeCreateDto>
{
    /// <inheritdoc />
    protected override void AssertThatWasUpdated(Grade actualEntity, GradeCreateDto updateDto)
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
    protected override GradeCreateDto GetValidCreateDto()
    {
        return new("5", 5);
    }

    /// <inheritdoc />
    protected override GradeCreateDto GetValidUpdateDto()
    {
        return new("4", 4);
    }
}
