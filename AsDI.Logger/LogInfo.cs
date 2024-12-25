using AsDI.Interceptor;

namespace AsDI.Log
{
    public class LogInfo
    {

        public LogInfo()
        {

        }

        public LogInfo(string message)
        {
            this.ServiceName = "";
            this.ClassName = "";
            this.MethodName = "";
            this.Message = message;
        }

        public LogInfo(string message, string className, string methodName)
        {
            this.ServiceName = className;
            this.ClassName = className;
            this.MethodName = methodName;
            this.Message = message;
        }

        public LogInfo(AspectEntity aspectEntity)
        {
            this.ServiceName = aspectEntity.Method?.TypeName;
            this.ClassName = aspectEntity.TargetAnalyzer.FinalTargetType()?.Name;
            this.MethodName = aspectEntity.Method?.Name;
            this.Parameters = aspectEntity.Method?.GetArgumentsValues();
            var log = aspectEntity.TargetAnalyzer?.FindAttribute<LogAttribute>();
            if (log != null)
            {
                this.Timeout = log.Timeout;
            }
        }

        /// <summary>
        /// 当前日志的请求号
        /// </summary>
        public string? TraceId { get; set; }

        /// <summary>
        /// 调用路径
        /// </summary>
        public string? CurrentTrace { get; set; }

        /// <summary>
        /// 服务名称（即接口名称）
        /// </summary>
        public string? ServiceName { get; set; }

        /// <summary>
        /// 类型名称（即实现类名称）
        /// </summary>
        public string? ClassName { get; set; }

        /// <summary>
        /// 方法名称
        /// </summary>
        public string? MethodName { get; set; }


        /// <summary>
        /// 执行的参数
        /// </summary>
        public object?[]? Parameters { get; set; }

        /// <summary>
        /// 执行结果（如果有）
        /// </summary>
        public object? Result { get; set; }

        /// <summary>
        /// 执行异常（如果有）
        /// </summary>
        public Exception? Exception { get; set; }

        /// <summary>
        /// 用户消息
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// 日志级别
        /// </summary>
        public LogLevel LogLevel { get; set; }

        /// <summary>
        /// 超时时间(默认最长5分钟)
        /// </summary>
        public int Timeout { get; set; } = 300000;


        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 用时（毫秒）
        /// </summary>
        public double? Duration { get; set; }

        /// <summary>
        /// 扩展信息
        /// </summary>
        public IDictionary<string, object>? ExtraInfo { get; set; }

    }
}
