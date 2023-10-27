using EUniversity.Core.Pagination;
using EUniversity.Core.Validation;
using FluentValidation.TestHelper;

namespace EUniversity.Tests.Validation;

public class PaginationPropertiesValidatorTests
{
    private PaginationPropertiesValidator _validator;

    private const int ValidPage = 2;
    private const int ValidPageSize = 25;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _validator = new();
    }

    [Test]
    public void Properties_Valid_Succeeds()
    {
        // Arrange
        PaginationProperties properties = new(ValidPage, ValidPageSize);

        // Act
        var result = _validator.TestValidate(properties);

        // Arrange
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    [TestCase(0)]
    [TestCase(-3)]
    public void Page_NotPositive_FailsWithPropertyTooSmallError(int page)
    {
        // Arrange
        PaginationProperties properties = new(page, ValidPageSize);

        // Act
        var result = _validator.TestValidate(properties);

        // Arrange
        result.ShouldHaveValidationErrorFor(p => p.Page)
            .WithErrorCode(ValidationErrorCodes.PropertyTooSmall);
    }

    [Test]
    public void PageSize_Small_FailsWithPropertyTooSmallError()
    {
        // Arrange
        PaginationProperties properties = new(ValidPage, PaginationProperties.MinPageSize - 1);

        // Act
        var result = _validator.TestValidate(properties);

        // Arrange
        result.ShouldHaveValidationErrorFor(p => p.PageSize)
            .WithErrorCode(ValidationErrorCodes.PropertyTooSmall);
    }

    [Test]
    public void PageSize_Large_FailsWithPropertyTooLargeError()
    {
        // Arrange
        PaginationProperties properties = new(ValidPage, PaginationProperties.MaxPageSize + 1);

        // Act
        var result = _validator.TestValidate(properties);

        // Arrange
        result.ShouldHaveValidationErrorFor(p => p.PageSize)
            .WithErrorCode(ValidationErrorCodes.PropertyTooLarge);
    }
}
