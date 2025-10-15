namespace AuHealthIds
{
    public interface IIdentifier
    {
        int MinLength { get; }
        int MaxLength { get; }

        IdentifierType IdType { get; }

        /// <summary>
        /// Validates an ID
        /// </summary>
        /// <param name="id">ID to validate</param>
        /// <returns>Bool - true if valid, false if not</returns>
        bool ValidateId(string id);

        string GenerateId();

    }
}
