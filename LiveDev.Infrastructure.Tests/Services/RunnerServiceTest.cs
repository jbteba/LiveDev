using System.CodeDom.Compiler;
using System.Reflection;
using LiveDev.Domain;
using LiveDev.Infrastructure.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LiveDev.Infrastructure.Tests.Services
{
    [TestClass]
    public class RunnerServiceTest
    {
        [TestMethod]
        public void RunMethod_RunSourceCodeAndReturnTheResult()
        {
            const string sourceCode = "public class stubClass{ public int stubMethod(){ return 1; }}";
            var stubQuestion = new Question(new Definition { ClassName = "stubClass", MethodName = "stubMethod" })
            {
                SourceCode = sourceCode
            };
            var compilationService = new CompilationService();
            CompilerResults compilerResults = compilationService.CompileSourceCode(stubQuestion.SourceCode);

            var runnerService = new RunnerService();
            var result = runnerService.RunMethod(compilerResults.CompiledAssembly, stubQuestion);

            Assert.AreEqual("1", result.Result);
        }
    }
}
