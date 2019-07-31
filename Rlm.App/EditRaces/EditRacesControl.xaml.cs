using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Rlm.App
{
    /// <summary>
    /// Interaction logic for EditRacesControl.xaml
    /// </summary>
    public partial class EditRacesControl : UserControl
    {
        public EditRacesControl()
        {
            InitializeComponent();
        }

        //private void BuildGrid(LineupControlViewModel vm)
        //{
        //    RacesByLaneGrid.Children.Clear();
        //    RacesByLaneGrid.ColumnDefinitions.Clear();
        //    RacesByLaneGrid.RowDefinitions.Clear();

        //    BuildRacesByLaneColumns(vm);
        //    BuildRacesByLaneRows(vm);
        //    PopulateRacesByLaneData(vm);
        //}

        //private void BuildRacesByLaneColumns(LineupControlViewModel vm)
        //{
        //    if (vm != null)
        //    {
        //        RacesByLaneGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

        //        for (int laneIndex = 0; laneIndex < vm.NumLanes; laneIndex++)
        //        {
        //            RacesByLaneGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        //        }

        //        RacesByLaneGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
        //    }
        //}

        //private void BuildRacesByLaneRows(LineupControlViewModel vm)
        //{
        //    if (vm != null)
        //    {
        //        RacesByLaneGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

        //        for (int raceIndex = 0; raceIndex < vm.NumRaces; raceIndex++)
        //        {
        //            RacesByLaneGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        //        }
        //    }
        //}

        //private void PopulateRacesByLaneData(LineupControlViewModel vm)
        //{
        //    int lastRow = RacesByLaneGrid.RowDefinitions.Count - 1;
        //    int lastColumn = RacesByLaneGrid.ColumnDefinitions.Count - 1;

        //    // Rows
        //    for (int row = 0; row < RacesByLaneGrid.RowDefinitions.Count; row++)
        //    {
        //        // Columns
        //        for (int column = 0; column < RacesByLaneGrid.ColumnDefinitions.Count; column++)
        //        {
        //            if ((row == 0) && (column != 0) && (column != lastColumn))
        //            {
        //                // header row
        //                ContentControl c = new ContentControl();
        //                c.Content = vm.LaneNames[column - 1];
        //                c.SetValue(Grid.RowProperty, row);
        //                c.SetValue(Grid.ColumnProperty, column);
        //                c.ContentTemplate = this.Resources["RowHeaderCell"] as DataTemplate;
        //                RacesByLaneGrid.Children.Add(c);
        //            }
        //            else if ((row != 0) && (column == 0))
        //            {
        //                // header column
        //                ContentControl c = new ContentControl();
        //                c.Content = vm.RaceNames[row - 1];
        //                c.SetValue(Grid.RowProperty, row);
        //                c.SetValue(Grid.ColumnProperty, column);
        //                c.ContentTemplate = this.Resources["ColumnHeaderCell"] as DataTemplate;
        //                RacesByLaneGrid.Children.Add(c);
        //            }
        //            else if ((row != 0) && (column != 0) && (column != lastColumn))
        //            {
        //                ContentControl c = new ContentControl();
        //                c.Content = vm.Races[row - 1].LaneAssignments[column - 1];
        //                c.SetValue(Grid.RowProperty, row);
        //                c.SetValue(Grid.ColumnProperty, column);
        //                c.ContentTemplate = this.Resources["CarCell"] as DataTemplate;
        //                RacesByLaneGrid.Children.Add(c);
        //            }
        //            else if ((column == lastColumn) && (row != 0))
        //            {
        //                ContentControl c = new ContentControl();
        //                c.Content = vm.Races[row - 1];
        //                c.SetValue(Grid.RowProperty, row);
        //                c.SetValue(Grid.ColumnProperty, column);
        //                c.ContentTemplate = this.Resources["ControlsCell"] as DataTemplate;
        //                RacesByLaneGrid.Children.Add(c);
        //            }
        //        }
        //    }
        //}

        private void SaveRaceButton_Click(object sender, RoutedEventArgs e)
        {
            EditRacesControlViewModel vm = (this.DataContext as EditRacesControlViewModel) ?? throw new Exception("DataContext is not set!");
            Button currentRaceButton = sender as Button;
            EditRaceViewModel race = currentRaceButton.DataContext as EditRaceViewModel;

            vm.SaveRace(race);
        }

        private void SetCurrentRaceButton_Click(object sender, RoutedEventArgs e)
        {
            EditRacesControlViewModel vm = (this.DataContext as EditRacesControlViewModel) ?? throw new Exception("DataContext is not set!");
            Button currentRaceButton = sender as Button;
            EditRaceViewModel race = currentRaceButton.DataContext as EditRaceViewModel;

            vm.SetCurrentRace(race);
        }

        private void StartRaceButton_Click(object sender, RoutedEventArgs e)
        {
            EditRacesControlViewModel vm = (this.DataContext as EditRacesControlViewModel) ?? throw new Exception("DataContext is not set!");
            Button currentRaceButton = sender as Button;
            EditRaceViewModel race = currentRaceButton.DataContext as EditRaceViewModel;

            vm.StartRace(race);
        }

        private void FinishRaceButton_Click(object sender, RoutedEventArgs e)
        {
            EditRacesControlViewModel vm = (this.DataContext as EditRacesControlViewModel) ?? throw new Exception("DataContext is not set!");
            Button currentRaceButton = sender as Button;
            EditRaceViewModel race = currentRaceButton.DataContext as EditRaceViewModel;

            vm.StopRace(race);
        }
    }
}
