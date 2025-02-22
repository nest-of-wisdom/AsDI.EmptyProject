namespace AsDI.Log
{
    public class SurroundLog : IDisposable
    {
        /// <summary>
        /// 当前日志信息
        /// </summary>
        public LogInfo LogInfo { private set; get; }

        private bool ended = false;

        public SurroundLog(string message)
        {
            ended = false;
            var logInfo = new LogInfo(message);
            this.LogInfo = logInfo;
            Logger.LogStart(logInfo);
        }

        public SurroundLog(string message, string serviceName, string methodName)
        {
            ended = false;
            var logInfo = new LogInfo(message, serviceName, methodName);
            this.LogInfo = logInfo;
            Logger.LogStart(logInfo);
        }

        public SurroundLog(LogInfo log)
        {
            ended = false;
            this.LogInfo = log;
            Logger.LogStart(log);
        }

        public void LogError(Exception ex)
        {
            this.LogInfo.Exception = ex;
            Logger.LogEnd(ex);
            ended = true;
        }

        public void LogEnd(object result)
        {
            this.LogInfo.Result = result;
            Logger.LogEnd(result);
            ended = true;
        }

        public void Dispose()
        {
            if (!ended)
            {
                //surround log 的执行结果需要参考之前的日志
                this.LogInfo.LogLevel = Logger.Last?.LogLevel ?? LogLevel.Normal;
                this.LogInfo.Exception = Logger.Last?.Exception;
                Logger.LogEnd("");
            }
            GC.SuppressFinalize(this);
        }

    }
}
