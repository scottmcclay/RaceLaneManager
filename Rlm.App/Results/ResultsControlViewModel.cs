using Rlm.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rlm.App
{
    class ResultsControlViewModel
    {
        public int TournamentID { get; private set; }

        private List<StandingViewModel> _overallResults = new List<StandingViewModel>();
        public IEnumerable<StandingViewModel> OverallResults => _overallResults;

        private List<StandingViewModel> _lionResults = new List<StandingViewModel>();
        public IEnumerable<StandingViewModel> LionResults => _lionResults;

        private List<StandingViewModel> _tigerResults = new List<StandingViewModel>();
        public IEnumerable<StandingViewModel> TigerResults => _tigerResults;

        private List<StandingViewModel> _wolfResults = new List<StandingViewModel>();
        public IEnumerable<StandingViewModel> WolfResults => _wolfResults;

        private List<StandingViewModel> _bearResults = new List<StandingViewModel>();
        public IEnumerable<StandingViewModel> BearResults => _bearResults;

        private List<StandingViewModel> _webelosIResults = new List<StandingViewModel>();
        public IEnumerable<StandingViewModel> WebelosIResults => _webelosIResults;

        private List<StandingViewModel> _webelosIIResults = new List<StandingViewModel>();
        public IEnumerable<StandingViewModel> WebelosIIResults => _webelosIIResults;

        public ResultsControlViewModel(int tournamentID)
        {
            this.TournamentID = tournamentID;

            DisplayResults();
        }

        public void DisplayResults()
        {
            List<GroupResults> results = TournamentManager.GetTournamentResults(this.TournamentID);

            foreach (GroupResults groupResults in results)
            {
                switch (groupResults.GroupName)
                {
                    case "Overall":
                        PopulateResults(groupResults.Standings, ref _overallResults);
                        break;

                    case "Lion":
                        PopulateResults(groupResults.Standings, ref _lionResults);
                        break;

                    case "Tiger":
                        PopulateResults(groupResults.Standings, ref _tigerResults);
                        break;

                    case "Wolf":
                        PopulateResults(groupResults.Standings, ref _wolfResults);
                        break;

                    case "Bear":
                        PopulateResults(groupResults.Standings, ref _bearResults);
                        break;

                    case "Webelos I":
                        PopulateResults(groupResults.Standings, ref _webelosIResults);
                        break;

                    case "Webelos II":
                        PopulateResults(groupResults.Standings, ref _webelosIIResults);
                        break;
                }
            }
        }

        private void PopulateResults(IEnumerable<IStanding> standings, ref List<StandingViewModel> results)
        {
            results.Clear();
            foreach (IStanding standing in standings)
            {
                results.Add(new StandingViewModel(standing));
            }
        }
    }
}
