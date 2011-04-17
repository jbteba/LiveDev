using System.CodeDom.Compiler;
using AtentoFramework2008.TestTools.Helpers;
using LiveDev.Infrastructure.Services;
using Microsoft.CSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace LiveDev.Infrastructure.Tests.Services
{
    [TestClass]
    public class CompilationServiceTest
    {
        [TestMethod]
        public void CompilationService_InitializeCodeProvider()
        {
            var compilationService = new CompilationService();
            Assert.IsNotNull(compilationService.GetFieldValue<CodeDomProvider>("_codeProvider"));
        }

        [TestMethod]
        public void CompilationService_InitializeCompilerParameters()
        {
            var compilationService = new CompilationService();
            Assert.IsNotNull(compilationService.GetFieldValue<CompilerParameters>("_compilerParameters"));
        }

        [TestMethod]
        public void CompilationService_AssemblyGeneratedInMemory()
        {
            var compilationService = new CompilationService();
            var compilerParameters = compilationService.GetFieldValue<CompilerParameters>("_compilerParameters");
            Assert.IsTrue(compilerParameters.GenerateInMemory);
        }

        [TestMethod]
        public void CompileSourceCode_CompileAssemblyFromSource()
        {
            var mockCodeProvider = MockRepository.GenerateMock<CSharpCodeProvider>();
            mockCodeProvider.Stub(
                s => s.CompileAssemblyFromSource(Arg<CompilerParameters>.Is.Anything, Arg<string>.Is.Anything)).Return(
                    new CompilerResults(new TempFileCollection()));

            var stubParameters = new CompilerParameters();
            const string stubSourceCode = "sourceCode";

            var compilationService = new CompilationService(mockCodeProvider, stubParameters);
            compilationService.CompileSourceCode(stubSourceCode);

            mockCodeProvider.AssertWasCalled(m => m.CompileAssemblyFromSource(stubParameters, stubSourceCode));
        }
    }
}
