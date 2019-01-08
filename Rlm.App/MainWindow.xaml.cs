using MaterialDesignThemes.Wpf;
using Rlm.Core;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Rlm.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ConductRacesWindow _conductRacesWindow;

        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = new MainWindowViewModel();
        }

        private void UIElement_OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MenuToggleButton.IsChecked = false;

            TournamentViewModel selectedTournament = this.TournamentsListBox.SelectedItem as TournamentViewModel;
            EditRacesControlViewModel vm = _conductRacesWindow?.DataContext as EditRacesControlViewModel;
            vm?.Dispose();

            if (_conductRacesWindow != null)
            {
                vm = null;

                if (selectedTournament != null)
                {
                    vm = new EditRacesControlViewModel(selectedTournament.TournamentID);
                }

                _conductRacesWindow.DataContext = vm;
            }
        }

        private void ViewsListBox_TargetUpdated(object sender, DataTransferEventArgs e)
        {
            ListBox lb = sender as ListBox;
            if ((lb != null) && (lb.HasItems))
            {
                lb.SelectedIndex = 0;
            }
        }

        private void CreateNewTournament_Click(object sender, RoutedEventArgs e)
        {
            TournamentManager.AddTournament("New Tournament", 4);
        }

        private void EditEvent_Click(object sender, RoutedEventArgs e)
        {
            TournamentViewModel vm = this.TournamentsListBox.SelectedItem as TournamentViewModel;
            vm?.EditTournament();
        }

        private void ConductRaces_Click(object sender, RoutedEventArgs e)
        {
            if (_conductRacesWindow != null)
            {
                if (_conductRacesWindow.IsLoaded)
                {
                    _conductRacesWindow.Close();
                }

                _conductRacesWindow = null;
            }

            TournamentViewModel vm = this.TournamentsListBox.SelectedItem as TournamentViewModel;

            _conductRacesWindow = new ConductRacesWindow
            {
                Owner = this,
                DataContext = new EditRacesControlViewModel(vm.TournamentID)
            };

            _conductRacesWindow.Closed += ConductRacesWindows_Closed;

            _conductRacesWindow.Show();
        }

        private void ConductRacesWindows_Closed(object sender, EventArgs e)
        {
            if (_conductRacesWindow != null)
            {
                EditRacesControlViewModel vm = _conductRacesWindow.DataContext as EditRacesControlViewModel;
                vm?.Dispose();

                _conductRacesWindow.Closed -= ConductRacesWindows_Closed;
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            RaceMonitor.Stop();
        }
    }
}
