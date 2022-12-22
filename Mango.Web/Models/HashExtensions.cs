using System.Security.Cryptography;
using System.Text;

namespace Mango.Web.Models
{
    //
    // Summary:
    //     Extension methods for hashing strings
    public static class HashExtensions
    {
        //
        // Summary:
        //     Creates a SHA256 hash of the specified input.
        //
        // Parameters:
        //   input:
        //     The input.
        //
        // Returns:
        //     A hash
        public static string Sha256(this string input)
        {
            if (input == null || string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            using (SHA256 sHA = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(input);
                return Convert.ToBase64String(sHA.ComputeHash(bytes));
            }
        }

        //
        // Summary:
        //     Creates a SHA256 hash of the specified input.
        //
        // Parameters:
        //   input:
        //     The input.
        //
        // Returns:
        //     A hash.
        public static byte[] Sha256(this byte[] input)
        {
            if (input == null)
            {
                return null;
            }

            using (SHA256 sHA = SHA256.Create())
            {
                return sHA.ComputeHash(input);
            }
        }

        //
        // Summary:
        //     Creates a SHA512 hash of the specified input.
        //
        // Parameters:
        //   input:
        //     The input.
        //
        // Returns:
        //     A hash
        public static string Sha512(this string input)
        {
            if (input == null || string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            using (SHA512 sHA = SHA512.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(input);
                return Convert.ToBase64String(sHA.ComputeHash(bytes));
            }
        }
    }
}
