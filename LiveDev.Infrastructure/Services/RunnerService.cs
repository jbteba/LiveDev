using System.Reflection;
using System.Runtime.InteropServices;
using LiveDev.Domain;

namespace LiveDev.Infrastructure.Services
{
    public class RunnerService
    {
        public virtual CorrectionResult RunMethod(_Assembly assembly, Question question)
        {
            var instance = assembly.CreateInstance(question.ContractDefinition.ClassName);
            var type = instance.GetType();
            MethodInfo methodInfo = type.GetMethod(question.ContractDefinition.MethodName);
            return new CorrectionResult {Result = methodInfo.Invoke(instance, null).ToString()};
        }
    }
}