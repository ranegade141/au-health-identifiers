using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace AuHealthIds
{

    /// <summary>
    /// The Australian Health Practitioner Regulation Agency (AHPRA) Registration Number is a unique identifier for health practitioners in Australia.
    /// </summary>
    /// <remarks>
    /// Source: https://curmi.com/australian-health-identifiers/
    /// The AHPRA Registration Number consists of 3 letters and 10 digits. 
    ///  - The first 3 letters represent the health profession.
    ///  - The following 10 digits identifier the individual within that profession.
    ///  - There is NO check digit in this identifier.
    /// </remarks>
    public class AhpraRegistrationNumber : IIdentifier
    {
        /// <summary>
        /// AHPRA Regex
        /// </summary>
        private static readonly Regex ahpraRegex = new Regex(@"^(?<profession>[A-Z]{3})(?<identifier>\d{10})$");

        /// <summary>
        /// Profession Codes in a valid AHPRA Registration
        /// </summary>
        private static readonly string[] ahpraProfessionCodes = new string[]
        {
            "ATS", // ATSI Health Practice
            "CMR", // Chinese Medicine
            "CHI", // Chiropractic
            "DEN", // Dental
            "MED", // Medical
            "MRP", // Medical Radiation Practice
            "NMW", // Nursing and Midwifery
            "OCC", // Occupational Therapy
            "OPT", // Optometry
            "OST", // Osteopathy
            "PHA", // Pharmacy
            "PHY", // Physiotherapy
            "POD", // Podiatry
            "PSY"  // Psychology
        };

        public int MinLength => 13;

        public int MaxLength => 13;

        public IdentifierType IdType => IdentifierType.AHPRA;

        public string GenerateId()
        {
            string id = string.Concat(ahpraProfessionCodes[Shared.GenerateRandomNumber(0, ahpraProfessionCodes.Length)],
                        Shared.GenerateRandomNumberString(10));
            return id;
        }

        /// <summary>
        /// The Australian Health Practitioner Regulation Agency (AHPRA) Registration Number is a unique identifier for health practitioners in Australia.
        /// </summary>
        /// <param name="ahpraNumber">AHPRA Registration Number to validate</param>
        /// <returns>Whether the AHPRA number is valid</returns>
        /// <exception cref="ArgumentException"></exception>
        public bool ValidateId(string ahpraNumber)
        {
            if (string.IsNullOrEmpty(ahpraNumber))
            {
                throw new ArgumentException($"'{nameof(ahpraNumber)}' cannot be null or empty.", nameof(ahpraNumber));
            }

            ahpraNumber = ahpraNumber.ToUpper().Trim();

            var match = ahpraRegex.Match(ahpraNumber);
            if (!match.Success)
                return false;

            string profession = match.Groups["profession"].Value;
            string id = match.Groups["id"].Value;

            // We've got a match AND the profession matches the code's we expect.
            return ahpraProfessionCodes.Contains(profession);
        }
    }
}
