using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsDI.Logger
{
    public class SurroundLog : IDisposable
    {
        public SurroundLog(string message)
        {
            var logInfo = new LogInfo(message);
            Logger.LogStart(logInfo);
        }

        public void Dispose()
        {
            Logger.LogEnd("");
            GC.SuppressFinalize(this);
        }
    }
}
