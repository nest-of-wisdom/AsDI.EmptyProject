﻿using AsDI.Attributes;
using AsDI.Logger;
using System.Text.RegularExpressions;

namespace AsDI.EmptyProject.Api
{
    [Service]
    public class LogWriter : ILogWriter
    {
        public void Begin(LogInfo logInfo)
        {
            Console.WriteLine("开始执行：" + logInfo.CurrentTrace + "[用时{0}]", logInfo.Duration);
        }
        public void End(LogInfo logInfo)
        {
            Console.WriteLine("完成执行：" + logInfo.CurrentTrace + "[用时{0}]", logInfo.Duration);
        }
    }
}