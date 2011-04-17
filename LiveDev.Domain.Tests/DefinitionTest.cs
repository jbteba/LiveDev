using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LiveDev.Domain.Tests
{
    [TestClass]
    public class DefinitionTest
    {
        [TestMethod]
        public void ToString_ReturnsAFormatedContractDefinition()
        {
            var definition = new Definition
                                 {ClassName = "stubClass", MethodName = "stubMethod", ReturnValue = "stubType"};

            Assert.AreEqual(
                "public class " + definition.ClassName + "{public " + definition.ReturnValue + " " +
                definition.MethodName + "(){//put content here.}}", definition.ToString());
        }
    }
}
