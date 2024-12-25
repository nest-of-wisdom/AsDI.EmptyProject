using AsDI.Attributes;

namespace AsDI.Observer.Attributes
{
    [System.AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class ObserverAttribute : IncludeAttribute
    {
    }
}
