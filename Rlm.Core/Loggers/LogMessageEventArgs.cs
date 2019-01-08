using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rlm.Core
{
    public delegate void LogMessageEventHandler(object sender, LogMessageEventArgs e);

    public class LogMessageEventArgs : EventArgs
    {
        public string Message { get; set; }

        public LogMessageEventArgs(string message)
        {
            this.Message = message;
        }
    }
}
