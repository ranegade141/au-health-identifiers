namespace AuHealthIds
{
    [TestClass]
    public class AhpraTests
    {
        AhpraRegistrationNumber ahpra = new AhpraRegistrationNumber();

        [DataTestMethod]
        // Real numbers sourced from AHPRA Website
        [DataRow("OPT0002667123", true)]
        [DataRow("MED0001180716", true)]
        [DataRow("PHA0000986946", true)]
        [DataRow("FAKE1223", false)]
        public void ValidationTests(string id, bool expectedResult)
        {
            var result = ahpra.ValidateId(id);
            Assert.AreEqual(result, expectedResult);
        }

        
    }
}
