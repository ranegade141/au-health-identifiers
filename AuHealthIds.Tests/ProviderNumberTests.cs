namespace AuHealthIds
{
    [TestClass()]
    public class ProviderNumberTests
    {
        private ProviderNumber identifier = new ProviderNumber();

        [DataTestMethod]

        [DataRow("021720RT", false)] // broken check
        // These are randomly generated
        [DataRow("162919DB", true)]
        [DataRow("141377RK", true)]
        [DataRow("117915TA", true)]
        [DataRow("167868WH", true)]
        [DataRow("1725943L", true)]
        [DataRow("146593QT", true)]
        [DataRow("142055JF", true)]
        [DataRow("127101AW", true)]
        [DataRow("1247132W", true)]
        [DataRow("137204PL", true)]
        [DataRow("1234", false)]
        [DataRow("ABCDEH", false)]
        [DataRow("000000AB", false)] 
        public void ValidateTest(string providerNumber, bool expected)
        {
            var result = identifier.ValidateId(providerNumber);
            Assert.AreEqual(expected, result);
        }


        [TestMethod()]
        public void SuccessTest()
        {
            var success = identifier.ValidateId("1260354F");
            Assert.IsTrue(success);
        }

        [TestMethod()]
        public void BlankTest()
        {
            Assert.ThrowsException<ArgumentException>(() => identifier.ValidateId(""));
            Assert.ThrowsException<ArgumentException>(() => identifier.ValidateId(null));
        }
    }
}