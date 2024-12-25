using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsDI.Log
{
    [System.AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    sealed class LogAttribute : Attribute
    {
        public LogAttribute()
        {
            this.Timeout = 300000;
        }

        /// <summary>
        /// 执行的超时时间
        /// </summary>
        public int Timeout
        {
            get; set;
        }

    }
}
