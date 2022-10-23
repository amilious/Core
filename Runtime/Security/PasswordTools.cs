using System;
using System.Security.Cryptography;
using Amilious.Core.Extensions;

namespace Amilious.Core.Security {
    
    /// <summary>
    /// This class is used for securely hashing passwords.
    /// </summary>
    public static class PasswordTools {
        
        #region Constants //////////////////////////////////////////////////////////////////////////////////////////////
                          
        private const string CHAR_POOL = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()";
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        ///  This method is used to make a random string for either a salt or identifier.
        /// </summary>
        /// <param name="size">The length of the generated string.</param>
        /// <returns>The randomly generated string.</returns>
        public static string GetRandomString(int size) {
            //https://stackoverflow.com/questions/32932679/using-rngcryptoserviceprovider-to-generate-random-string
            var sb = StringBuilderPool.Rent;
            var length = size;
            using (var rng = new RNGCryptoServiceProvider()) {
                var uintBuffer = new byte[sizeof(uint)];
                while (length-- > 0) {
                    rng.GetBytes(uintBuffer);
                    var num = BitConverter.ToUInt32(uintBuffer, 0);
                    sb.Append(CHAR_POOL[(int)(num % (uint)CHAR_POOL.Length)]);
                }
            }
            return sb.ToStringAndReturnToPool();
        }
        
        /// <summary>
        /// This method is used to hash a password with the given salt using SHA512.
        /// </summary>
        /// <param name="password">The raw password.</param>
        /// <param name="salt">The user's salt.</param>
        /// <returns>The raw salted password.</returns>
        public static string HashPasswordSHA512(string password, string salt) {
            password = password.Trim();
            salt = salt.Trim();
            var bytes = System.Text.Encoding.UTF8.GetBytes($"{salt}{password}");
            using var hash = SHA512.Create();
            var hashedInputBytes = hash.ComputeHash(bytes);
            // Convert to text
            // StringBuilder Capacity is 128, because 512 bits / 8 bits in byte * 2 symbols for byte 
            var hashedInputStringBuilder = new System.Text.StringBuilder(128);
            foreach (var b in hashedInputBytes) hashedInputStringBuilder.Append(b.ToString("X2"));
            var hashedPassword = hashedInputStringBuilder.ToString();
            return hashedPassword;
        }

        /// <summary>
        /// This method is used to hash the clients password before sending it to the server.
        /// </summary>
        /// <param name="password">The user's stored password (this may have another hash).</param>
        /// <param name="iterations">The number of hash iterations.</param>
        /// <param name="saltSize">The size of the generated salt.</param>
        /// <param name="hashSize">The hash size.</param>
        /// <returns>The hashed password.</returns>
        /// <remarks>This method should only be called from a client.</remarks>
        public static string ClientSecurePasswordHash(string password, int iterations, int saltSize = 16, 
            int hashSize = 20) {
            using var rng = new RNGCryptoServiceProvider();
            byte[] newSalt;
            rng.GetBytes(newSalt = new byte[saltSize]);
            using var pbkdf2 = new Rfc2898DeriveBytes(password, newSalt, iterations);
            var hash = pbkdf2.GetBytes(hashSize);
            // Combine salt and hash
            var hashBytes = new byte[saltSize + hashSize];
            Array.Copy(newSalt, 0, hashBytes, 0, saltSize);
            Array.Copy(hash, 0, hashBytes, saltSize, hashSize);
            // Convert to base64
            return Convert.ToBase64String(hashBytes);
        }

        /// <summary>
        /// This method is used to validate a hash received by the server from the client.
        /// </summary>
        /// <param name="password">The user's stored password (this may have another hash).</param>
        /// <param name="hashedPassword">The hashed password received by the server.</param>
        /// <param name="iterations">The number of hash iterations.</param>
        /// <param name="saltSize">The size of the generated salt.</param>
        /// <param name="hashSize">The hash size.</param>
        /// <returns>True if the hashed password is valid, otherwise false.</returns>
        public static bool ServerValidateClientSecurePasswordHash(string password, string hashedPassword,int iterations,
            int saltSize = 16, int hashSize = 20) {
            // Get hash bytes
            var hashBytes = Convert.FromBase64String(hashedPassword);
            var salt = new byte[saltSize];
            Array.Copy(hashBytes, 0, salt, 0, saltSize);
            // Create hash with given salt
            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);
            var hash = pbkdf2.GetBytes(hashSize);
            // Get result
            for(var i = 0; i < hashSize; i++) {
                if(hashBytes[i + saltSize] == hash[i]) continue;
                return false;
            }
            return true;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
    
}