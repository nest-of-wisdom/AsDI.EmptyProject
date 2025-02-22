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

        private static AsyncLocal<bool> writting;

        private static AsyncLocal<LogInfo> last;

        static Logger()
        {
            logStack = new AsyncLocal<Stack<LogInfo>>();
            currentTrace = new AsyncLocal<string>();
            logTranceId = new AsyncLocal<string>();
            extraInfos = new AsyncLocal<Dictionary<string, object>>();
            writting = new AsyncLocal<bool>();
            last = new AsyncLocal<LogInfo>();

        }

        public static void LogStart(LogInfo log)
        {
            logStack.Value ??= new Stack<LogInfo>();
            log.TraceId = TranceId;
            var trace = log.ClassName ?? "";
            if (!string.IsNullOrEmpty(log.MethodName))
            {
                trace += (trace.Length > 0 ? "." : "") + log.MethodName;
            }
            log.CurrentTrace = InTrace(trace);
            log.StartTime = DateTime.Now;
            log.ExtraInfo = extraInfos.Value;
            try
            {
                writting.Value = true;
                writer?.Begin(log);
                writting.Value = false;
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

                if (item.Duration > item.Timeout)
                {
                    item.LogLevel = LogLevel.Timeout;
                }
                else
                {
                    item.LogLevel = LogLevel.Normal;
                }

                if (item.Exception != null)
                {
                    item.LogLevel = LogLevel.Error;
                }

                try
                {
                    writting.Value = true;
                    writer?.End(item);
                    writting.Value = false;
                }
                catch { }

                last.Value = item;

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

        public static bool IsWritting
        {
            get
            {
                return writting.Value;
            }
            set
            {
                writting.Value = value;
            }
        }

        public static LogInfo? Last
        {
            get
            {
                return last.Value;
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

        public static object? GetExtraInfo(string key)
        {
            if (extraInfos.Value == null)
            {
                return null;
            }
            else
            {
                return extraInfos.Value[key];
            }
        }

    }
}
