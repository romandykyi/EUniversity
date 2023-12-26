using EUniversity.Core.Dtos.University.Grades;
using EUniversity.Core.Filters;
using EUniversity.Core.Models;
using EUniversity.Core.Models.University.Grades;
using EUniversity.Core.Pagination;
using EUniversity.Core.Services.University.Grades;
using EUniversity.Infrastructure.Data;
using EUniversity.Infrastructure.Pagination;
using IdentityModel;
using Microsoft.EntityFrameworkCore.Query;

namespace EUniversity.Infrastructure.Services.University.Grades;

public class AssignedGradesService : IAssignedGradesService
{
    private readonly ApplicationDbContext _dbContext;

    public AssignedGradesService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    /// <inheritdoc />
    public async Task<Page<TViewDto>> GetPageAsync<TViewDto>(PaginationProperties properties, IFilter<AssignedGrade>? filter = null,
        bool includeGroups = true, bool includeStudents = true)
    {
        IQueryable<AssignedGrade> query = _dbContext.AssignedGrades
            .Include(ag => ag.ActivityType)
            .Include(ag => ag.Assigner)
            .Include(ag => ag.Reassigner)
            .Include(ag => ag.Grade);
        if (includeGroups)
        {
            query = query
                .Include(ag => ag.Group)
                .ThenInclude(g => g.Course)
                .ThenInclude(c => c.Semester);
        }
        if (includeStudents)
        {
            query = query.Include(ag => ag.Student);
        }
        query = query.AsNoTracking();

        // Optionally apply a filter
        if (filter != null) query = filter.Apply(query);

        return await query.ToPageAsync<AssignedGrade, TViewDto>(properties);
    }


    /// <inheritdoc />
    public async Task<AssignedGrade> AssignAsync(AssignedGradeCreateDto dto, string assignerId)
    {
        var assignedGrade = dto.Adapt<AssignedGrade>();
        // Set a creation date
        assignedGrade.CreationDate = DateTimeOffset.Now;
        // Set an update date
        assignedGrade.UpdateDate = DateTimeOffset.Now;
        // Set an assigner
        assignedGrade.AssignerId = assignerId;

        _dbContext.Add(assignedGrade);
        await _dbContext.SaveChangesAsync();

        return assignedGrade;
    }

    /// <inheritdoc />
    public async Task<bool> ReassignAsync(int id, AssignedGradeUpdateDto dto, string reassignerId)
    {
        // Try to find an existing assigned grade
        var assignedGrade = await _dbContext.AssignedGrades
            .FirstOrDefaultAsync(e => e.Id.Equals(id));
        // If it doesn't exist then return false
        if (assignedGrade == null) return false;

        // Update the grade
        dto.Adapt(assignedGrade);
        // Set a reassigner
        assignedGrade.ReassignerId = reassignerId;
        // Set an update date
        assignedGrade.UpdateDate = DateTimeOffset.Now;
        _dbContext.Update(assignedGrade);
        await _dbContext.SaveChangesAsync();

        return true;
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAsync(int id)
    {
        // Find an assigned grade by its ID
        var assignedGrade = await _dbContext.AssignedGrades
            .FirstOrDefaultAsync(e => e.Id.Equals(id));
        // If it does not exist then return false
        if (assignedGrade == null) return false;

        // Remove entity
        _dbContext.Remove(assignedGrade);
        await _dbContext.SaveChangesAsync();

        return true;
    }

    /// <inheritdoc />
    public async Task<GetAssignerIdResponse> GetAssignerIdAsync(int id)
    {
        var grade = await _dbContext.AssignedGrades
            .AsNoTracking()
            .Where(g => g.Id == id)
            .FirstOrDefaultAsync();
        if (grade == null)
        {
            return new(false, null);
        }
        return new(true, grade.AssignerId);
    }
}
