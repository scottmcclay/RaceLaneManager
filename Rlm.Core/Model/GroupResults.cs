using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rlm.Core
{
    public class GroupResults
    {
        public string GroupName { get; set; }
        public List<Standing> Standings { get; set; }

        public GroupResults()
        {
            this.Standings = new List<Standing>();
        }
    }
}
