using AsDI.Attributes;

namespace AsDI.Observer.Attributes
{

    /// <summary>
    /// 当前类是一个观察者
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class ObserverAttribute : IncludeAttribute
    {
    }
}
