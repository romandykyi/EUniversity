using EUniversity.Core.Pagination;
using FluentValidation;

namespace EUniversity.Core.Validation
{
    public class PaginationPropertiesValidator : AbstractValidator<PaginationProperties>
    {
        private readonly string RangeErrorMessage =
            $"Page size must be within the range from " +
            $"{PaginationProperties.MinPageSize} to {PaginationProperties.MaxPageSize}";

        public PaginationPropertiesValidator()
        {
            RuleFor(p => p.Page)
                .GreaterThan(0)
                .WithErrorCode(ValidationErrorCodes.PropertyTooSmall)
                .WithMessage("Page number must be positive");

            RuleFor(p => p.PageSize)
                .GreaterThanOrEqualTo(PaginationProperties.MinPageSize)
                .WithErrorCode(ValidationErrorCodes.PropertyTooSmall)
                .WithMessage(RangeErrorMessage);

            RuleFor(p => p.PageSize)
                .LessThanOrEqualTo(PaginationProperties.MaxPageSize)
                .WithErrorCode(ValidationErrorCodes.PropertyTooLarge)
                .WithMessage(RangeErrorMessage);
        }
    }
}
