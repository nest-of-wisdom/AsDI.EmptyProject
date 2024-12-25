namespace AsDI.Log
{

    /// <summary>
    /// 不记录日志
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = false, AllowMultiple = false)]
    public sealed class NoLogAttribute : Attribute
    {
        public NoLogAttribute()
        {
        }
    }
}
