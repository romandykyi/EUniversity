using EUniversity.Core.Dtos.Users;
using EUniversity.Core.Models;
using EUniversity.Core.Pagination;
using EUniversity.Core.Services;
using EUniversity.Infrastructure.Data;

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

        private static async Task<Page<UserViewDto>> SelectUsersAsync(
            IQueryable<ApplicationUser> query,
            PaginationProperties? properties)
        {
            return await query
                .ToPageAsync<ApplicationUser, UserViewDto>(properties);
        }

        /// <inheritdoc />
        public async Task<Page<UserViewDto>> GetAllUsersAsync(PaginationProperties? properties)
        {
            return await SelectUsersAsync(_dbContext.Users, properties);
        }

        /// <inheritdoc />
        public async Task<Page<UserViewDto>> GetUsersInRoleAsync(string role, PaginationProperties? properties)
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
