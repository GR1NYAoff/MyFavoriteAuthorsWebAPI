using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace MyFavoriteAuthorsWebService.Helpers
{
    public static class HashPasswordHelper
    {
        public static (string, byte[]) HashPassword(string password, byte[]? salt = null)
        {
            if (salt == null)
            {
                // generate a 128-bit salt using a cryptographically strong random sequence of nonzero values
                salt = new byte[128 / 8];
                using var rngCsp = new RNGCryptoServiceProvider();
                rngCsp.GetNonZeroBytes(salt);
            }

            // derive a 256-bit subkey (use HMACSHA512 with 100,000 iterations)
            var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            return (hashed, salt);
        }
    }
}
