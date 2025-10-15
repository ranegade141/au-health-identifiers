using System;
using System.Text.RegularExpressions;

namespace AuHealthIds
{


    /// <summary>
    /// Validates Medicare Provider Numbers
    /// </summary>
    /// <remarks>
    /// See https://curmi.com/australian-health-identifiers/ and https://clearwater.com.au/code/provider
    /// 
    /// Medicare Provider Numbers are issued to GPs (doctors), Allied Health and other specialists. 
    /// A provider has a unique provider number for every location they practice at, so may have multiple provider numbers.
    /// 
    ///  - The first 6 digits are in the range 0–9 (know as the provider stem).
    ///  - The next character is the practice location character, an alphanumeric character(0–9 and A–Z) excluding I, O, S and Z.
    ///  - The final character is the check character, one of A, B, F, H, J, K, L, T, W, X or Y (see below).
    ///  
    /// To calculate the check digit you first convert the location character to a Practice Location Value (PLV) – a number between 
    /// 0 and 31. The conversion is according to the following table:
    /// 
    /// Location Character  PLV
    /// 0                   0
    /// 1                   1
    /// 2                   2
    /// 3                   3
    /// 4                   4
    /// 5                   5
    /// 6                   6
    /// 7                   7
    /// 8                   8
    /// 9                   9
    /// A                   10
    /// B                   11
    /// C                   12
    /// D                   13
    /// E                   14
    /// F                   15
    /// G                   16
    /// H                   17
    /// J                   18
    /// K                   19
    /// L                   20
    /// M                   21
    /// N                   22
    /// P                   23
    /// Q                   24
    /// R                   25
    /// T                   26
    /// U                   27
    /// V                   28
    /// W                   29
    /// X                   30
    /// Y                   31
    /// 
    /// * Take each digit from the first 6 digits, followed by the PLV.
    /// * Multiple them in turn using the following weights: 3, 5, 8, 4, 2, 1, 6.
    /// * The check digit is the sum of the weighted values modulo 11.
    /// * Convert the check digit to a check character using the following table.
    /// 
    /// Check Digit	Check Character
    /// 0	        Y
    /// 1	        X
    /// 2	        W
    /// 3	        T
    /// 4	        L
    /// 5	        K
    /// 6	        J
    /// 7	        H
    /// 8	        F
    /// 9	        B
    /// 10         	A
    /// 
    /// As an example, if the first 6 digits are `486674` and the location character is `Y`, then the PLV is `31`, 
    /// the checksum is `(4*3+8*5+6*8+6*4+7*2+4*1+31*6)%11 = 9`, and `9` maps to the check character `B`.
    /// 
    /// Note that if a provider number is presented with 5 digits, you should add a 0 to the front before 
    /// calculating the check character.It is likely some very early numbers were 5 digits before the government 
    /// moved to 6 digits.    
    /// </remarks>
    public class ProviderNumber : IIdentifier
    {
        // The index of each character reflects the Practice Location Value (PLV) for the character
        // The character is the practice location character, an alphanumeric character (0–9 and A–Z) excluding I, O, S and Z.
        const string practiceLocationNumbers = "0123456789ABCDEFGHJKLMNPQRTUVWXY";
        // The final character is the check character, one of A, B, F, H, J, K, L, T, W, X or Y 
        const string checkCharacters = "YXWTLKJHFBA";
        // Weights for each digit in the stem. Note that the final weight is 6 and is multipled by the PLV 
        static readonly int[] weights = new int[] { 3, 5, 8, 4, 2, 1 };

        readonly static Regex providerNumberRegex = new Regex($@"^(?<stem>\d{{5,6}})(?<location>[{practiceLocationNumbers}]{{1}})(?<check>[{checkCharacters}]{{1}})$");

        public int MinLength => 5;

        public int MaxLength => 6;

        public IdentifierType IdType => IdentifierType.Provider;

        private char CalculateCheckChar(int plv, string stem)
        {
            int sum = plv * 6; // The final weight is 6 and is multipled by the PLV. Do it up front!
            for (int i = 0; i < weights.Length; i++)
            {
                // Subtract the '0' char from each digit to get the int representation of the value (remember your asci tables youngin's)
                // Alternatively, do `char.GetNumericValue(c)`, or Convert.ToInt32('3'.ToString())
                sum += (stem[i] - '0') * weights[i];
            }

            return checkCharacters[sum % 11];

        }

        /// <summary>
        /// Validates the specified provider number
        /// </summary>
        /// <param name="providerNumber">Provider Number to validate</param>
        /// <returns>True if the number if valid, False if it is not</returns>
        /// <exception cref="ArgumentException">Thrown if provider numbe ris not specified</exception>
        public bool ValidateId(string providerNumber)
        {

            if (string.IsNullOrEmpty(providerNumber))
            {
                throw new ArgumentException($"'{nameof(providerNumber)}' cannot be null or empty.", nameof(providerNumber));
            }

            // Ensure the value is in upper-case
            providerNumber = providerNumber.ToUpper().Trim();

            // - The first 6 digits are in the range 0–9 (known as the provider stem).
            // - The next character is the practice location character, an alphanumeric character (0–9 and A–Z) excluding I, O, S and Z.
            // - The final character is the check character, one of A, B, F, H, J, K, L, T, W, X or Y (see below).
            var match = providerNumberRegex.Match(providerNumber);
            if (!match.Success)
            {
                //throw new InvalidDataException("The provided format is not in the expected format.");
                return false;
            }

            string location = match.Groups["location"].Value;
            char checkDigit = match.Groups["check"].Value[0];
            // Get the pracise location Value 
            int plv = practiceLocationNumbers.IndexOf(location);

            if (plv == -1)
            {
                //throw new InvalidDataException($"The practice location character '{location}' is not valid.");
                return false;
            }

            var stem = match.Groups["stem"].Value;

            // Note that if a provider number is presented with 5 digits, you should add a 0 to the front before calculating the check character.
            // It is likely some very early numbers were 5 digits before the government moved to 6 digits.
            if (stem.Length == 5)
                stem = "0" + stem;

            var checkChar = CalculateCheckChar(plv, stem);

            // Make sure it matches the check character
            // The check digit is the sum of the weighted values modulo 11.
            return checkDigit == checkChar;

        }

        public string GenerateId()
        {
            string stem = Shared.GenerateRandomNumberString(6);
            string location = Shared.GenerateRandomFromChars(1, practiceLocationNumbers);
            int plv = practiceLocationNumbers.IndexOf(location);
            char checkChar = CalculateCheckChar(plv, stem);
            return $"{stem}{location}{checkChar}";
        }

    }
}