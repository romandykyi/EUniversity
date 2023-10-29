using EUniversity.Core.Dtos.University;
using EUniversity.Core.Models.University;
using EUniversity.Core.Services.University;
using EUniversity.Infrastructure.Data;

namespace EUniversity.Infrastructure.Services.University;

public class GroupsService :
    BaseCrudService<Group, int, GroupPreviewDto, GroupViewDto, GroupCreateDto, GroupCreateDto>,
    IGroupsService
{
    protected override IQueryable<Group> GetByIdQuery =>
        Entities
        .Include(e => e.Course)
        .Include(e => e.Teacher)
        .Include(e => e.Students)
        .AsNoTracking();

    protected override IQueryable<Group> GetPageQuery =>
        Entities
        .Include(e => e.Course)
        .Include(e => e.Teacher)
        .AsNoTracking();

    public GroupsService(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    /// <inheritdoc />
    public Task<bool> AddStudent(StudentGroupDto dto)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public Task<bool> RemoveStudent(StudentGroupDto dto)
    {
        throw new NotImplementedException();
    }
}
