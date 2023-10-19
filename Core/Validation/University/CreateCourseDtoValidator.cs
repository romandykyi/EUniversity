using EUniversity.Core.Dtos.University;
using FluentValidation;

namespace EUniversity.Core.Validation.University
{
    public class CreateCourseDtoValidator : AbstractValidator<CreateCourseDto>
    {
        public CreateCourseDtoValidator() 
        { 
        }
    }
}
