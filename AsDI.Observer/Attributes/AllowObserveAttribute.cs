namespace AsDI.Observer.Attributes
{

    /// <summary>
    /// 指示当前类允许被观察
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface, Inherited = false, AllowMultiple = false)]
    public sealed class AllowObserveAttribute : Attribute
    {
    }
}
