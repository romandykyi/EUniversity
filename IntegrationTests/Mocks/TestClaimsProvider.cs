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

		public static TestClaimsProvider Create(string name, params string[] roles)
		{
			var provider = new TestClaimsProvider();
			provider.Claims.Add(new(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()));
			provider.Claims.Add(new(ClaimTypes.Name, name));
			foreach (var role in roles)
			{
				provider.Claims.Add(new(ClaimTypes.Role, role));
			}

			return provider;
		}
	}
}
