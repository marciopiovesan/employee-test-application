using Employees.Application.Interfaces.Security;
using Employees.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;

namespace Employees.Infrastructure.Security
{
    public class PasswordHasher : IPasswordHasher
    {
        //private const int _saltSize = 16;
        //private const int _hashSize = 32;
        //private const int _iterations = 500000;
        //private static readonly HashAlgorithmName _algorithm = HashAlgorithmName.SHA512;

        //public string Hash(string password)
        //{
        //    byte[] salt = RandomNumberGenerator.GetBytes(_saltSize);
        //    byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, _iterations, _algorithm, _hashSize);

        //    return $"{Convert.ToHexString(hash)}-{Convert.ToHexString(salt)}";
        //}

        //public bool Verify(string password, string passwordHash)
        //{
        //    string[] parts = passwordHash.Split('-');
        //    byte[] hash = Convert.FromHexString(parts[0]);
        //    byte[] salt = Convert.FromHexString(parts[1]);

        //    byte[] inputHash = Rfc2898DeriveBytes.Pbkdf2(password, salt, _iterations, _algorithm, _hashSize);

        //    return CryptographicOperations.FixedTimeEquals(hash, inputHash);
        //}

        public string Hash(string password)
        {
            return new PasswordHasher<Employee>().HashPassword(null, password);
        }

        public bool Verify(string password, string hashedPassword)
        {
            var result = new PasswordHasher<Employee>().VerifyHashedPassword(null, hashedPassword, password);
            return result == PasswordVerificationResult.Success;
        }
    }
}
