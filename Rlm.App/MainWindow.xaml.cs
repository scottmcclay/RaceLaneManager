using MaterialDesignThemes.Wpf;
using Rlm.Core;
using System.ComponentModel;
using System.Windows;
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
        }

        private void CreateNewTournament_Click(object sender, RoutedEventArgs e)
        {
            TournamentManager.AddTournament("New Tournament", 4);
        }

        private void EditEvent_Click(object sender, RoutedEventArgs e)
        {
            TournamentViewModel vm = this.TournamentsListBox.SelectedItem as TournamentViewModel;

            if (vm != null)
            {
                EditTournamentViewModel editVm = new EditTournamentViewModel(vm.Tournament);
                var result = DialogHost.Show(editVm, "RootDialog");
            }
        }
    }
}
