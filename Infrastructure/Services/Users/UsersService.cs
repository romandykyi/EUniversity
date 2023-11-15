﻿using EUniversity.Core.Dtos.Users;
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

    private static async Task<Page<UserViewDto>> SelectUsersAsync(
        IQueryable<ApplicationUser> query,
        PaginationProperties? properties,
        IFilter<ApplicationUser>? filter)
    {
        query = filter?.Apply(query) ?? query;
        return await query
            .AsNoTracking()
            .ToPageAsync<ApplicationUser, UserViewDto>(properties);
    }

    /// <inheritdoc />
    public async Task<Page<UserViewDto>> GetAllUsersAsync(PaginationProperties? properties,
        IFilter<ApplicationUser>? filter = null)
    {
        return await SelectUsersAsync(_dbContext.Users, properties, filter);
    }

    /// <inheritdoc />
    public async Task<Page<UserViewDto>> GetUsersInRoleAsync(string role, PaginationProperties? properties,
        IFilter<ApplicationUser>? filter = null)
    {
        string? roleId = await _dbContext.Roles
            .Where(r => r.Name == role)
            .Select(r => r.Id)
            .FirstOrDefaultAsync() ??
            throw new InvalidOperationException($"Role {role} doesn't exists");

        var users = _dbContext.UserRoles
            .Where(r => r.RoleId == roleId)
            .Join(_dbContext.Users, r => r.UserId, u => u.Id, (r, u) => u);
        return await SelectUsersAsync(users, properties, filter);
    }
}
