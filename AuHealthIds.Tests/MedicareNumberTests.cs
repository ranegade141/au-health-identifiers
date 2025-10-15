namespace AuHealthIds
{
    [TestClass()]
    public class MedicareNumberTests
    {
        public MedicareNumber medicare = new MedicareNumber();
        [DataTestMethod]
        // Dummy test cases
        [DataRow("68696670616", true)]
        [DataRow("30166688634", true)]
        [DataRow("1234567890", false)]
        [DataRow("4205987980", false)]
        [DataRow("a487980", false)]
        public void ValidateTest(string medicareNumber, bool expected)
        {
            var result = medicare.ValidateId(medicareNumber);
            Assert.AreEqual(expected, result);
        }


        [TestMethod()]
        public void BlankTest()
        {
            Assert.ThrowsException<ArgumentException>(() => medicare.ValidateId(""));
            Assert.ThrowsException<ArgumentException>(() => medicare.ValidateId(null));
        }
    }
}