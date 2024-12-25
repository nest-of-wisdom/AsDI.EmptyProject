namespace AsDI.Observer.Attributes
{
    [System.AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class ObserveAttribute : Attribute
    {
        public Type? Type { get; private set; }

        public string MethodName { get; private set; }

        /// <summary>
        /// 参数
        /// </summary>
        public string[]? Parameters { get; set; }

        public ObserveAttribute(string typeName, string methodName)
        {
            this.Type = Type.GetType(typeName);
            this.MethodName = methodName;
        }

        public ObserveAttribute(Type type, string methodName)
        {
            this.Type = type;
            this.MethodName = methodName;
        }
    }
}
