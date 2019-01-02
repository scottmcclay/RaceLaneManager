using MaterialDesignThemes.Wpf;
using Rlm.Core;
using System.ComponentModel;
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
        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = new MainWindowViewModel();
        }

        private void UIElement_OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MenuToggleButton.IsChecked = false;
            var selectedTournament = this.TournamentsListBox.SelectedItem;
            if (selectedTournament != null)
            {

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
            TournamentViewModel vm = this.TournamentsListBox.SelectedItem as TournamentViewModel;
        }
    }
}
