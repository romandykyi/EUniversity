using EUniversity.Core.Dtos.University;
using EUniversity.Core.Models.University;
using EUniversity.Core.Services.University;
using Mapster;

namespace EUniversity.IntegrationTests.Services.University;

public class SemestersServiceTests :
    CrudServicesTest<ISemestersService, Semester, int, SemesterPreviewDto, SemesterViewDto, SemesterCreateDto, SemesterCreateDto>
{
    protected override void AssertThatWasUpdated(Semester actualEntity, SemesterCreateDto updateDto)
    {
        Assert.Multiple(() =>
        {
            Assert.That(actualEntity.Name, Is.EqualTo(updateDto.Name));
            Assert.That(actualEntity.DateFrom, Is.EqualTo(updateDto.DateFrom));
            Assert.That(actualEntity.DateTo, Is.EqualTo(updateDto.DateTo));
        });
    }

    protected override int GetNonExistentId()
    {
        return -1;
    }

    protected override Semester GetTestEntity()
    {
        return GetTestSemester();
    }

    protected override SemesterCreateDto GetValidCreateDto()
    {
        return new SemesterCreateDto("Semester II", DateTimeOffset.MinValue, DateTimeOffset.MaxValue);
    }

    protected override SemesterCreateDto GetValidUpdateDto()
    {
        return new SemesterCreateDto("Semester III", DateTimeOffset.Now, DateTimeOffset.Now + TimeSpan.FromDays(10));
    }

    internal static Semester GetTestSemester()
    {
        return new Semester()
        {
            Name = "Semester I",
            DateFrom = DateTimeOffset.MinValue,
            DateTo = DateTimeOffset.MaxValue,
            StudentEnrollments = new List<StudentSemester>()
        };
    }


    [Test]
    public override async Task GetById_ElementExists_ReturnsValidElement()
    {
        // Arrange
        var semester = await CreateTestEntityAsync();
        var expectedResult = semester.Adapt<SemesterViewDto>();

        // Act
        var result = await Service.GetByIdAsync(semester.Id);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(expectedResult.Id));
            Assert.That(result.Name, Is.EqualTo(expectedResult.Name));
            Assert.That(result.DateFrom, Is.EqualTo(expectedResult.DateFrom));
            Assert.That(result.DateTo, Is.EqualTo(expectedResult.DateTo));
            Assert.That(result.StudentEnrollments, Is.EquivalentTo(expectedResult.StudentEnrollments));
        });
    }
}
