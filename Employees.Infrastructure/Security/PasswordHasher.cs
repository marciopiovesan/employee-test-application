using Employees.Domain.Entities;
using System.Security.Cryptography;

namespace Employees.Infrastructure.Security
{
    public class PasswordHasher
    {
        private const int _saltSize = 16;
        private const int _hashSize = 32;
        private const int _iterations = 500000;

        private static readonly HashAlgorithmName _algorithm = HashAlgorithmName.SHA512;

        public static string Hash(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(_saltSize);
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, _iterations, _algorithm, _hashSize);

            return $"{Convert.ToHexString(hash)}-{Convert.ToHexString(salt)}";
        }

        public static bool Verify(string password, string passwordHash)
        {
            string[] parts = passwordHash.Split('-');
            byte[] hash = Convert.FromHexString(parts[0]);
            byte[] salt = Convert.FromHexString(parts[1]);

            byte[] inputHash = Rfc2898DeriveBytes.Pbkdf2(password, salt, _iterations, _algorithm, _hashSize);

            return CryptographicOperations.FixedTimeEquals(hash, inputHash);
        }

        private readonly Microsoft.AspNetCore.Identity.PasswordHasher<Employee> _hasher = new();

        public string HashPassword(Employee user, string password)
        {
            return _hasher.HashPassword(user, password);
        }

        public bool VerifyPassword(Employee user, string hashedPassword, string providedPassword)
        {
            var result = _hasher.VerifyHashedPassword(user, hashedPassword, providedPassword);
            return result == Microsoft.AspNetCore.Identity.PasswordVerificationResult.Success;
        }
    }
}
