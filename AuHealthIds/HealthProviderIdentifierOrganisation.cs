using System;
using System.Text.RegularExpressions;

namespace AuHealthIds
{
    /// <summary>
    /// The Healthcare Provider Identifier for Organisations (HPI-O) 
    /// </summary>
    /// <remarks>
    /// Source: https://curmi.com/australian-health-identifiers/
    /// The Healthcare Provider Identifier for Organisations (HPI-O) is allocated by Medicare to healthcare practices 
    /// involved in providing patient care. Like the IHI and HPI-I, it is often used with My Health Record API calls.
    ///  - The first 6 digits are the issuer ID.This is the same for all HPI-Os, and is 800362.
    ///  - The next 9 digits identify the individual.
    ///  - The last digit is a check digit (see below).
    ///
    /// The check digit is based on the Luhn formula modulus 10. See IHI above for an explanation of how this algorithm works to 
    /// calculate the check digit.
    /// </remarks>
    public class HealthProviderIdentifierOrganisation : BaseHealthProviderIdentifier, IIdentifier
    {
        /// <summary>
        /// HPI-O Prefix
        /// </summary>
        public const string HPI_O_PREFIX = "800362";
        /// <summary>
        /// HPI-O Regex
        /// </summary>
        protected readonly static Regex hpiOrgRegex = new Regex($@"^{HPI_O_PREFIX}\d{{10}}$");

        public IdentifierType IdType => IdentifierType.HPI_O;

        /// <summary>
        /// Validate Health Provider Identifier for Organisations (HPI-O)
        /// </summary>
        /// <param name="hpi">The HPI-O to Validate</param>
        /// <returns>True if the HPI is valid, false if it is not</returns>
        /// <exception cref="ArgumentException">Thrown if hpi is blank or null</exception>
        public bool ValidateId(string hpi)
        {
            if (string.IsNullOrEmpty(hpi))
            {
                throw new ArgumentException($"'{nameof(hpi)}' cannot be null or empty.", nameof(hpi));
            }
            return ValidateId(hpiOrgRegex, hpi);
        }

        public string GenerateId()
        {
            return GenerateId(HPI_O_PREFIX);
        }
    }
}
