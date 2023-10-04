using Duende.IdentityServer.Extensions;
using EUniversity.Core.Models;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace EUniversity.Infrastructure.Identity
{
    public class CustomClaimsTransform : IClaimsTransformation
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public CustomClaimsTransform(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var user = await _userManager.FindByIdAsync(principal.GetSubjectId());
            if (user == null) return principal;

            var claimsIdentity = (ClaimsIdentity)principal.Identity!;
            claimsIdentity.AddClaim(new(JwtClaimTypes.GivenName, user.FirstName));
            if (user.MiddleName != null)
            {
                claimsIdentity.AddClaim(new(JwtClaimTypes.MiddleName, user.MiddleName));
            }
            claimsIdentity.AddClaim(new(JwtClaimTypes.FamilyName, user.LastName));

            return principal;
        }
    }
}
