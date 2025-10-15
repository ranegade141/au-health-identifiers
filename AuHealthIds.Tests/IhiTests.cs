namespace AuHealthIds
{
    [TestClass()]
    public class IhiTests
    {
        IndividualHealthIdentifier id = new IndividualHealthIdentifier();

        
        [DataTestMethod]
        [DataRow("8003608166690503", true)] // Example from FHIR Working Group
        [DataRow("ABCDEFGHIJKLKMNO", false)]
        [DataRow("1234", false)]
        [DataRow("12345789ABCDEFGH", false)]
        [DataRow("8003604649852310", false)] // bad check value
        public void ValidateTest(string ihi, bool expected)
        {
            var result = id.ValidateId(ihi);
            Assert.AreEqual(expected, result);
        }

        [TestMethod()]
        public void WrongLengthTest()
        {
            Assert.IsFalse(id.ValidateId("12345"));
        }

        [TestMethod()]
        public void BlankTest()
        {
            Assert.ThrowsException<ArgumentException>(() => id.ValidateId(""));
            Assert.ThrowsException<ArgumentException>(() => id.ValidateId(null));
        }

        [TestMethod()]
        public void GenerateRandomIhiTest()
        {
            var testId = id.GenerateId();
            Assert.IsNotNull(id);
            Assert.AreEqual(id.MaxLength, testId.Length);
            Assert.IsTrue(id.ValidateId(testId));
        }

        [TestMethod()]
        public void GenerateRandomIhisUniqueTest()
        {
            var ids = new HashSet<string>();
            for (int i = 0; i < 10000; i++)
            {
                var testId = id.GenerateId();
                Assert.IsFalse(ids.Contains(testId), $"Duplicate IHI found: {testId}");
                ids.Add(testId);
            }
        }

      
    }
}