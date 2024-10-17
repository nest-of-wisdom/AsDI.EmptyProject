using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsDI.Logger
{
    public enum LogLevel
    {
        /// <summary>
        /// 正常信息
        /// </summary>
        Normal = 1,

        /// <summary>
        /// 异常信息
        /// </summary>
        Error = 2,

        /// <summary>
        /// 超时信息
        /// </summary>
        Timeout = 4

    }
}
