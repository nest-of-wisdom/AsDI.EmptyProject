using AsDI.Attributes;
using AsDI.Interceptor;
using System.Reflection;

namespace AsDI.Log
{
    [Fit("!attr('AsDI.+NoLogAttribute')", 999)]
    public class LogInterceptor : IInterceptor
    {
        public object Around(AspectEntity aspect)
        {
            // 正在写日志，说明当前的执行过程属于写日志的过程，不应当产生新的日志，否则会死循环
            if (Logger.IsWritting || NoLog(aspect))
            {
                return aspect.Continue();
            }

            var logInfo = new LogInfo(aspect);
            Logger.LogStart(logInfo);
            try
            {
                var value = aspect.Continue();
                Logger.LogEnd(value);
                return value;
            }
            catch (Exception ex)
            {
                Logger.LogEnd(ex);
                throw;
            }
        }

        private bool NoLog(AspectEntity aspect)
        {
            var nolog = aspect.TargetAnalyzer.FinalTargetType()?.GetCustomAttribute<NoLogAttribute>();
            if (nolog != null)
            {
                return true;
            }
            else
            {
                return aspect.TargetAnalyzer.FindAttribute<NoLogAttribute>() != null;
            }
        }

    }
}
