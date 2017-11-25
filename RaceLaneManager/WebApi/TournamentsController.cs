using Newtonsoft.Json.Linq;
using RaceLaneManager.Model;
using RaceLaneManager.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace RaceLaneManager.WebApi
{
    public class TournamentsController : ApiController
    {
        public IEnumerable<Tournament> GetAllTournaments()
        {
            return RepositoryManager.GetDefaultRepository().GetAllTournaments();
        }

        public HttpResponseMessage PostTournament([FromBody] dynamic json)
        {
            //int lanes = json["lanes"].Value<int>();
            int lanes = json.lanes;
            string name = json.name;

            Tournament tournament = new Tournament(name, lanes);

            tournament = RepositoryManager.GetDefaultRepository().AddTournament(tournament);
            HttpResponseMessage response = Request.CreateResponse<Tournament>(HttpStatusCode.Created, tournament);
            string uri = Url.Link("RaceLaneManagerTournamentApi", new { id = tournament.ID });
            response.Headers.Location = new Uri(uri);

            return response;
        }

        public Tournament GetTournament(int tournamentId)
        {
            return RepositoryManager.GetDefaultRepository().GetTournament(tournamentId);
        }

        public void PutTournament(int tournamentId, Tournament tournament)
        {
        }

        public void DeleteTournament(int tournamentId)
        {

        }
    }
}
