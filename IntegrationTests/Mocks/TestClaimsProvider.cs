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
			id ??= Guid.NewGuid().ToString();
			Claims.Add(new("sub", id));
			Claims.Add(new(ClaimTypes.NameIdentifier, id));
			Claims.Add(new(ClaimTypes.Name, name));
			foreach (var role in roles)
			{
				Claims.Add(new(ClaimTypes.Role, role));
			}
		}
	}
}
