using EUniversity.Core.Dtos.University;
using EUniversity.Core.Models.University;
using EUniversity.Core.Validation.University;
using EUniversity.Core.Validation;
using FluentValidation.TestHelper;

namespace EUniversity.Tests.Validation.University;

public class SemesterCreateDtoValidatorTests
{
    private SemesterCreateDtoValidator _validator;

    private const string DefaultName = "Semester I";
    private readonly DateTimeOffset DefaultDateFrom = DateTimeOffset.Now + TimeSpan.FromDays(14);
    private readonly DateTimeOffset DefaultDateTo = DateTimeOffset.Now + TimeSpan.FromDays(14 + 30 * 5);

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _validator = new();
    }

    [Test]
    public void Dto_Valid_Succeeds()
    {
        // Arrange
        SemesterCreateDto dto = new(DefaultName, DefaultDateFrom, DefaultDateTo);

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void Name_TooLarge_FailsWithPropertyTooLargeError()
    {
        // Arrange
        SemesterCreateDto dto = new(new string('0', Semester.MaxNameLength + 1), DefaultDateFrom, DefaultDateTo);

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(dto => dto.Name)
            .WithErrorCode(ValidationErrorCodes.PropertyTooLarge);
    }

    [Test]
    public void Name_Empty_FailsWithPropertyRequiredError()
    {
        // Arrange
        SemesterCreateDto dto = new(string.Empty, DefaultDateFrom, DefaultDateTo);

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(dto => dto.Name)
            .WithErrorCode(ValidationErrorCodes.PropertyRequired);
    }

    [Test]
    public void DateFrom_GreaterThanDateTo_FailsWithInvalidRangeError()
    {
        // Arrange
        DateTimeOffset dateFrom = DateTimeOffset.Now + TimeSpan.FromDays(30);
        DateTimeOffset dateTo = dateFrom - TimeSpan.FromDays(5);
        SemesterCreateDto dto = new(string.Empty, dateFrom, dateTo);

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveAnyValidationError()
            .WithErrorCode(ValidationErrorCodes.InvalidRange);
    }

    [Test]
    public void DateTo_LessThanDateNow_FailsWithInvalidRangeError()
    {
        // Arrange
        DateTimeOffset dateFrom = DateTimeOffset.Now - TimeSpan.FromDays(30);
        DateTimeOffset dateTo = dateFrom + TimeSpan.FromDays(15);
        SemesterCreateDto dto = new(string.Empty, dateFrom, dateTo);

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(dto => dto.DateTo)
            .WithErrorCode(ValidationErrorCodes.InvalidRange);
    }
}
