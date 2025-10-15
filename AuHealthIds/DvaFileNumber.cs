using System;
using System.Text.RegularExpressions;

namespace AuHealthIds
{
    /// <summary>
    /// A Department of Veterans’ Affairs (DVA) file number is a reference number for veterans and dependents.
    /// </summary>
    /// <remarks>
    /// Source: https://curmi.com/australian-health-identifiers/
    /// A Department of Veterans’ Affairs (DVA) file number is a reference number for veterans and dependents.
    /// The DVA number will be 8 or 9 characters long. Individuals with this card may have 3 different coloured cards:
    ///  - Veteran Gold Card: Allows access to clinically required treatment, pharmaceuticals, support, and services
    ///    for all health conditions. Surviving partners and dependents may also be eligible for this card.
    ///    
    ///  - Veteran White Card: Allows access to clinically required treatment for specific health conditions related 
    ///    to the veteran’s service. White Card holders may be eligible for Veterans’ Home Care (VHC) services.
    ///    
    ///  - Veteran Orange Card: Allows access to clinically required pharmaceutical items for all medical conditions 
    ///    at a cheaper concession rate. 
    ///    To be eligible, veterans must have qualifying service from World War I or World War II, be at least 
    ///    70 years old, and have been a resident of Australia for at least 10 years.
    ///
    /// The DVA number itself will be 8 or 9 characters long.
    /// The first character is a state identifier from the following values:
    ///  - N - New South Wales (including ACT)
    ///  - S - South Australia (including Northern Territory)
    ///  - W - Western Australia
    ///  - T - Tasmania
    ///  - V - Victoria
    ///  - Q - Queensland
    ///
    /// The War Code characters optionally follow the state identifier (for WW1, no code is used). War Codes are valid from the following list:
    ///  -      World War I
    ///  - A    Allied Forces
    ///  - AGX  Act of Grace 1939
    ///  - BUR  Burma
    ///  - BW   Boer War
    ///  - CGW  Commonwealth Gulf War
    ///  - CIV  Civilians
    ///  - CN   Canadian Forces 1914
    ///  - CNK  Canada Korea/Malaya
    ///  - CNS  Canada Special Overseas Service
    ///  - CNX  Canadian Forces 1939
    ///  - FIJ  Fiji
    ///  - GHA  Ghana
    ///  - GW   Australian Gulf War
    ///  - HKS  Hong Kong(SP Eligibility)
    ///  - HKX  Hong Kong 1939
    ///  - IND  India
    ///  - IV   Indigenous Veteran 1939
    ///  - KM   Korea-Malaya
    ///  - KYA  Kenya
    ///  - MAL  Malaya-Singapore
    ///  - MAU  Mauritius
    ///  - MLS  Malaysia – Singapore SP Eligibility
    ///  - MTX  Malta 1939
    ///  - MWI  Malawi
    ///  - N    New Zealand 1914
    ///  - NF   Newfoundland
    ///  - NG   New Guinea Civilian War Pension
    ///  - NGR  Nigeria
    ///  - NK   New Zealand Korea-Malaya
    ///  - NRD  Northern Rhodesia
    ///  - NSM  New Zealand Serving Members
    ///  - NSS  New Zealand Special Overseas Service
    ///  - NSW  New Zealand Merchant Navy
    ///  - NX   New Zealand 1939
    ///  - P    British Pension 1914
    ///  - PAD  British Admiralty Pension
    ///  - PAM  British Air Ministry Pension
    ///  - PCA  Governments and Administration
    ///  - PCR  British Service Department – CRO
    ///  - PCV  British Pension Civilian
    ///  - PK   British Korea/Malaya
    ///  - PMS  British Merchant Seaman 1914
    ///  - PSM  British Serving Members
    ///  - PSW  British Merchant Seaman 1939
    ///  - PWO  British War Offices Pension
    ///  - PX   British Pension 1939
    ///  - Q    Query
    ///  - RD   Southern Rhodesia 1914
    ///  - RDX  Southern Rhodesia 1939
    ///  - SA   South African Forces 1914
    ///  - SAX  South African Forces 1939
    ///  - SL   Sierra Leone
    ///  - SM   Serving Member
    ///  - SR   Far East Strategic Reserve
    ///  - SS   Special Overseas Act
    ///  - SUD  Sudan
    ///  - SWP  Seamans War Pension 1939
    ///  - TZA  Tanganyika(Tanzania)
    ///  - X    Australian Forces 1939
    ///  - ZZ   Zanzibar
    ///    
    /// * The numeric file number follows the War Code (or the State ID where no War Code exists) and consists 
    ///   of up to 6 numeric characters.
    ///   
    /// * The dependency indicator is the last item of the DVA number. 
    ///   It consists of a single alphabetic character (or space where no dependency exists).
    ///   
    /// * There is NO check digit in this identifier.
    /// 
    /// Keep in mind when validating this identifier that others wars may have been added to the list above 
    /// (and will, unfortunately, be added in the future). You should be able to determine that the identifier 
    /// appears to be in a valid format if it has 1–3 characters for the War Code, even if you can’t recognise the code.
    /// </remarks>
    public class DvaFileNumber : IIdentifier
    {

