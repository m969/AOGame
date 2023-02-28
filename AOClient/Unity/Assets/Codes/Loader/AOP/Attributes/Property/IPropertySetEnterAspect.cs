using ITnnovative.AOP.Processing.Execution.Arguments;

namespace ITnnovative.AOP.Attributes.Property
{
    public interface IPropertySetEnterAspect : IPropertySetAspect
    {
        void OnPropertySetEnter(PropertyExecutionArguments args);
    }
}