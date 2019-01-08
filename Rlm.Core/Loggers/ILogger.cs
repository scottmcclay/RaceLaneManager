using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rlm.Core
{
    public interface ILogger
    {
        void LogMessage(string message);
        void LogMessage(object sender, string message);
    }
}
