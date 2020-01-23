using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Rlm.Core;

namespace Rlm.Web.WebApi
{
    public class LineupController : ApiController
    {
        public IEnumerable<string> GetAllLineup()
        {
            List<string> result = new List<string>();

            result.Add("Hello world!");

            return result;
        }

        public RlmGetRacesResponse GetLineup(int id)
        {
            return TournamentManager.GetRaces(id);
        }
    }
}
