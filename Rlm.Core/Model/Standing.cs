using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rlm.Core
{
    public interface IStanding
    {
        ICar Car { get; }
        int Points { get; }
        string Position { get; }
        long AverageTime { get; }
    }

    public class Standing : IStanding
    {
        public ICar Car { get; set; }
        public int Points { get; set; }
        public string Position { get; set; }
        public long AverageTime { get; set; }
    }
}
