﻿using Rlm.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Rlm.App
{
    class StandingsControlViewModel
    {
        public int TournamentID { get; private set; }

        public ObservableCollection<StandingViewModel> Standings { get; private set; }

        public StandingsControlViewModel(int tournamentID)
        {
            this.TournamentID = tournamentID;

            TournamentManager.StandingsUpdated += TournamentManager_StandingsUpdated;

            this.Standings = new ObservableCollection<StandingViewModel>();
            PopulateStandings();
        }

        private void PopulateStandings()
        {
            foreach (IStanding standing in TournamentManager.GetStandings(this.TournamentID))
            {
                Standings.Add(new StandingViewModel(standing));
            }
        }

        private void TournamentManager_StandingsUpdated(int tournamentID, StandingsUpdatedEventArgs e)
        {
            if (!Application.Current.Dispatcher.CheckAccess())
            {
                Application.Current.Dispatcher.Invoke(() => this.TournamentManager_StandingsUpdated(tournamentID, e));
            }
            else
            {
                if (tournamentID != this.TournamentID) return;

                this.Standings.Clear();

                foreach (IStanding standing in e.Standings)
                {
                    this.Standings.Add(new StandingViewModel(standing));
                }
            }
        }
    }
}
