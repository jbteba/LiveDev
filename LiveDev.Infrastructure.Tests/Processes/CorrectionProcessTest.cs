using System.CodeDom.Compiler;
using System.Reflection;
using AtentoFramework2008.TestTools.Helpers;
using LiveDev.Domain;
using LiveDev.Infrastructure.Processes;
using LiveDev.Infrastructure.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace LiveDev.Infrastructure.Tests.Processes
{
    [TestClass]
    public class CorrectionProcessTest
    {
        [TestMethod]
        public void CorrectionProcess_InitializesCompilationService()
        {
            var correctionProcess = new CorrectionProcess();
            Assert.IsNotNull(correctionProcess.GetFieldValue<CompilationService>("_compilationService"));
        }

        [TestMethod]
        public void CorrectionProcess_InitializesRunnerService()
        {
            var correctionProcess = new CorrectionProcess();
            Assert.IsNotNull(correctionProcess.GetFieldValue<RunnerService>("_runnerService"));
        }

        [TestMethod]
        public void CheckAnswer_CompileSourceCode()
        {
            var mockCompilationService = MockRepository.GenerateMock<CompilationService>();
            mockCompilationService.Stub(s => s.CompileSourceCode(Arg<string>.Is.Anything)).Return(
                MockRepository.GenerateStub<CompilerResults>(new TempFileCollection()));
            var stubTestsRunnerService = MockRepository.GenerateStub<RunnerService>();
            var stubQuestion = MockRepository.GenerateStub<Question>();
            stubQuestion.SourceCode = "answerSourceCode";

            var correctionProcess = new CorrectionProcess(mockCompilationService, stubTestsRunnerService);
            correctionProcess.CheckAnswer(stubQuestion);

            mockCompilationService.AssertWasCalled(m => m.CompileSourceCode(stubQuestion.SourceCode));
        }

        [TestMethod]
        public void CheckAnswer_WhenExistsCompilationErrors_SetsErrorList()
        {
            var stubCompilationService = new CompilationService();
            CompilerResults compilerResults = stubCompilationService.CompileSourceCode("NotCompilingSourceCode");
            var stubTestsRunnerService = MockRepository.GenerateStub<RunnerService>();
            
            var correctionProcess = new CorrectionProcess(stubCompilationService, stubTestsRunnerService);
            CorrectionResult correctionResult =
                correctionProcess.CheckAnswer(new Question {SourceCode = "NotCompilingSourceCode"});

            Assert.AreEqual(compilerResults.Errors.Count, correctionResult.Errors.Count);
            Assert.AreEqual(compilerResults.Errors[0].ErrorText, correctionResult.Errors[0]);
            Assert.AreEqual(compilerResults.Errors[compilerResults.Errors.Count - 1].ErrorText,
                            correctionResult.Errors[correctionResult.Errors.Count - 1]);
        }

        [TestMethod]
        public void CheckAnswer_RunSourceCode()
        {
            var mockRunnerService = MockRepository.GenerateMock<RunnerService>();
            var stubQuestion = MockRepository.GenerateStub<Question>();
            var stubCompilationService = MockRepository.GenerateMock<CompilationService>();
            var stubAssembly = Assembly.GetCallingAssembly();
            var stubCompilerResults = MockRepository.GenerateStub<CompilerResults>(new TempFileCollection());
            stubCompilerResults.CompiledAssembly = stubAssembly;
            stubCompilationService.Stub(s => s.CompileSourceCode(stubQuestion.SourceCode)).Return(
                stubCompilerResults);

            var correctionProcess = new CorrectionProcess(stubCompilationService, mockRunnerService);
            correctionProcess.CheckAnswer(stubQuestion);

            mockRunnerService.AssertWasCalled(m => m.RunMethod(stubAssembly, stubQuestion));
        }
    }
}
