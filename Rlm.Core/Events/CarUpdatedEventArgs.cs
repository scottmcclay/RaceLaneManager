using System;

namespace Rlm.Core
{
    public delegate void CarUpdatedEventHandler(int tournamentID, CarUpdatedEventArgs e);

    public class CarUpdatedEventArgs : EventArgs
    {
        public ICar Car { get; set; }
        public CarUpdatedEventArgs(ICar car)
        {
            this.Car = car;
        }
    }
}
