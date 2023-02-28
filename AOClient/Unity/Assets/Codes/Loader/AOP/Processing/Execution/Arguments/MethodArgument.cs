namespace ITnnovative.AOP.Processing.Execution.Arguments
{
    public class MethodArgument
    {
        public string name;
        public object value;

        public MethodArgument(string n, object v)
        {
            name = n;
            value = v;
        }
    }
}