using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace LiveDev.Domain.Tests
{
    [TestClass]
    public class QuestionTest
    {
        [TestMethod]
        public void QuestionTest_InitializesContractDefinition()
        {
            var stubDefinition = new Definition
                                     {ClassName = "stubClass", MethodName = "stubMethod", ReturnValue = "stubType"};      
            
            var question = new Question(stubDefinition);
                               
            Assert.AreEqual(stubDefinition, question.ContractDefinition);
        }

        [TestMethod]
        public void GetContractDefinitionSourceCode_ReturnsFormatedContractDefinitionSourceCode()
        {
            var stubDefinition = new Definition { ClassName = "stubClass", MethodName = "stubMethod", ReturnValue = "stubType" };

            var question = new Question(stubDefinition);
            var contractDefinitionSourceCode = question.GetContractDefinitionSourceCode();

            Assert.AreEqual(stubDefinition.ToString(),contractDefinitionSourceCode);
        }
    }
}
