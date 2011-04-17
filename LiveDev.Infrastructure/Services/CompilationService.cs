using System;
using System.CodeDom.Compiler;
using System.Reflection;

namespace LiveDev.Infrastructure.Services
{
    public class CompilationService
    {
        private readonly CodeDomProvider _codeProvider;
        private readonly CompilerParameters _compilerParameters;

        public CompilationService()
        {
            _codeProvider = CodeDomProvider.CreateProvider("CSharp");
            _compilerParameters = new CompilerParameters {GenerateInMemory = true};
        }

        public CompilationService(CodeDomProvider codeProvider, CompilerParameters compilerParameters)
        {
            _codeProvider = codeProvider;
            _compilerParameters = compilerParameters;
        }

        public virtual CompilerResults CompileSourceCode(string sourceCode)
        {
            return _codeProvider.CompileAssemblyFromSource(_compilerParameters, sourceCode);

        }
    }
}