using EUniversity.Core.Dtos.Users;
using EUniversity.Core.Models;
using EUniversity.Core.Services;
using EUniversity.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;

namespace EUniversity.Infrastructure.Services
{
    /// <inheritdoc />
    public class UsersService : IUsersService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _dbContext;

        public UsersService(UserManager<ApplicationUser> userManager,
            ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        /// <inheritdoc />
        public async Task<UsersViewDto> GetAllUsersAsync()
        {
            var users = await _dbContext.Users
                .ProjectToType<UserViewDto>()
                .ToListAsync();
            return new(users);
        }

        /// <inheritdoc />
        public async Task<UsersViewDto> GetUsersInRoleAsync(string role)
        {
            var users = await _userManager.GetUsersInRoleAsync(role);
            var result = users.Adapt<List<UserViewDto>>();
            return new(result);
        }
    }
}
