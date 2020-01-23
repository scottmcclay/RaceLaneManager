using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using Rlm.Core;
using Rlm.Web.DTOs;

namespace Rlm.Web.WebApi
{
    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    public class RaceController : ApiController
    {
        public IEnumerable<RaceDto> GetAllRaces(int tournamentId)
        {
            ITournament tournament = TournamentManager.GetTournament(tournamentId);

            List<RaceDto> result = new List<RaceDto>();
            foreach (IRace race in tournament.Races)
            {
                result.Add(RaceDto.FromRace(race));
            }

            return result;
        }
    }
}
