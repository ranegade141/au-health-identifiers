using System;
using System.Linq;
using System.Text;

namespace AuHealthIds
{
    public static class Shared
    {
        private readonly static Random random = new Random();
        
        /// <summary>
        /// Generates a string of random numbers of the specified length
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static string GenerateRandomNumberString(int length)
        {
            if (length <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length), "Length must be a positive integer.");
            }

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                // Generate a random digit (0-9)
                sb.Append(random.Next(0, 10));
            }
            return sb.ToString();
        }

        /// <summary>
        /// Generates a random string of the charaters provided
        /// </summary>
        /// <param name="length"></param>
        /// <param name="chars"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static string GenerateRandomFromChars(int length, string chars)
        {
            if (length <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length), "Length must be a positive integer.");
            }

            if (string.IsNullOrEmpty(chars))
            {
                throw new ArgumentException($"'{nameof(chars)}' cannot be null or empty.", nameof(chars));
            }
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                // Generate a random character from the chars string
                sb.Append(chars[random.Next(chars.Length)]);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Generates a random string of capital letters (A-Z)
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GenerateRandomAlpha(int length)
        {
            return GenerateRandomFromChars(length, "ABCDEFGHIJKLMNOPQRSTUVWXYZ");
        }

        /// <summary>
        /// Generates a random string of alpha-numeric values
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GenerateRandomAlphaNumeric(int length)
        {
            return GenerateRandomFromChars(length, "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789");
        }

        /// <summary>
        /// Returns a random number between the start and end value
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static int GenerateRandomNumber(int start, int end)
        {
            return random.Next(start, end);
        }

     

        /// <summary>
        /// Converts a string to an int array. 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static int[] ToIntArray(this string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                throw new ArgumentException($"'{nameof(s)}' cannot be null or empty.", nameof(s));
            }

            if (!s.All(char.IsDigit))
            {
                throw new ArgumentException($"'{nameof(s)}' must consist only of numeric characters.");
            }

            return s.Select(c => c - '0').ToArray();
        }


    }
}
