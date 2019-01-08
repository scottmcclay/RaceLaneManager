using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for CarsControl.xaml
    /// </summary>
    public partial class CarsControl : UserControl
    {
        public CarsControl()
        {
            InitializeComponent();
        }

        private void EditCar_Click(object sender, RoutedEventArgs e)
        {
            Button editButton = sender as Button;
            CarViewModel carVM = editButton.DataContext as CarViewModel;
            carVM.Edit = true;
            DialogHost.Show(carVM, "CarsDialogHost", EditCarDialogClosingEventHandler);
        }

        private void DeleteCar_Click(object sender, RoutedEventArgs e)
        {
            Button editButton = sender as Button;
            CarViewModel carVM = editButton.DataContext as CarViewModel;
            CarsControlViewModel vm = this.DataContext as CarsControlViewModel;
            vm.DeleteCar(carVM);
        }

        private void AddCar_Click(object sender, RoutedEventArgs e)
        {
            CarViewModel vm = new CarViewModel();
            DialogHost.Show(vm, "CarsDialogHost", EditCarDialogClosingEventHandler);
        }

        private void EditCarDialogClosingEventHandler(object sender, DialogClosingEventArgs e)
        {
            if ((e.Parameter as bool?) == true)
            {
                CarsControlViewModel vm = this.DataContext as CarsControlViewModel;
                CarViewModel carVM = e.Content as CarViewModel;
                if (carVM.Edit)
                {
                    vm.UpdateCar(carVM);
                }
                else
                {
                    vm.AddCar(carVM);
                }
            }
        }
    }
}
