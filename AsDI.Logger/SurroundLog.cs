namespace AsDI.Log
{
    public class SurroundLog : IDisposable
    {
        public SurroundLog(string message)
        {
            var logInfo = new LogInfo(message);
            Logger.LogStart(logInfo);
        }

        public SurroundLog(string message, string serviceName, string methodName)
        {
            var logInfo = new LogInfo(message, serviceName, methodName);
            Logger.LogStart(logInfo);
        }

        public void Dispose()
        {
            Logger.LogEnd("");
            GC.SuppressFinalize(this);
        }
    }
}
