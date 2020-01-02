using System;

namespace ChaoticRotation.Core
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
