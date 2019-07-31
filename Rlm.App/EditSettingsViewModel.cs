using Rlm.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rlm.App
{
    class EditSettingsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool _simulate = false;
        public bool Simulate
        {
            get => _simulate;
            set
            {
                if (value != _simulate)
                {
                    _simulate = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Simulate)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.EnableHWOptions)));
                }
            }
        }

        public bool EnableHWOptions => !this.Simulate;
        public ObservableCollection<string> ComPorts { get; private set; } = new ObservableCollection<string>();
        public string ComPort { get; set; }
        public string BaudRate { get; private set; }

        public EditSettingsViewModel()
        {
            this.Simulate = TournamentManager.Simulate;
            RefreshComPorts();
        }

        public void RefreshComPorts()
        {
            this.ComPorts.Clear();

            foreach (string port in SerialPort.GetPortNames())
            {
                this.ComPorts.Add(port);
            }
        }
    }
}
