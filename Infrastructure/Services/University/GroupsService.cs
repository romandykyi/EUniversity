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
    public async Task<GetOwnerIdResponse> GetOwnerIdAsync(int groupId)
    {
        Group? group =  await GetByIdQuery
            .Where(g => g.Id == groupId)
            .FirstOrDefaultAsync();
        if (group == null)
        {
            return new(false, null);
        }
        return new(true, group.TeacherId);
    }
}
