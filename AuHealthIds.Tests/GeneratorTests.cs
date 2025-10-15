namespace AuHealthIds
{
    [TestClass()]
    public class GeneratorTests
    {
        [DataTestMethod]
        [DataRow(IdentifierType.AHPRA)]
        [DataRow(IdentifierType.DVA)]
        [DataRow(IdentifierType.IHI)]
        [DataRow(IdentifierType.HPI_I)]
        [DataRow(IdentifierType.HPI_O)]
        [DataRow(IdentifierType.AHPRA)]
        [DataRow(IdentifierType.Medicare)]
        [DataRow(IdentifierType.Prescriber)]
        [DataRow(IdentifierType.Provider)]        
        public void TestGenerators(IdentifierType type)
        {
            // run each test 1000 times
            for (int i = 0; i < 1000; i++)
            {
                var id = IdentifierTools.GenerateId(type);
                Assert.IsNotNull(id);
                Assert.IsTrue(IdentifierTools.ValidateId(type, id), $"Generated ID {id} of type {type} did not validate");
            }
            
            
        }
    }
}
