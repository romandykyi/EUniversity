using AnyAscii;
using EUniversity.Core.Models;
using EUniversity.Core.Services;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using System.Text;

namespace EUniversity.Infrastructure.Services
{
    public class AuthHelper : IAuthHelper
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public const int PasswordLength = 12;

        public AuthHelper(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        // Extracts the first letter of first/last name and makes it an ascii letter,
        // or returns empty string if this cannot be achieved
        private static string ExtractFirstLetter(string name)
        {
            string result = name[0].ToString().Transliterate().ToLower();
            if (!char.IsAsciiLetterLower(result[0]))
            {
                return string.Empty;
            }
            return result;
        }

        /// <inheritdoc />
        public async Task<string> GenerateUserNameAsync(RandomNumberGenerator random, string firstName, string lastName)
        {
            string result;
            do
            {
                // Random 5-digit number
                const int min = 10000, max = 1000000;
                byte[] bytes = new byte[4];
                random.GetBytes(bytes);
                int rndInt = bytes[0] | (bytes[1] << 8) | (bytes[2] << 16) | (bytes[3] << 24);
                int rndNumber = rndInt % (max - min) + min;

                // Username part based on first name and last name
                // (or just "u" if it cannot be created)
                StringBuilder namePart = new();
                namePart.Append(ExtractFirstLetter(firstName));
                namePart.Append(ExtractFirstLetter(lastName));
                if (namePart.Length == 0)
                {
                    namePart.Append('u');
                }

                // Initials and random number
                result = $"{namePart}{rndNumber}";
            } while (await _userManager.FindByNameAsync(result) != null);

            return result;
        }

        /// <inheritdoc />
        public string GeneratePassword(RandomNumberGenerator random)
        {
            const string lowercases = "qwertyuipasdfghjkzxcvbnm";
            const string uppercases = "QWERTYUIPASDFGHJKLZXCVBNM";
            const string digits = "23456789";
            const string nonalphanumerics = "-=/*-+!@#$%_?.";
            string allowedCharacters = lowercases + uppercases + digits + nonalphanumerics;

            // Generate password with random characters
            const int length = PasswordLength - 4;
            StringBuilder passwordBuilder = new(length);
            byte[] bytes = new byte[length];
            random.GetBytes(bytes);
            for (int i = 0; i < length; i++)
            {
                int rndIndex = bytes[i] % allowedCharacters.Length;
                passwordBuilder.Append(allowedCharacters[rndIndex]);
            }

            // Insert lowercase letter, uppercase letter, digit and nonalphanumerics
            string[] ranges =
            {
                lowercases, uppercases, digits, nonalphanumerics
            };
            byte[] charactersBytes = new byte[ranges.Length];
            random.GetBytes(charactersBytes);
            byte[] indexesBytes = new byte[ranges.Length];
            random.GetBytes(indexesBytes);
            for (int i = 0; i < ranges.Length; i++)
            {
                // Random character from the range
                int rndChar = charactersBytes[i] % ranges[i].Length;
                char c = ranges[i][rndChar];
                // Random index to insert
                int rndIndex = indexesBytes[i] % passwordBuilder.Length;

                passwordBuilder.Insert(rndIndex, c);
            }

            return passwordBuilder.ToString();
        }
    }
}
