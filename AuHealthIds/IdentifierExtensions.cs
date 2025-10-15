namespace AuHealthIds
{
    public static class IdentifierExtensions
    {
        /// <summary>
        /// Validates an Individual Health Identifier (IHI)
        /// </summary>
        /// <param name="ihi"></param>
        /// <returns></returns>
        public static bool IsValidIhi(this string ihi)
        {
            var id = new IndividualHealthIdentifier();
            return id.ValidateId(ihi);
        }

        /// <summary>
        /// Validates a Prescriber Number
        /// </summary>
        /// <param name="prescriberNumber"></param>
        /// <returns></returns>
        public static bool IsValidPrescriberNumber(this string prescriberNumber)
        {
            var id = new PrescriberNumber();
            return id.ValidateId(prescriberNumber);
        }

        /// <summary>
        /// Validates an AHPRA Registration Number
        /// </summary>
        /// <param name="ahpraNumber"></param>
        /// <returns></returns>
        public static bool IsValidAhpraRegistrationNumber(this string ahpraNumber)
        {
            var id = new AhpraRegistrationNumber();
            return id.ValidateId(ahpraNumber);
        }

        /// <summary>
        /// Validates a DVA File Number
        /// </summary>
        /// <param name="dvaFileNumber"></param>
        /// <returns></returns>
        public static bool IsValidDvaFileNumber(this string dvaFileNumber)
        {
            var id = new DvaFileNumber();
            return id.ValidateId(dvaFileNumber);
        }

        /// <summary>
        /// Validates a Medicare Number
        /// </summary>
        /// <param name="medicareNumber"></param>
        /// <returns></returns>
        public static bool IsValidMedicareNumber(this string medicareNumber)
        {
            var id = new MedicareNumber();
            return id.ValidateId(medicareNumber);
        }

        /// <summary>
        /// Validates a Health Provider Identifier - Individual (HPI-I)
        /// </summary>
        /// <param name="hpiIndividual"></param>
        /// <returns></returns>
        public static bool IsValidHpiIndividual(this string hpiIndividual)
        {
            var id = new HealthProviderIdentifierIndividual();
            return id.ValidateId(hpiIndividual);
        }

        /// <summary>
        /// Validates a Health Provider Identifier - Organisation (HPI-O)
        /// </summary>
        /// <param name="hpiOrganisation"></param>
        /// <returns></returns>
        public static bool IsValidHpiOrganisation(this string hpiOrganisation)
        {
            var id = new HealthProviderIdentifierOrganisation();
            return id.ValidateId(hpiOrganisation);
        }

        /// <summary>
        /// Validates a Provider Number
        /// </summary>
        /// <param name="providerNumber"></param>
        /// <returns></returns>
        public static bool IsValidProviderNumber(this string providerNumber)
        {
            var id = new ProviderNumber();
            return id.ValidateId(providerNumber);
        }


    }
}
