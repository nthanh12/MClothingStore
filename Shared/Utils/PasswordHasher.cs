using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Utils
{
    public static class PasswordHasher
    {
        public static string HassPassword(string password)
        {
            // Generate a random salt using RandomNumberGenerator
            byte[] salt = new byte[16];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // Create a new instance of SHA-256
            using (SHA256 sha256 = SHA256.Create())
            {
                // Combine the password and salt
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password); // Chuyển pass về dạng byte
                byte[] saltedPassword = new byte[passwordBytes.Length + salt.Length]; // Khởi tạo mới 1 biến có độ dài bằng mật khẩu dạng byte + salt
                
                Array.Copy(passwordBytes, saltedPassword, passwordBytes.Length);
                Array.Copy(salt, 0, saltedPassword, passwordBytes.Length, salt.Length);

                // Compute the hash
                byte[] hashBytes = sha256.ComputeHash(saltedPassword);

                // Combine the salt and hash
                byte[] saltedHash = new byte[hashBytes.Length + salt.Length];
                Array.Copy(salt, saltedHash, salt.Length);
                Array.Copy(hashBytes, 0, saltedHash, salt.Length, hashBytes.Length);

                // Convert the salted hash to a base64-encoded string
                string hashBase64 = Convert.ToBase64String(saltedHash);

                return hashBase64;
            }
        }
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            // Convert the hashed password from base64 to byte array
            byte[] saltedHash = Convert.FromBase64String(hashedPassword);

            // Extract the salt
            byte[] salt = new byte[16];
            Array.Copy(saltedHash, salt, salt.Length);

            // Recompute the hash using the provided password and extracted salt
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] saltedPassword = new byte[passwordBytes.Length + salt.Length];
                Array.Copy(passwordBytes, saltedPassword, passwordBytes.Length);
                Array.Copy(salt, 0, saltedPassword, passwordBytes.Length, salt.Length);

                byte[] hashBytes = sha256.ComputeHash(saltedPassword);

                // Compare the computed hash with the stored hash
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    if (hashBytes[i] != saltedHash[i + salt.Length])
                    {
                        return false; // Passwords do not match
                    }
                }

                return true; // Passwords match
            }
        }
    }
}
