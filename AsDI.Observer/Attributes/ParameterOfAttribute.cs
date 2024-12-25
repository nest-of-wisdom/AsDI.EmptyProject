namespace AsDI.Observer.Attributes
{
    [System.AttributeUsage(AttributeTargets.Parameter, Inherited = false, AllowMultiple = false)]
    public class ParameterOfAttribute : Attribute
    {
        public string? ParameterName { get; private set; }

        public int? ParameterIndex { get; private set; }

        public ParameterOfAttribute(string parameterName)
        {
            ParameterName = parameterName;
        }

        public ParameterOfAttribute(uint parameterIndex)
        {
            ParameterIndex = (int)parameterIndex;
        }

        protected ParameterOfAttribute()
        {
            ParameterIndex = -1;
        }
    }

    public class ReturnAttribute : ParameterOfAttribute
    {
        public ReturnAttribute() : base() { }
    }

}
