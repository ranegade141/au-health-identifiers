using System;
using System.Text.RegularExpressions;


namespace AuHealthIds
{
    /// <summary>
    /// Australian Medicare Number
    /// </summary>
    /// <remarks>
    /// Source: https://curmi.com/australian-health-identifiers/
    /// The Australian Medicare Number is the most well-known health identifier in the country as it is a unique number 
    /// provided to every Australian citizen for claiming health services under Medicare. 
    /// Just about every person in the country has one.
    /// 
    /// The Medicare Number format consists of 10 digits and an Individual Reference number.
    ///  - The first digit is in the range 2–6.
    ///  - The next 7 digits are in the range 0–9.
    ///  - The next digit is a checksum digit(see below).
    ///  - The next digit is the issue number(starting at 1; each time a card is replaced(expired/lost) this number is increased).
    ///  - The next digit is the Individual Reference Number(IRN) – a reference to the individual on the card.
    ///  
    /// Software will often only request the first 10 digits, and may require the IRN to be include separately or after a “/”.
    /// The checksum digit is a simple algorithm that produces a digit based on the other numbers.
    /// This helps to reduce input errors by misreading a number or swapping digits as the checksum will be different in 
    /// such cases indicating incorrect entry.
    /// 
    /// The algorithm is as follows:
    ///  - Take each digit from the first 8 digits.
    ///  - Multiple them in turn using the following weights: 1, 3, 7, 9, 1, 3, 7, 9.
    ///  - The check digit is the sum of the weighted values modulo 10.
    ///
    /// For example, if the first 8 digits were `31899770`, then the check digit would be 
    /// `(3*1+1*3+8*7+9*9+9*1+7*3+7*7+0*9)%10 = 2`.
    /// 
    /// Good healthcare software should check the digits are in the range as above, 
    /// there are 10 (plus an IRN if required), and that the check digit matches.
    /// </remarks>
    public class MedicareNumber : IIdentifier
    {
        private readonly static int[] weights = new int[] { 1, 3, 7, 9, 1, 3, 7, 9 };
        private const int modulus = 10;

        public int MinLength => 10;
        public int MaxLength => 11; // If an IRN is included
        public IdentifierType IdType => IdentifierType.Medicare;

        // A valid Medicare number consists of 10, and an Individual Reference Number. 
        // The first digit is in the range 2–6.
        // The next 7 digits are in the range 0–9.
        // The next digit is a checksum digit(see below).
        // The next digit is the issue number(starting at 1; each time a card is replaced(expired / lost) this number is increased).
        // The next digit is the Individual Reference Number(IRN) – a reference to the individual on the card.

        private static readonly Regex medicareNumberRegex = new Regex(@"[2-6]\d{7}\d{1}[1-9]\d?");

        private int CalculateChecksum(string medicareNumber)
        {
            int sum = 0;
            for (int i = 0; i < weights.Length; i++)
            {
                sum += (medicareNumber[i] - '0') * weights[i];
            }
            return sum % modulus;
        }

        /// <summary>
        /// Validates the specified Medicare number
        /// </summary>
        /// <param name="medicareNumber">Medicare Number</param>
        /// <returns>True if the number if valid, False if it is not</returns>
        /// <exception cref="ArgumentException">Thrown if Medicare number is not specified</exception>
        public bool ValidateId(string medicareNumber)
        {
            if (string.IsNullOrEmpty(medicareNumber))
            {
                throw new ArgumentException($"'{nameof(medicareNumber)}' cannot be null or empty.", nameof(medicareNumber));
            }
            // Ensure the value is in upper-case
            medicareNumber = medicareNumber.ToUpper().Trim();
            // Validate the format
            if (!medicareNumberRegex.IsMatch(medicareNumber))
            {
                return false;
            }
            int checkDigit = medicareNumber[8] - '0';

            // Calculate the checksum
            int checksum = CalculateChecksum(medicareNumber);

            // Compare to the provided check digit
            return checkDigit == checksum;
        }

        public string GenerateId()
        {
            // First digit is between 2 and 6
            string id = Shared.GenerateRandomNumber(2, 6).ToString();
            id += Shared.GenerateRandomNumberString(7);
            id += CalculateChecksum(id).ToString(); // Check Digit
            id += Shared.GenerateRandomNumber(1, 9).ToString(); // Issue number
            return id;
        }
    }
}
