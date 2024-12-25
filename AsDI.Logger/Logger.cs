using AsDI.Attributes;

namespace AsDI.Log
{
    [Include]
    public class Logger
    {
        [AutoAssemble(require: false)]
        private static ILogWriter? writer;

        private static AsyncLocal<Stack<LogInfo>> logStack;

        private static AsyncLocal<string> logTranceId;

        private static AsyncLocal<string> currentTrace;

        private static AsyncLocal<Dictionary<string, object>> extraInfos;

        static Logger()
        {
            logStack = new AsyncLocal<Stack<LogInfo>>();
            currentTrace = new AsyncLocal<string>();
            logTranceId = new AsyncLocal<string>();
            extraInfos = new AsyncLocal<Dictionary<string, object>>();
        }

        public static void LogStart(LogInfo log)
        {
            if (logStack.Value == null)
            {
                logStack.Value = new Stack<LogInfo>();
            }
            log.TraceId = TranceId;
            log.CurrentTrace = InTrace(log.ClassName + "." + log.MethodName);
            log.StartTime = DateTime.Now;
            log.ExtraInfo = extraInfos.Value;
            try
            {
                writer?.Begin(log);
            }
            catch { }
            logStack.Value?.Push(log);
        }

        public static void LogEnd(object result)
        {
            LogEnd(result, null);
        }

        public static void LogEnd(Exception exception)
        {
            LogEnd(null, exception);
        }

        public static void LogEnd(object? result, Exception? exception)
        {
            var item = logStack.Value?.Pop();
            if (item != null)
            {
                item.EndTime = DateTime.Now;
                item.Duration = (item.EndTime - item.StartTime)?.TotalMilliseconds;
                item.Result = result;
                item.Exception = exception;
                item.ExtraInfo = extraInfos.Value;
                if (item.Exception != null)
                {
                    item.LogLevel = LogLevel.Error;
                }
                else
                {
                    item.LogLevel = LogLevel.Normal;
                }

                if (item.Duration > item.Timeout)
                {
                    item.LogLevel = item.LogLevel | LogLevel.Timeout;
                }

                try
                {
                    writer?.End(item);
                }
                catch { }

            }
            OutTrace();
        }

        public static string TranceId
        {
            get
            {
                if (string.IsNullOrEmpty(logTranceId.Value))
                {
                    logTranceId.Value = Guid.NewGuid().ToString("N").ToUpper();
                }
                return logTranceId.Value;
            }
            set
            {
                logTranceId.Value = value;
            }
        }

        public static string CurrentTrace
        {
            get
            {
                return currentTrace.Value ?? "";
            }
        }

        private static string InTrace(string trace)
        {
            var current = CurrentTrace;
            if (current.Length > 0)
            {
                current += "->" + trace;
            }
            else
            {
                current = trace;
            }
            currentTrace.Value = current;
            return CurrentTrace;
        }

        private static string OutTrace()
        {
            var current = CurrentTrace;
            if (current.Length > 0)
            {
                var index = current.LastIndexOf("->") + 1;
                if (index > 0)
                {
                    current = CurrentTrace.Substring(0, index);
                }
                else
                {
                    current = "";
                }
                currentTrace.Value = current;
                return current;
            }
            else
            {
                return "";
            }
        }

        public static void AddExtraInfo(string key, object value)
        {
            if (extraInfos.Value == null)
            {
                extraInfos.Value = new Dictionary<string, object>();
            }
            extraInfos.Value[key] = value;
        }

        public static void AddExtraInfo(IDictionary<string, object> extraInfo)
        {
            var current = extraInfos.Value;
            if (current == null)
            {
                current = new Dictionary<string, object>();
                extraInfos.Value = current;
            }
            foreach (var kv in extraInfo)
            {
                current[kv.Key] = kv.Value;
            }
        }

    }
}
