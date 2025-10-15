using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace AuHealthIds
{
    /// <summary>
    /// Health Provider Identifier Validators. Contains methods for validating HPI for Individuals and Organisations, and IHI
    /// </summary>
    public abstract class BaseHealthProviderIdentifier
    {


        /// <summary>
        /// Length 
        /// </summary>
        public const int LENGTH = 16;


        public int MinLength => LENGTH;

        public int MaxLength => LENGTH;


        /// <summary>
        /// Calculates the Luhn check digit 
        /// </summary>
        /// <param name="digits">digits to calculate</param>
        /// <returns>Check digit</returns>
        /// <remarks>
        /// Source: https://curmi.com/australian-health-identifiers/
        /// The check digit is based on the Luhn formula modulus 10. This is the same formula used for check digits on credit cards. 
        /// 
        ///  - Start with the Issuer ID and the next 9 digits(so no check digit).
        ///  - Double every second digit starting at the second last.If the result of doubling a digit is > 9, 
        ///    subtract 9 from the result (or add the digits).
        ///  - Add together all the doubled digits and the ones that weren’t.Call this s.
        ///  - The check digit is calculated by (10 - s mod 10)) mod 10.
        ///  
        /// Here is a Swift function that calculates the check digit. It is close enough to pseudo-code that any developer should be able to translate this to their favourite language:
        /// 
        /// ```swift
        ///     func generateCheck(_ id:[Int]) -> Int {
        ///     var check = 0
        ///     for count in 1...(id.count) {
        ///         let digit = id[count - 1]
        ///         if count%2 == 0 {
        ///             check += digit
        ///         } else {
        ///             check += (Int((digit*2)/10) + (digit*2)%10)
        ///         }
        ///     }
        ///     check = abs((10 - (check%10))%10)
        ///     return check
        /// }
        /// ```
        /// 
        /// As an example, if our base is 800360790627904, we do the following to find s:
        /// 
        /// Base	            8	0	0	3	6	0	7	9	0	6	2	7	9	0	4
        /// Double		        0		6		0		18		12		14		0	
        /// Add double digits                           9		3		5			
        /// Add All digits	    8	0	0	6	6	0	7	9	0	3	2	5	9	0	4
        /// 
        /// Adding the bottom row gives `s = 8+0+0+6+6+0+7+9+0+3+2+5+9+0+4 = 59`. Finally, check digit is `(59%10)%10 = 9`.
        /// </remarks>
        private static int CalculateLuhn(int[] digits)
        {
            // The HPI uses the Luhn algorithm for validation
            int sum = 0;
            bool applyDouble = true;
            for (int i = digits.Length - 1; i >= 0; i--) // Iterate from Right to Left
            {
                int digit = digits[i];
                if (applyDouble)
                {
                    sum += digit > 4 ? digit * 2 - 9 : digit * 2;
                }
                else
                {
                    sum += digit;
                }
                applyDouble = !applyDouble; // Toggle the flag for the next digit
            }
            int calculatedCheckDigit = (10 - sum % 10)%10;
            return calculatedCheckDigit;
        }


        /// <summary>
        /// Internal method to validate a IHI/HPI using the Luhn algorithm
        /// </summary>
        /// <param name="regex">Regex to validate the string</param>
        /// <param name="id">HPI to validate</param>
        /// <returns>True if value is valid, false if it is not</returns>
        /// <exception cref="ArgumentException">Thrown if HPI is null or empty.</exception>
        protected static bool ValidateId(Regex regex, string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException($"'{nameof(id)}' cannot be null or empty.", nameof(id));
            }
            // Ensure the value is in upper-case
            id = id.ToUpper().Trim();

            if (!regex.IsMatch(id))
                return false;

            // The first 6 digits are the issuer ID.
            // The next 9 digits identify the individual.
            // The last digit is a check digit(see below).
            var digits = id.Select(c => c - '0').Take(15).ToArray();
            var checkDigit = id.Last() - '0';

            int calculatedCheckDigit = CalculateLuhn(digits);

            return calculatedCheckDigit == checkDigit; // Check if the calculated check digit matches the provided one
        }

        protected static string GenerateId(string prefix)
        {
            string id = prefix + Shared.GenerateRandomNumberString(9);
            var checksum = CalculateLuhn(id.ToIntArray());
            return $"{id}{checksum}";
        }

       

    }
}
