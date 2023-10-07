using EUniversity.Core.Dtos.Users;
using EUniversity.Core.Models;
using EUniversity.Core.Pagination;
using EUniversity.Core.Services;
using EUniversity.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;

namespace EUniversity.Infrastructure.Services
{
    /// <inheritdoc />
    public class UsersService : IUsersService
    {
        private readonly ApplicationDbContext _dbContext;

        public UsersService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private static async Task<IEnumerable<UserViewDto>> SelectUsersAsync(
            IQueryable<ApplicationUser> query,
            PaginationProperties? properties)
        {
            return await query
                .ApplyPagination(properties)
                .ProjectToType<UserViewDto>()
                .ToListAsync();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<UserViewDto>> GetAllUsersAsync(PaginationProperties? properties)
        {
            return await SelectUsersAsync(_dbContext.Users, properties);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<UserViewDto>> GetUsersInRoleAsync(string role, PaginationProperties? properties)
        {
            string? roleId = await _dbContext.Roles
                .Where(r => r.Name == role)
                .Select(r => r.Id)
                .FirstOrDefaultAsync() ?? 
                throw new InvalidOperationException($"Role {role} doesn't exists");

            var users = _dbContext.UserRoles
                .Where(r => r.RoleId == roleId)
                .Join(_dbContext.Users, r => r.UserId, u => u.Id, (r, u) => u);
            return await SelectUsersAsync(users, properties);
        }
    }
}
