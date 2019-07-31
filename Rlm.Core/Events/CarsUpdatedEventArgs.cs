using System;
using System.Collections.Generic;

namespace Rlm.Core
{
    public delegate void CarsUpdatedEventHandler(int tournamentID, CarsUpdatedEventArgs e);

    public class CarsUpdatedEventArgs : EventArgs
    {
        public IEnumerable<ICar> Cars { get; set; }
        public CarsUpdatedEventArgs(IEnumerable<ICar> cars)
        {
            this.Cars = cars;
        }
    }
}
