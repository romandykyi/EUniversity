using FluentValidation;
using EUniversity.Core.Dtos.Auth;

namespace EUniversity.Core.Validation
{
	public class ChangePasswordDtoValidator : AbstractValidator<ChangePasswordDto>
	{
		public ChangePasswordDtoValidator() 
		{ 
		}
	}
}
