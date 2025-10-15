namespace AuHealthIds
{
    [TestClass()]
    public class PrescriberNumberTests
    {

        PrescriberNumber id = new PrescriberNumber();
        [DataTestMethod]
        [DataRow("2603485", true)]
        [DataRow("3326185", true)]
        [DataRow("3058925", true)]
        [DataRow("ABCDEFG", false)]
        //[DataRow("0000000", false)] // valid!?
        //[DataRow("1234567", false)] // Valid!?
        [DataRow("0", false)]
        public void ValidateTest(string prescriberNumber, bool expected)
        {
            var result = id.ValidateId(prescriberNumber);
            Assert.AreEqual(expected, result);
        }


        [TestMethod()]
        public void BlankTest()
        {
            Assert.ThrowsException<ArgumentException>(() => id.ValidateId(""));
            Assert.ThrowsException<ArgumentException>(() => id.ValidateId(null));
        }

    }
}