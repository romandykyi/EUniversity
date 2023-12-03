using EUniversity.Core.Dtos.University;
using EUniversity.Core.Models.University;

namespace EUniversity.Core.Services.University;

public interface IClassesService :
    ICrudService<Class, int, ClassViewDto, ClassViewDto, ClassCreateDto, ClassUpdateDto>
{
}
