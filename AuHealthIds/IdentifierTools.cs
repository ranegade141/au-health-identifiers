using System.Collections.Generic;
using System.Linq;

namespace AuHealthIds
{
    public static class IdentifierTools
    {
        public readonly static IDictionary<IdentifierType, IIdentifier> IdCache = new Dictionary<IdentifierType, IIdentifier>()
        {
            { IdentifierType.IHI, new IndividualHealthIdentifier() },
            { IdentifierType.Prescriber, new PrescriberNumber() },
            { IdentifierType.DVA, new DvaFileNumber() },
            { IdentifierType.Medicare, new MedicareNumber() },
            { IdentifierType.AHPRA, new AhpraRegistrationNumber() },
            { IdentifierType.HPI_I, new HealthProviderIdentifierIndividual() },
            { IdentifierType.HPI_O, new HealthProviderIdentifierOrganisation() },
            { IdentifierType.Provider, new ProviderNumber() }
        };

        public static bool ValidateId(IdentifierType idType, string id)
        {
            var identifier = IdCache[idType];
            return identifier.ValidateId(id);
        }

        public static string GenerateId(IdentifierType idType)
        {
            var identifier = IdCache[idType];
            return identifier.GenerateId();
        }

        public static string[] GenerateIds(IdentifierType idType, int count)
        {
            var identifier = IdCache[idType];
            return Enumerable.Range(0, count).Select(_ => identifier.GenerateId()).ToArray();
        }

    }
}
