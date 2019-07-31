using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for LineupControl.xaml
    /// </summary>
    public partial class LineupControl : UserControl
    {
        public LineupControl()
        {
            InitializeComponent();
        }

        private void DataContext_Changed(object sender, DependencyPropertyChangedEventArgs e)
        {
            LineupControlViewModel vm = e.OldValue as LineupControlViewModel;
            if (vm != null)
            {
                vm.DataChanged -= LineupControlViewModel_DataChanged;
            }

            vm = e.NewValue as LineupControlViewModel;
            if (vm != null)
            {
                vm.DataChanged += LineupControlViewModel_DataChanged;
            }

            BuildGrid(vm);
        }

        private void LineupControlViewModel_DataChanged(object sender, EventArgs e)
        {
            BuildGrid(sender as LineupControlViewModel);
        }

        private void BuildGrid(LineupControlViewModel vm)
        {
            RacesByLaneGrid.Children.Clear();
            RacesByLaneGrid.ColumnDefinitions.Clear();
            RacesByLaneGrid.RowDefinitions.Clear();

            BuildRacesByLaneColumns(vm);
            BuildRacesByLaneRows(vm);
            PopulateRacesByLaneData(vm);

            RacesByCarGrid.Children.Clear();
            RacesByCarGrid.ColumnDefinitions.Clear();
            RacesByCarGrid.RowDefinitions.Clear();

            BuildRacesByCarColumns(vm);
            BuildRacesByCarRows(vm);
            PopulateRacesByCarData(vm);
        }

        private void BuildRacesByLaneColumns(LineupControlViewModel vm)
        {
            if (vm != null)
            {
                RacesByLaneGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

                for (int laneIndex = 0; laneIndex < vm.NumLanes; laneIndex++)
                {
                    RacesByLaneGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                }
            }
        }

        private void BuildRacesByLaneRows(LineupControlViewModel vm)
        {
            if (vm != null)
            {
                RacesByLaneGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                for (int raceIndex = 0; raceIndex < vm.NumRaces; raceIndex++)
                {
                    RacesByLaneGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                }
            }
        }

        private void PopulateRacesByLaneData(LineupControlViewModel vm)
        {
            // Rows
            for (int row = 0; row < RacesByLaneGrid.RowDefinitions.Count; row++)
            {
                // Columns
                for (int column = 0; column < RacesByLaneGrid.ColumnDefinitions.Count; column++)
                {
                    if ((row == 0) && (column != 0))
                    {
                        // header row
                        ContentControl c = new ContentControl();
                        c.Content = vm.LaneNames[column - 1];
                        c.SetValue(Grid.RowProperty, row);
                        c.SetValue(Grid.ColumnProperty, column);
                        c.ContentTemplate = this.Resources["RowHeaderCell"] as DataTemplate;
                        RacesByLaneGrid.Children.Add(c);
                    }
                    else if ((row != 0) && (column == 0))
                    {
                        // header column
                        ContentControl c = new ContentControl();
                        c.Content = vm.RaceNames[row - 1];
                        c.SetValue(Grid.RowProperty, row);
                        c.SetValue(Grid.ColumnProperty, column);
                        c.ContentTemplate = this.Resources["ColumnHeaderCell"] as DataTemplate;
                        RacesByLaneGrid.Children.Add(c);
                    }
                    else if ((row != 0) && (column != 0))
                    {
                        ContentControl c = new ContentControl();
                        c.Content = vm.Races[row - 1].LaneAssignments[column - 1];
                        c.SetValue(Grid.RowProperty, row);
                        c.SetValue(Grid.ColumnProperty, column);
                        c.ContentTemplate = this.Resources["CarCell"] as DataTemplate;
                        RacesByLaneGrid.Children.Add(c);
                    }
                }
            }
        }

        private void BuildRacesByCarColumns(LineupControlViewModel vm)
        {
            if (vm != null)
            {
                RacesByCarGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

                for (int laneIndex = 0; laneIndex < vm.NumLanes; laneIndex++)
                {
                    RacesByCarGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                }
            }
        }

        private void BuildRacesByCarRows(LineupControlViewModel vm)
        {
            if (vm != null)
            {
                RacesByCarGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                for (int carIndex = 0; carIndex < vm.NumCars; carIndex++)
                {
                    RacesByCarGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                }
            }
        }

        private void PopulateRacesByCarData(LineupControlViewModel vm)
        {
            // Rows
            for (int row = 0; row < RacesByCarGrid.RowDefinitions.Count; row++)
            {
                // Columns
                for (int column = 0; column < RacesByCarGrid.ColumnDefinitions.Count; column++)
                {
                    if ((row == 0) && (column != 0))
                    {
                        // header row
                        if (vm.LaneNames.Length > (column - 1))
                        {
                            ContentControl c = new ContentControl();
                            c.Content = vm.LaneNames[column - 1];
                            c.SetValue(Grid.RowProperty, row);
                            c.SetValue(Grid.ColumnProperty, column);
                            c.ContentTemplate = this.Resources["RowHeaderCell"] as DataTemplate;
                            RacesByCarGrid.Children.Add(c);
                        }
                    }
                    else if ((row != 0) && (column == 0))
                    {
                        // header column
                        if (vm.CarNames.Length > (row - 1))
                        {
                            ContentControl c = new ContentControl();
                            c.Content = vm.CarNames[row - 1];
                            c.SetValue(Grid.RowProperty, row);
                            c.SetValue(Grid.ColumnProperty, column);
                            //c.SetValue(Grid.ColumnSpanProperty, RacesByCarGrid.ColumnDefinitions.Count);
                            c.ContentTemplate = this.Resources["ColumnHeaderCell"] as DataTemplate;
                            RacesByCarGrid.Children.Add(c);
                        }
                    }
                    else if ((row != 0) && (column != 0))
                    {
                        if ((vm.Cars.Count > (row - 1)) && (vm.Cars[row - 1].Races.Length > column - 1))
                        {
                            ContentControl c = new ContentControl();
                            c.Content = vm.Cars[row - 1].Races[column - 1];
                            c.SetValue(Grid.RowProperty, row);
                            c.SetValue(Grid.ColumnProperty, column);
                            c.ContentTemplate = this.Resources["RaceLaneCell"] as DataTemplate;
                            RacesByCarGrid.Children.Add(c);
                        }
                    }
                }
            }
        }

        private void GenerateRacesButton_Click(object sender, RoutedEventArgs e)
        {
            LineupControlViewModel vm = this.DataContext as LineupControlViewModel;
            vm.GenerateRaces();
        }
    }
}
