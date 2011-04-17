using System.CodeDom.Compiler;
using LiveDev.Domain;
using LiveDev.Infrastructure.Services;

namespace LiveDev.Infrastructure.Processes
{
    public class CorrectionProcess
    {
        private readonly CompilationService _compilationService;
        private readonly RunnerService _runnerService;

        public CorrectionProcess()
        {
            _compilationService = new CompilationService();
            _runnerService = new RunnerService();
        }

        public CorrectionProcess(CompilationService compilationService, RunnerService runnerService)
        {
            _compilationService = compilationService;
            _runnerService = runnerService;
        }

        public virtual CorrectionResult CheckAnswer(Question question)
        {
            CompilerResults compiledSourceCode = _compilationService.CompileSourceCode(question.SourceCode);
            if(compiledSourceCode.Errors.HasErrors)
            {
                var correctionResult = new CorrectionResult();
                foreach (CompilerError compilationError in compiledSourceCode.Errors)
                {
                    correctionResult.Errors.Add(compilationError.ErrorText);
                }
                return correctionResult;
            }
            return _runnerService.RunMethod(compiledSourceCode.CompiledAssembly, question);
        }
    }
}