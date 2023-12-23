using EUniversity.Core.Dtos.Users;
using EUniversity.Core.Filters;
using EUniversity.Core.Models;
using EUniversity.Core.Pagination;
using EUniversity.Core.Services.Users;
using EUniversity.Infrastructure.Data;
using EUniversity.Infrastructure.Pagination;

namespace EUniversity.Infrastructure.Services.Users;

/// <inheritdoc />
public class UsersService : IUsersService
{
    private readonly ApplicationDbContext _dbContext;

    public UsersService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
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
        var user = await _dbContext.Users
            .Where(u => u.Id == userId && !u.IsDeleted)
            .FirstOrDefaultAsync();
        // User does not exist(or deleted) - return false
        if (user == null) return false;

        // Set IsDeleted flag to true
        user.IsDeleted = true;
        await _dbContext.SaveChangesAsync();

        return true;
    }
}
