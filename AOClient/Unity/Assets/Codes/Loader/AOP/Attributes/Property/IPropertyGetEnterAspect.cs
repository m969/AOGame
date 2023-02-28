using ITnnovative.AOP.Processing.Execution.Arguments;

namespace ITnnovative.AOP.Attributes.Property
{
    public interface IPropertyGetEnterAspect : IPropertyGetAspect
    {

        void OnPropertyGetEnter(PropertyExecutionArguments args);

    }
}