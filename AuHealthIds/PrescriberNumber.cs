using System;
using System.Linq;

namespace AuHealthIds
{
    /// <summary>
    /// Validates Medicare Prescriber Numbers
    /// </summary>
    /// <remarks>
    /// Source: https://curmi.com/australian-health-identifiers/
    /// A prescriber number is a unique, 7-digit number that identifies a registered health professional as eligible to 
    /// prescribe medication under the Pharmaceutical Benefits Scheme (PBS) in Australia. 
    ///
    /// If you’ve ever been issued a prescription in paper form in Australia you’d likely see a prescriber number 
    /// up the top near the prescriber’s name.
    /// 
    ///  - The first 6 digits are in the range 0–9.
    ///  - The next digit is a checksum digit(see below).
    ///  
    /// There are actually two different algorithms for calculating the check digit.
    /// If the first digit is `0`:
    ///  - Take each digit from the first 6 digits.
    ///  - Multiple them in turn using the following weights: `0, 5, 8, 4, 2, 1`.
    ///  - The check digit is the sum of the weighted values modulo 11.
    /// 
    /// If the first digit is not `0`:
    ///  - Take each digit from the first 6 digits.
    ///  - Multiple them in turn using the following weights: 1, 3, 7, 9, 1, 3.
    ///  - The check digit is the sum of the weighted values modulo 10.
    ///  
    /// For example, if the stem was `084840`, the check digit would be `(8*5+4*8+8*4+4*2+0*1)%11 = 2`. 
    /// If the stem was `242573`, the check digit would be `(2*1+4*3+2*7+5*9+7*1+3*3)%10 = 9`.
    /// 
    /// Note that the algorithm here is unclear in the case of a stem starting with 0 giving a remainder of 10. 
    /// For example, 012342 gives a check digit of 10. The specification I have seen does not say what to do 
    /// with the check digit in this case. Do you put an “A“? Is the specification wrong and it should also use modulo 10? 
    /// Does this never happen in the real world? If anyone knows, let me know and I’ll update the notes here.
    /// </remarks>
    public class PrescriberNumber : IIdentifier
    {
        // A prescriber number is a unique, 7-digit number that identifies a registered health professional as eligible to prescribe medication
        // under the Pharmaceutical Benefits Scheme (PBS) in Australia.
        /// <summary>
        /// Weights for Prescriber number starting with 0
        /// </summary>
        private readonly static int[] zeroWeights = new int[] { 0, 5, 8, 4, 2, 1 };
        /// <summary>
        /// Weight for Prescriber number starting <> 0
        /// </summary>
        private readonly static int[] otherWeights = new int[] { 1, 3, 7, 9, 1, 3 };
        /// <summary>
        /// Modulo for Prescriber number starting with 0
        /// </summary>
        private const int ZERO_MODULO = 11;
        /// <summary>
        /// Modulo for Prescriber number starting <> 0
        /// </summary>
        private const int OTHER_MODULO = 10;

        public const int LENGTH = 7;

        public int MinLength => LENGTH;
        public int MaxLength => LENGTH;

        public IdentifierType IdType => IdentifierType.Prescriber;

        private int CalculateChecksum(int[] digits)
        {
            var weights = digits[0] == 0 ? zeroWeights : otherWeights;
            var modulo = digits[0] == 0 ? ZERO_MODULO : OTHER_MODULO;
            int sum = 0;
            for (int i = 0; i < weights.Length; i++)
            {
                sum += digits[i] * weights[i];
            }
            return sum % modulo;
        }

        /// <summary>
        /// Validates a Prescriber Number
        /// </summary>
        /// <param name="prescriberNumber">Precriber number to validate</param>
        /// <returns>Bool - true if the value is valid, false if it is not</returns>
        /// <exception cref="ArgumentException"></exception>
        public bool ValidateId(string prescriberNumber)
        {
            if (string.IsNullOrEmpty(prescriberNumber))
            {
                throw new ArgumentException($"'{nameof(prescriberNumber)}' cannot be null or empty.", nameof(prescriberNumber));
            }
            // Ensure the value is in upper-case
            prescriberNumber = prescriberNumber.ToUpper().Trim();
            if (prescriberNumber.Length != 7 || !prescriberNumber.All(char.IsDigit))
            {
                //throw new ArgumentException("Prescriber number must be a 7-digit number.", nameof(prescriberNumber));
                return false;
            }

            var digits = prescriberNumber.Select(c => c - '0').ToArray(); 
            var checksumDigit = digits[6];
            var checksum = CalculateChecksum(digits);

            return checksumDigit == checksum;
        }

        public string GenerateId()
        {
            string id = Shared.GenerateRandomNumberString(6);
            var checksum = CalculateChecksum(id.ToIntArray());
            if (checksum == 10) // The notes suggest this is invalid - something is up with this
                return GenerateId();
            id += checksum.ToString();
            return id;
        }

    }
}
