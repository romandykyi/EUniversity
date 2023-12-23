namespace EUniversity.Core.Validation;

public static class ValidationErrorCodes
{
    public const string PropertyRequired = "PropertyRequiredError";
    public const string PropertyNull = "PropertyNullError";
    public const string PropertyTooSmall = "PropertyTooSmallError";
    public const string PropertyTooLarge = "PropertyTooLargeError";
    public const string InvalidProperty = "InvalidPropertyError";
    public const string InvalidEmail = "InvalidEmailError";
    public const string InvalidUserName = "InvalidUserNameError";
    public const string InvalidRange = "InvalidRangeError";
    public const string Equal = "EqualError";
    public const string EmptyCollection = "EmptyCollectionError";

    public const string InvalidForeignKey = "InvalidForeignKeyError";
    public const string UserIsNotInRole = "UserIsNotInRole";
}
