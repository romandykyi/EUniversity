using EUniversity.Core.Dtos.Users;
using EUniversity.Core.Models;
using EUniversity.Core.Validation;
using EUniversity.Core.Validation.Users;
using FluentValidation.TestHelper;

namespace EUniversity.Tests.Validation.Users;

public class EditUserDtoValidatorTests
{
    private EditUserDtoValidator _validator;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _validator = new EditUserDtoValidator();
    }

    [Test]
    [TestCase("12345")]
    [TestCase("waltuh")]
    [TestCase("jp-900")]
    [TestCase("Saul_Go0dmaN.CrimiNal-LAWyer")]
    public void UserName_Valid_Succeeds(string userName)
    {
        // Arrange
        EditUserDto dto = new(userName, RegisterDtoValidatorTests.DefaultEmail,
            RegisterDtoValidatorTests.DefaultFirstName, RegisterDtoValidatorTests.DefaultLastName,
            RegisterDtoValidatorTests.DefaultMiddleName);

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    [TestCase("Invąlid")]
    [TestCase("SomethingIs300$")]
    [TestCase("nOS_Svnm+-/*")]
    [TestCase("I have spaces")]
    [TestCase("IWantToBeValid!?")]
    public void UserName_ContainsInvalidCharacters_FailsWithInvalidUserNameError(string userName)
    {
        // Arrange
        EditUserDto dto = new(userName, RegisterDtoValidatorTests.DefaultEmail,
            RegisterDtoValidatorTests.DefaultFirstName, RegisterDtoValidatorTests.DefaultLastName,
            RegisterDtoValidatorTests.DefaultMiddleName);

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.UserName)
            .WithErrorCode(ValidationErrorCodes.InvalidUserName)
            .Only();
    }

    [Test]
    public void UserName_Empty_FailsWithPropertyRequiredError()
    {
        // Arrange
        EditUserDto dto = new(string.Empty, RegisterDtoValidatorTests.DefaultEmail,
            RegisterDtoValidatorTests.DefaultFirstName, RegisterDtoValidatorTests.DefaultLastName,
            RegisterDtoValidatorTests.DefaultMiddleName);

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.UserName)
            .WithErrorCode(ValidationErrorCodes.PropertyRequired)
            .Only();
    }

    [Test]
    public void UserName_TooLarge_FailsWithPropertyTooLargeError()
    {
        // Arrange
        string largeUserName = new('0', ApplicationUser.MaxUserNameLength + 1);
        EditUserDto dto = new(largeUserName, RegisterDtoValidatorTests.DefaultEmail,
            RegisterDtoValidatorTests.DefaultFirstName, RegisterDtoValidatorTests.DefaultLastName,
            RegisterDtoValidatorTests.DefaultMiddleName);

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.UserName)
            .WithErrorCode(ValidationErrorCodes.PropertyTooLarge)
            .Only();
    }
}
