using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rlm.Core
{
    public class EventLogger : ILogger
    {
        public event LogMessageEventHandler LogMessageGenerated;

        public void LogMessage(string message)
        {
            LogMessage(null, message);
        }

        public void LogMessage(object sender, string message)
        {
            this.LogMessageGenerated?.Invoke(sender, new LogMessageEventArgs(message));
        }
    }
}
