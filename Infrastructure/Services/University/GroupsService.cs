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
    public async Task<bool> AddStudentAsync(StudentGroupDto dto)
    {
        // Check if student is already in the group
        bool studentIsInGroup = await DbContext.StudentGroups
            .AnyAsync(sg => sg.StudentId == dto.StudentId && dto.GroupId == dto.GroupId);
        if (studentIsInGroup)
        {
            return false;
        }

        // Add a student to the group
        var studentGroup = dto.Adapt<StudentGroup>();
        DbContext.StudentGroups.Add(studentGroup);
        await DbContext.SaveChangesAsync();

        return true;
    }

    /// <inheritdoc />
    public async Task<bool> RemoveStudentAsync(StudentGroupDto dto)
    {
        // Check if student is in the group
        var studentGroup = await DbContext.StudentGroups
            .FirstOrDefaultAsync(
             sg => sg.StudentId == dto.StudentId && dto.GroupId == dto.GroupId
            );
        // User is not in the group
        if (studentGroup == null) return false;

        // Remove student from the group
        DbContext.Remove(studentGroup);
        await DbContext.SaveChangesAsync();

        return true;
    }
}
