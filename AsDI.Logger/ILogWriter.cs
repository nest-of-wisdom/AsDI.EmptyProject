using AsDI.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsDI.Log
{
    [NoProxy]
    public interface ILogWriter
    {
        public void Begin(LogInfo logInfo);

        public void End(LogInfo logInfo);

    }
}
