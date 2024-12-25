using AsDI.Attributes;
using AsDI.DbExtend.Attributes;
using AsDI.Interceptor;
using AsDI.Proxy;

namespace AsDI.DbExtend
{
    [Fit(@"attr('AsDI.+RepositoryAttribute')", 1)]
    public class QueryInterceptor : IInterceptor
    {
        private readonly IDbExecutor nativeExecutor;
        private static readonly Type nativeType = typeof(NativeExecuteAttribute);

        public QueryInterceptor(IDbExecutor executor)
        {
            this.nativeExecutor = executor;
        }

        public object Around(AspectEntity aspect)
        {
            //有最终实现或者有有默认实现
            if (aspect.Method.GetTargetAnalyzer(aspect.Target).FinalTarget() != null || !aspect.Method.Method.IsAbstract)
            {
                try
                {
                    var rtn = aspect.Continue();
                    return rtn;
                }
                catch (NotImplementedException) { }
                catch
                {
                    throw;
                }
            }

            if (aspect.Method.DeclaringType != aspect.Method.BaseType)
            {
                var obj = AsDIContext.Get(aspect.Method.DeclaringType);
                if (obj != null)
                {
                    try
                    {
                        var rtn = DynamicProxyBase.BaseInvoke(aspect.Instance, obj, aspect.Method);
                        return rtn;
                    }
                    catch (NotImplementedException) { }
                }
            }

            var native = aspect.Method.CustAttributes.FirstOrDefault(p => p.GetType() == nativeType);
            if (native != null)
            {
                Dictionary<string, object> ps = new();
                foreach (var item in aspect.Method.Arguments)
                {
                    ps.Add(item.Name, item.Value);
                }

                string sql = ((NativeExecuteAttribute)native).Sql;
                SqlType type = ((NativeExecuteAttribute)native).SqlType;
                switch (type)
                {
                    case SqlType.DQL: return nativeExecutor.Query(sql, ps, aspect.Method.ReturnType);
                    case SqlType.DML:
                        {
                            if (aspect.Method.ReturnType != typeof(int) && aspect.Method.ReturnType != typeof(int?) && aspect.Method.ReturnType != typeof(void))
                            {
                                throw new Exception("Return Type of DML only allow 'int' or 'void'");
                            }
                            int rtn = nativeExecutor.Modify(sql, ps);
                            if (aspect.Method.ReturnType == typeof(int) || aspect.Method.ReturnType == typeof(int?))
                            {
                                return rtn;
                            }
                            else
                            {
                                return null;
                            }
                        }
                    case SqlType.DDL:
                    case SqlType.DCL:
                        {
                            if (aspect.Method.ReturnType != typeof(int) && aspect.Method.ReturnType != typeof(int?) && aspect.Method.ReturnType != typeof(void))
                            {
                                throw new Exception("Return Type of DDL or DCL only allow 'void'");
                            }
                            nativeExecutor.Modify(sql, ps);
                            return null;
                        }
                }
                return null;
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