        private const string STATE_IDS = "NSWTVQ";

        private string[] warIds = new string[]
        {
            "", // World War I
            "A", // Allied Forces
            "AGX", // Act of Grace 1939
            "BUR", // Burma
            "BW", // Boer War
            "CGW", // Commonwealth Gulf War
            "CIV", // Civilians
            "CN", // Canadian Forces 1914
            "CNK", // Canada Korea/Malaya
            "CNS", // Canada Special Overseas Service
            "CNX", // Canadian Forces 1939
            "FIJ", // Fiji
            "GHA", // Ghana
            "GW", // Australian Gulf War
            "HKS", // Hong Kong (SP Eligibility)
            "HKX", // Hong Kong 1939
            "IND", // India
            "IV", // Indigenous Veteran 1939
            "KM", // Korea-Malaya
            "KYA", // Kenya
            "MAL", // Malaya-Singapore
            "MAU", // Mauritius
            "MLS", // Malaysia – Singapore SP Eligibility
            "MTX", // Malta 1939
            "MWI", // Malawi
            "N", // New Zealand 1914
            "NF", // Newfoundland
            "NG", // New Guinea Civilian War Pension
            "NGR", // Nigeria
            "NK", // New Zealand Korea-Malaya
            "NRD", // Northern Rhodesia
            "NSM", // New Zealand Serving Members
            "NSS", // New Zealand Special Overseas Service
            "NSW", // New Zealand Merchant Navy
            "NX", // New Zealand 1939
            "P", // British Pension 1914
            "PAD", // British Admiralty Pension
            "PAM", // British Air Ministry Pension
            "PCA", // Governments and Administration
            "PCR", // British Service Department – CRO
            "PCV", // British Pension Civilian
            "PK", // British Korea/Malaya
            "PMS", // British Merchant Seaman 1914
            "PSM", // British Serving Members
            "PSW", // British Merchant Seaman 1939
            "PWO", // British War Offices Pension
            "PX", // British Pension 1939
            "Q", // Query
            "RD", // Southern Rhodesia 1914
            "RDX", // Southern Rhodesia 1939
            "SA", // South African Forces 1914
            "SAX", // South African Forces 1939
            "SL", // Sierra Leone
            "SM", // Serving Member
            "SR", // Far East Strategic Reserve
            "SS", // Special Overseas Act
            "SUD", // Sudan
            "SWP", // Seamans War Pension 1939
            "TZA", // Tanganyika (Tanzania)
            "X", // Australian Forces 1939
            "ZZ", // Zanzibar
        };
        


        /// <summary>
        /// DVA Regex
        /// </summary>
        private static readonly Regex dvaRegex = new Regex(@"[" + STATE_IDS + @"][A-Z]{0,3}\d{1,6}[A-Z]?");


        public int MinLength => 8;

        public int MaxLength => 9;

        public IdentifierType IdType => IdentifierType.DVA;

        public string GenerateId()
        {
            string id = string.Concat(Shared.GenerateRandomFromChars(1, STATE_IDS),
                        warIds[Shared.GenerateRandomNumber(0, warIds.Length)],
                        Shared.GenerateRandomNumberString(6),
                        Shared.GenerateRandomFromChars(1, "ABC"));
            return id;

        }

        /// <summary>Validates a DVA File Number</summary>
        /// <param name="dva">DVA File Number to validate</param>
        /// <returns>Whether the number is symantically correct</returns>
        /// <exception cref="ArgumentException"></exception>
        public bool ValidateId(string dva)
        {
            if (string.IsNullOrEmpty(dva))
            {
                throw new ArgumentException($"'{nameof(dva)}' cannot be null or empty.", nameof(dva));
            }
            dva = dva.ToUpper().Trim();
            return dvaRegex.IsMatch(dva);
        }
    }
}
