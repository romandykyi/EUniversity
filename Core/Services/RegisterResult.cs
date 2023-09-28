using Microsoft.AspNetCore.Identity;
using System.Diagnostics.CodeAnalysis;

namespace EUniversity.Core.Services
{
	/// <summary>
	/// Wrapper class for IdentityResult with username and password properties.
	/// </summary>
	public class RegisterResult
	{
		private readonly IdentityResult _identityResult;
		private readonly string? _userName, _password;

		/// <inheritdoc cref="IdentityResult.Succeeded" />
		public bool Succeeded => _identityResult.Succeeded;

		/// <inheritdoc cref="IdentityResult.Errors" />
		public IEnumerable<IdentityError> Errors => _identityResult.Errors;

		/// <summary>
		/// Username of the registered user. 
		/// </summary>
		/// <value>
		/// <see langword="null" /> when <see cref="Succeeded" /> is <see langword="false" />.
		/// </value>
		public string UserName => _userName!;
		/// <summary>
		/// Password of the registered user.
		/// <see langword="null" /> on failure.
		/// </summary>
		/// <value>
		/// <see langword="null" /> when <see cref="Succeeded" /> is <see langword="false" />.
		/// </value>
		public string Password => _password!;

		public RegisterResult(IdentityResult result,
			string? userName = null, string? password = null)
		{
			_identityResult = result;
			if (result.Succeeded)
			{
				_userName = userName;
				_password = password;
			}
		}

		public static implicit operator IdentityResult(RegisterResult registerResult)
		{
			return registerResult._identityResult;
		}
	}
}
