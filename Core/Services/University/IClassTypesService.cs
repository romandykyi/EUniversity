using EUniversity.Core.Dtos.University;
using EUniversity.Core.Models.University;

namespace EUniversity.Core.Services.University;

public interface IClassTypesService :
    ICrudService<ClassType, int, ClassTypeViewDto, ClassTypeViewDto, ClassTypeCreateDto, ClassTypeCreateDto>
{
}
