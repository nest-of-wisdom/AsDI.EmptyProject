using AsDI.Attributes;
using AsDI.Interceptor;
using AsDI.Observer.Attributes;
using AsDI.Observer.Models;
using System.Reflection;

namespace AsDI.Observer
{
    [Fit(typeof(AllowObserveAttribute))]
    public class ObserverInterceptor : IInterceptor
    {
        public readonly Dictionary<string, List<ObserveModel>> models;
        public ObserverInterceptor()
        {
            var objects = AsDIContext.Container.Where(p => p.BaseType.GetCustomAttribute<ObserverAttribute>() != null);

            models = new Dictionary<string, List<ObserveModel>>();
            foreach (var observer in objects)
            {
                var type = observer.BaseType;
                var methods = type.GetMethods();
                foreach (var method in methods)
                {
                    var attr = method.GetCustomAttribute<ObserveAttribute>();
                    if (attr != null)
                    {
                        if (attr.Type == null || string.IsNullOrEmpty(attr.MethodName))
                        {
                            continue;
                        }

                        ObserveModel model = new ObserveModel()
                        {
                            ObserveTarget = attr.Type?.FullName + "." + attr.MethodName,
                            ObserveParameters = attr.Parameters,
                            Method = method,
                            Observer = observer.Instance,
                        };

                        model.Parameters = method.GetParameters().Select(p =>
                        {
                            var item = new ObserveParamModel();

                            var name = p.Name;
                            var pof = p.GetCustomAttribute<ParameterOfAttribute>();
                            if (pof != null)
                            {
                                item.Name = pof.ParameterName;
                                item.Index = pof.ParameterIndex;
                            }
                            else
                            {
                                item.Name = name;
                            }

                            return item;

                        }).ToList();

                        if (models.ContainsKey(model.ObserveTarget))
                        {
                            var list = models[model.ObserveTarget];
                            list.Add(model);
                        }
                        else
                        {
                            models[model.ObserveTarget] = new List<ObserveModel>() { model };
                        }
                    }
                }
            }
        }

        public object Around(AspectEntity aspect)
        {
            var rtn = aspect.Continue();
            try
            {
                var key = aspect.Method.BaseType.FullName + "." + aspect.Method.Method.Name;
                if (models.ContainsKey(key))
                {
                    var list = models[key];

                    foreach (var model in list)
                    {
                        if (CheckParameters(model, aspect, rtn, out object[] ps))
                        {
                            model.Method!.Invoke(model.Observer, ps);
                        }
                    }
                }
                return rtn;
            }
            catch (TargetInvocationException ex)
            {
                throw ex.GetBaseException();
            }

        }

        private bool CheckParameters(ObserveModel model, AspectEntity aspect, object returnData, out object[] ps)
        {
            if (model.ObserveParameters != null)
            {
                if (model.ObserveParameters.Length != aspect.Method.Arguments.Length)
                {
                    ps = [];
                    return false;
                }
                else
                {
                    for (int i = 0; i < model.ObserveParameters.Length; i++)
                    {
                        var type1 = model.ObserveParameters[i];
                        var type2 = aspect.Method.Arguments[i].Type.FullName ?? "";

                        if (!type2.ToLower().EndsWith(type1) && type2 != type1)
                        {
                            ps = [];
                            return false;
                        }
                    }
                }
            }

            if (model.Parameters != null)
            {
                object[] rtn = new object[model.Parameters.Count];
                int i = 0;
                foreach (var item in model.Parameters)
                {
                    if (!string.IsNullOrEmpty(item.Name))
                    {
                        var arg = aspect.Method.Arguments.FirstOrDefault(p => p.Name == item.Name);
                        if (arg == null)
                        {
                            ps = [];
                            return false;
                        }
                        else
                        {
                            rtn[i] = arg.Value;
                        }
                    }
                    else if (item.Index != null)
                    {
                        if (item.Index < aspect.Method.Arguments.Length)
                        {
                            if (item.Index >= 0)
                            {
                                var arg = aspect.Method.Arguments[i];
                                rtn[i] = arg.Value;
                            }
                            else
                            {
                                rtn[i] = returnData;
                            }
                        }
                        else
                        {
                            ps = [];
                            return false;
                        }
                    }
                    else
                    {
                        ps = [];
                        return false;
                    }
                }

                ps = rtn;
                return true;
            }
            else
            {
                ps = [];
                if (aspect.Method.Arguments.Length == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
