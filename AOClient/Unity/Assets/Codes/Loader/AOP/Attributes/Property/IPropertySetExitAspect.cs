using ITnnovative.AOP.Processing.Execution.Arguments;

namespace ITnnovative.AOP.Attributes.Property
{
    public interface IPropertySetExitAspect : IPropertySetAspect
    {
        void OnPropertySetExit(PropertyExecutionArguments args);
    }
}