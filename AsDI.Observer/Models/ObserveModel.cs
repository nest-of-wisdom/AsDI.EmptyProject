using AsDI.Observer.Attributes;
using System.Reflection;

namespace AsDI.Observer.Models
{
    public class ObserveModel
    {
        public string? ObserveTarget { get; set; }

        public string[]? ObserveParameters { get; set; }
        public object? Observer { get; set; }

        public MethodInfo? Method { get; set; }

        public List<ObserveParamModel>? Parameters { get; set; }

    }

    public class ObserveParamModel
    {
        public string? Name { get; set; }

        public int? Index { get; set; }

    }
}
