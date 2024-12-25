using AsDI.Attributes;

namespace AsDI.Log
{
    [Service]
    public class LogWriter : ILogWriter
    {
        public void Begin(LogInfo logInfo)
        {
            Console.WriteLine("[" + logInfo.TraceId + "]开始执行：" + logInfo.CurrentTrace);
        }
        public void End(LogInfo logInfo)
        {
            Console.WriteLine("[" + logInfo.TraceId + "]完成执行：" + logInfo.CurrentTrace + "[用时{0}]", logInfo.Duration);
        }
    }
}
