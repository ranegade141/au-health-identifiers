using System;
using System.Text.RegularExpressions;

namespace AuHealthIds
{
    /// <summary>
    /// Individual Health identifier (IHI)
    /// </summary>
    /// <remarks>
    /// Source: https://curmi.com/australian-health-identifiers/
    /// The Individual Health identifier (IHI) is a unique 16 digit identifier assigned to all residents and 
    /// others accessing healthcare in Australia. 
    /// It is often used with My Health Record APIs, though most patients in Australia don’t even know this 
    /// identifier exists.
    /// 
    ///  - The first 6 digits are the issuer ID. This is the same for all IHIs, and is 800360.
    ///  - The next 9 digits identify the individual.
    ///  - The last digit is a check digit (see below).
    /// </remarks>
    public class IndividualHealthIdentifier : BaseHealthProviderIdentifier, IIdentifier
    {
        public IdentifierType IdType => IdentifierType.IHI;

        /// <summary>
        /// IHI Prefix - The IHI prefix is always 800360
        /// </summary>
        public const string IHI_PREFIX = "800360";

        /// <summary>
        /// IHI 
        /// </summary>
        protected readonly static Regex ihiRegex = new Regex($@"^{IHI_PREFIX}\d{{10}}$");

        public bool ValidateId(string ihi)
        {
            if (string.IsNullOrEmpty(ihi))
            {
                throw new ArgumentException($"'{nameof(ihi)}' cannot be null or empty.", nameof(ihi));
            }
            return ValidateId(ihiRegex, ihi);
        }

        public string GenerateId()
        {
            return GenerateId(IHI_PREFIX);
        }
    }
}
