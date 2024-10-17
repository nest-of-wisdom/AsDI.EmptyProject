namespace AsDI.DbExtend.Attributes
{
    [System.AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class AutoTransAttribute : Attribute
    {
        public AutoTransAttribute()
        {
            RollBackFor = new Type[] { typeof(Exception) };
            ContinueFor = null;
        }

        public Type[] RollBackFor { get; set; }

        public Type[] ContinueFor { get; set; } = null;

        public bool AutoRollBack(Exception ex)
        {
            var exType = ex.GetType();
            if (ContinueFor != null)
            {
                foreach (var t in ContinueFor)
                {
                    if (t.IsAssignableFrom(exType))
                    {
                        return false;
                    }
                }
            }
            foreach (var t in RollBackFor)
            {
                if (t.IsAssignableFrom(exType))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
