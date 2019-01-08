using Rlm.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rlm.App
{
    class CarViewModel : ICar
    {
        public int ID { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
        public string Owner { get; set; }
        public string Den { get; set; }
        public bool Edit { get; set; }

        public string[] Dens => new string[] { "Lion", "Tiger", "Wolf", "Bear", "Webelos I", "Webelos II" };

        public CarViewModel(ICar car = null)
        {
            if (car != null)
            {
                this.ID = car.ID;
                this.Number = car.Number;
                this.Name = String.IsNullOrEmpty(car.Name) ? car.Owner : car.Name;
                this.Owner = car.Owner;
                this.Den = car.Den;
            }
        }
    }
}
