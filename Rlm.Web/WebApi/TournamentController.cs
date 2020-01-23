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
    public class TournamentController : ApiController
    {
        public IEnumerable<TournamentDto> GetAllTournaments()
        {
            List<TournamentDto> result = new List<TournamentDto>();

            foreach (ITournament tournament in TournamentManager.GetTournaments())
            {
                result.Add(TournamentDto.FromTournament(tournament));
            }

            return result;
        }

        public TournamentDto GetTournament(int id)
        {
            ITournament tournament = TournamentManager.GetTournament(id);
            return TournamentDto.FromTournament(tournament);
        }
    }
}
