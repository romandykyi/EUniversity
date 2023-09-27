using System.Security.Cryptography;

namespace EUniversity.Core.Services
{
	/// <summary>
	/// Provides methods for generating usernames and passwords.
	/// </summary>
	public interface IAuthHelper
	{
		/// <summary>
		/// Generates random unique username.
		/// </summary>
		/// <param name="random">Random numbers generator to be used.</param>
		/// <param name="firstName">First name of the user.</param>
		/// <param name="lastName">Last name of the.</param>
		/// <returns>
		/// Unique username.
		/// </returns>
		Task<string> GenerateUserNameAsync(RandomNumberGenerator random, string firstName, string lastName);

		/// <summary>
		/// Generates random password.
		/// </summary>
		/// <param name="random">Random numbers generator to be used.</param>
		/// <returns>
		/// 12 characters password with at least one lowercase letter, one uppercase letter,
		/// one number and one nonalphanumerical character.
		/// </returns>
		string GeneratePassword(RandomNumberGenerator random);
	}
}
