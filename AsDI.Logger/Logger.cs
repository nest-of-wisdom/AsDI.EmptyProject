using AsDI.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsDI.Logger
{
    [Include]
    public class Logger
    {
        [AutoAssemble(require: false)]
        private static ILogWriter? writer;

        private static AsyncLocal<Stack<LogInfo>> logStack;

        private static AsyncLocal<string> logTranceId;

        private static AsyncLocal<string> currentTrace;

        static Logger()
        {
            logStack = new AsyncLocal<Stack<LogInfo>>();
            currentTrace = new AsyncLocal<string>();
            logTranceId = new AsyncLocal<string>();
        }

        public static void LogStart(LogInfo log)
        {
            if (logStack.Value == null)
            {
                logStack.Value = new Stack<LogInfo>();
            }
            log.TraceId = CurrentTrace;
            log.CurrentTrace = InTrace(log.ClassName + "." + log.MethodName);
            log.StartTime = DateTime.Now;
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
                    logTranceId.Value = Guid.NewGuid().ToString();
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

        public static string InTrace(string trace)
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

        public static string OutTrace()
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

    }
}
