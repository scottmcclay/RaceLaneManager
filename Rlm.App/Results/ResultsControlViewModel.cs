using Rlm.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rlm.App
{
    class ResultsControlViewModel
    {
        public int TournamentID { get; private set; }

        public ObservableCollection<StandingViewModel> OverallResults { get; } = new ObservableCollection<StandingViewModel>();
        public ObservableCollection<StandingViewModel> LionResults { get; } = new ObservableCollection<StandingViewModel>();
        public ObservableCollection<StandingViewModel> TigerResults { get; } = new ObservableCollection<StandingViewModel>();
        public ObservableCollection<StandingViewModel> WolfResults { get; } = new ObservableCollection<StandingViewModel>();
        public ObservableCollection<StandingViewModel> BearResults { get; } = new ObservableCollection<StandingViewModel>();
        public ObservableCollection<StandingViewModel> WebelosIResults { get; } = new ObservableCollection<StandingViewModel>();
        public ObservableCollection<StandingViewModel> WebelosIIResults { get; } = new ObservableCollection<StandingViewModel>();

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
                        this.OverallResults.Clear();
                        foreach (IStanding standing in groupResults.Standings)
                        {
                            this.OverallResults.Add(new StandingViewModel(standing));
                        }
                        break;

                    case "Lion":
                        this.LionResults.Clear();
                        foreach (IStanding standing in groupResults.Standings)
                        {
                            this.LionResults.Add(new StandingViewModel(standing));
                        }
                        break;

                    case "Tiger":
                        this.TigerResults.Clear();
                        foreach (IStanding standing in groupResults.Standings)
                        {
                            this.TigerResults.Add(new StandingViewModel(standing));
                        }
                        break;

                    case "Wolf":
                        this.WolfResults.Clear();
                        foreach (IStanding standing in groupResults.Standings)
                        {
                            this.WolfResults.Add(new StandingViewModel(standing));
                        }
                        break;

                    case "Bear":
                        this.BearResults.Clear();
                        foreach (IStanding standing in groupResults.Standings)
                        {
                            this.BearResults.Add(new StandingViewModel(standing));
                        }
                        break;

                    case "Webelos I":
                        this.WebelosIResults.Clear();
                        foreach (IStanding standing in groupResults.Standings)
                        {
                            this.WebelosIResults.Add(new StandingViewModel(standing));
                        }
                        break;

                    case "Webelos II":
                        this.WebelosIIResults.Clear();
                        foreach (IStanding standing in groupResults.Standings)
                        {
                            this.WebelosIIResults.Add(new StandingViewModel(standing));
                        }
                        break;
                }
            }
        }
    }
}
