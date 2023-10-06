using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using EUniversity.Core.Models;
using IdentityModel;
using Microsoft.AspNetCore.Identity;

namespace EUniversity.Infrastructure.Identity
{
    public class CustomProfileService : IProfileService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public CustomProfileService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var user = context.Subject;
            HashSet<string> claimsTypes = new()
            {
                JwtClaimTypes.Subject, JwtClaimTypes.Name, JwtClaimTypes.Email,
                JwtClaimTypes.Role, JwtClaimTypes.GivenName,
                JwtClaimTypes.MiddleName, JwtClaimTypes.FamilyName
            };
            var claims = user.Claims.Where(c => claimsTypes.Contains(c.Type));
            context.IssuedClaims.AddRange(claims);

            return Task.CompletedTask;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var user = await _userManager.FindByIdAsync(context.Subject.GetSubjectId());

            if (user != null)
            {
                context.IsActive = true;
            }
            else
            {
                // The user is not found, so consider them as not active
                context.IsActive = false;
            }
        }
    }
}
