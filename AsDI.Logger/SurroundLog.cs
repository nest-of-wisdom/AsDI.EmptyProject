namespace AsDI.Log
{
    public class SurroundLog : IDisposable
    {
        /// <summary>
        /// 当前日志信息
        /// </summary>
        public LogInfo LogInfo { private set; get; }

        public SurroundLog(string message)
        {
            var logInfo = new LogInfo(message);
            this.LogInfo = logInfo;
            Logger.LogStart(logInfo);
        }

        public SurroundLog(string message, string serviceName, string methodName)
        {
            var logInfo = new LogInfo(message, serviceName, methodName);
            this.LogInfo = logInfo;
            Logger.LogStart(logInfo);
        }

        public SurroundLog(LogInfo log)
        {
            this.LogInfo = log;
            Logger.LogStart(log);
        }

        public void Dispose()
        {
            //surround log 的执行结果需要参考之前的日志
            this.LogInfo.LogLevel = Logger.Last?.LogLevel ?? LogLevel.Normal;
            this.LogInfo.Exception = Logger.Last?.Exception;
            Logger.LogEnd("");
            GC.SuppressFinalize(this);
        }

    }
}
