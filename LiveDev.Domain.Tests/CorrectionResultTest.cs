using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LiveDev.Domain.Tests
{
    [TestClass]
    public class CorrectionResultTest
    {
        [TestMethod]
        public void CorrectionResult_InitializesErrorsList()
        {
            var correctionResult = new CorrectionResult();
            Assert.IsNotNull(correctionResult.Errors);
        }

        [TestMethod]
        public void HasErrors_WhenExistsErrors_ReturnTrue()
        {
            var correctionResult = new CorrectionResult {Errors = new List<string> {"Error"}};
            var result = correctionResult.HasErrors();

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void HasErrors_WhenThereIsNoErrors_ReturnFalse()
        {
            var correctionResult = new CorrectionResult();
            var result = correctionResult.HasErrors();

            Assert.IsFalse(result);
        }
    }
}
