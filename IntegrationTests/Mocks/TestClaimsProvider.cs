using IdentityModel;
using System.Security.Claims;

namespace EUniversity.IntegrationTests.Mocks
{
	public class TestClaimsProvider
	{
		public IList<Claim> Claims { get; }

		public TestClaimsProvider()
		{
			Claims = new List<Claim>();
		}

		public void Init(string? id, string name, params string[] roles)
		{
			Claims.Clear();
			Claims.Add(new(JwtClaimTypes.Subject, id ?? Guid.NewGuid().ToString()));
			Claims.Add(new(JwtClaimTypes.Name, name));
			foreach (var role in roles)
			{
				Claims.Add(new(JwtClaimTypes.Role, role));
			}
		}
	}
}
