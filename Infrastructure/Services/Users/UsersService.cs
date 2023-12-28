using EUniversity.Core.Dtos.University;
using EUniversity.Core.Dtos.Users;
using EUniversity.Core.Filters;
using EUniversity.Core.Models;
using EUniversity.Core.Models.University;
using EUniversity.Core.Pagination;
using EUniversity.Core.Policy;
using EUniversity.Core.Services.Users;
using EUniversity.Infrastructure.Data;
using EUniversity.Infrastructure.Pagination;
using Microsoft.AspNetCore.Identity;

namespace EUniversity.Infrastructure.Services.Users;

/// <inheritdoc />
public class UsersService : IUsersService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;

    public UsersService(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
    }

    private static async Task<Page<UserPreviewDto>> SelectUsersAsync(
        IQueryable<ApplicationUser> query,
        PaginationProperties? properties,
        IFilter<ApplicationUser>? filter,
        bool onlyDeleted = false)
    {
        query = query.AsNoTracking()
            .Where(u => u.IsDeleted == onlyDeleted);
        query = filter?.Apply(query) ?? query;

        return await query.ToPageAsync<ApplicationUser, UserPreviewDto>(properties);
    }

    private async Task<ApplicationUser?> GetExistingUserById(string id)
    {
        return await _dbContext.Users
            .Where(u => u.Id == id && !u.IsDeleted)
            .FirstOrDefaultAsync();
    }

    /// <inheritdoc />
    public async Task<Page<UserPreviewDto>> GetAllUsersAsync(PaginationProperties? properties,
        IFilter<ApplicationUser>? filter = null, bool onlyDeleted = false)
    {
        return await SelectUsersAsync(_dbContext.Users, properties, filter, onlyDeleted);
    }

    /// <inheritdoc />
    public async Task<Page<UserPreviewDto>> GetUsersInRoleAsync(string role, PaginationProperties? properties,
        IFilter<ApplicationUser>? filter = null)
    {
        string? roleId = await _dbContext.Roles
            .Where(r => r.Name == role)
            .Select(r => r.Id)
            .FirstOrDefaultAsync() ??
            throw new InvalidOperationException($"Role {role} doesn't exists");

        var users = _dbContext.UserRoles
            .AsNoTracking()
            .Where(ur => ur.RoleId == roleId)
            .Join(_dbContext.Users, r => r.UserId, u => u.Id, (ur, u) => u);
        return await SelectUsersAsync(users, properties, filter, false);
    }

    /// <inheritdoc />
    public async Task<UserViewDto?> GetByIdAsync(string id)
    {
        var user = await _dbContext.Users
            .AsNoTracking()
            .Where(u => u.Id == id)
            .FirstOrDefaultAsync();
        if (user == null) return null;

        var dto = user.Adapt<UserViewDto>();
        // Select user's roles
        dto.Roles = await _dbContext.UserRoles
            .AsNoTracking()
            .Where(ur => ur.UserId == id)
            .Join(_dbContext.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r)
            .Select(r => r.Name!)
            .ToListAsync();
        return dto;
    }

    /// <inheritdoc />
    public async Task<bool> DeleteUserAsync(string userId)
    {
        // Find a user by its ID
        var user = await GetExistingUserById(userId);
        // User does not exist(or deleted) - return false
        if (user == null) return false;

        // Set IsDeleted flag to true
        user.IsDeleted = true;
        await _dbContext.SaveChangesAsync();

        return true;
    }

    /// <inheritdoc />
    public async Task<bool> UpdateUserAsync(string userId, EditUserDto editUserDto)
    {
        // Find a user by its ID
        var user = await GetExistingUserById(userId);
        // User does not exist(or deleted) - return false
        if (user == null) return false;

        // Update the user
        editUserDto.Adapt(user);
        _dbContext.Update(user);
        await _dbContext.SaveChangesAsync();

        return true;
    }

    /// <inheritdoc />
    public async Task<bool> UpdateUserRolesAsync(string userId, ChangeRolesDto dto)
    {
        // Find a user by its ID
        var user = await GetExistingUserById(userId);
        // User does not exist(or deleted) - return false
        if (user == null) return false;

        // Needs refactor if many roles will be added
        if (dto.IsTeacher == true)
        {
            await _userManager.AddToRoleAsync(user, Roles.Teacher);
        }
        else if (dto.IsTeacher == false)
        {
            await _userManager.RemoveFromRoleAsync(user, Roles.Teacher);
        }
        if (dto.IsAdministrator == true)
        {
            await _userManager.AddToRoleAsync(user, Roles.Administrator);
        }
        else if (dto.IsAdministrator == false)
        {
            await _userManager.RemoveFromRoleAsync(user, Roles.Administrator);
        }
        if (dto.IsStudent == true)
        {
            await _userManager.AddToRoleAsync(user, Roles.Student);
        }
        else if (dto.IsStudent == false)
        {
            await _userManager.RemoveFromRoleAsync(user, Roles.Student);
        }

        return true;
    }

    /// <inheritdoc />
    public async Task<Page<GroupPreviewDto>> GetGroupsOfStudentAsync(string studentId, PaginationProperties properties, IFilter<Group>? filter = null)
    {
        var groups = _dbContext.StudentGroups
            // Include dependencies of a group
            .Include(g => g.Group)
            .ThenInclude(g => g.Teacher)
            .Include(g => g.Group)
            .ThenInclude(g => g.Course)
            .ThenInclude(c => c.Semester)
            .AsNoTracking()
            // Select group enrollments of the student
            .Where(sg => sg.StudentId == studentId)
            // Select groups
            .Select(sg => sg.Group);

        // Apply a filter if it's available
        if (filter != null) groups = filter.Apply(groups);

        // Apply pagination and project to DTO
        return await groups.ToPageAsync<Group, GroupPreviewDto>(properties);
    }

    /// <inheritdoc />
    public async Task<Page<SemesterPreviewDto>> GetSemestersOfStudentAsync(string studentId, PaginationProperties properties, IFilter<Semester>? filter = null)
    {
        var semesters = _dbContext.StudentSemesters
            // Select semester enrollments of the student
            .Where(sg => sg.StudentId == studentId)
            // Select semesters
            .Select(sg => sg.Semester);

        // Apply a filter if it's available
        if (filter != null) semesters = filter.Apply(semesters);

        // Apply pagination and project to DTO
        return await semesters.ToPageAsync<Semester, SemesterPreviewDto>(properties);
    }
}
