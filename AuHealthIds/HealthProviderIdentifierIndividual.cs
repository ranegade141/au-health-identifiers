using System;
using System.Text.RegularExpressions;

namespace AuHealthIds
{
    /// <summary>
    /// Healthcare Provider Identifier for Individuals (HPI-I)
    /// </summary>
    /// <remarks>
    /// Source: https://curmi.com/australian-health-identifiers/
    /// The Healthcare Provider Identifier for Individuals (HPI-I) is allocated by Medicare to healthcare providers and other 
    /// health personnel involved in providing patient care. Like the IHI, it is often used with My Health Record API calls.
    ///  - The first 6 digits are the issuer ID. This is the same for all HPI-Is, and is 800361.
    ///  - The next 9 digits identify the individual.
    ///  - The last digit is a check digit (see below).
    ///  
    /// The check digit is based on the Luhn formula modulus 10. See IHI for an explanation of how this algorithm works to 
    /// calculate the check digit.
    /// </remarks>
    public class HealthProviderIdentifierIndividual : BaseHealthProviderIdentifier, IIdentifier
    {

        /// <summary>
        /// HPI-I Prefix
        /// </summary>
        public const string HPI_I_PREFIX = "800361";

        /// <summary>
        /// HPI-I Regex
        /// </summary>
        protected readonly static Regex hpiIndividualRegex = new Regex($@"^{HPI_I_PREFIX}\d{{10}}$");

        public IdentifierType IdType => IdentifierType.HPI_I;

        /// <summary>
        /// Validate Health Provider Identifier for Individuals (HPI-I)
        /// </summary>
        /// <param name="hpi">HPI-I to validate</param>
        /// <returns>True if the HPI is valid, false if it is not</returns>
        /// <exception cref="ArgumentException">Thrown if hpi is blank or null</exception>
        public bool ValidateId(string hpi)
        {
            if (string.IsNullOrEmpty(hpi))
            {
                throw new ArgumentException($"'{nameof(hpi)}' cannot be null or empty.", nameof(hpi));
            }

            return ValidateId(hpiIndividualRegex, hpi);
        }

        public string GenerateId()
        {
            return GenerateId(HPI_I_PREFIX);
        }
    }
}
