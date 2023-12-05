using EUniversity.Core.Dtos.University;
using EUniversity.Core.Models.University;

namespace EUniversity.Core.Services.University;

/// <summary>
/// Service for classes.
/// </summary>
public interface IClassesService :
    ICrudService<Class, int, ClassViewDto, ClassViewDto, ClassCreateDto, ClassUpdateDto>
{

}
