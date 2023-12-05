using EUniversity.Core.Dtos.University;
using EUniversity.Core.Models.University;
using EUniversity.Core.Services.University;
using EUniversity.Infrastructure.Data;

namespace EUniversity.Infrastructure.Services.University;

/// <inheritdoc />
public class ClassTypesService :
    BaseCrudService<ClassType, int, ClassTypeViewDto, ClassTypeViewDto, ClassTypeCreateDto, ClassTypeCreateDto>,
    IClassTypesService
{
    public ClassTypesService(ApplicationDbContext dbContext) : base(dbContext) { }
}
