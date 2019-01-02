using Rlm.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rlm.App
{
    class CarViewModel
    {
        private ICar _car;

        public int Number => 100;
        public string Name => _car.Name;
        public string Owner => _car.Owner;
        public string Den => _car.Den;

        public CarViewModel(ICar car)
        {
            _car = car;
        }
    }
}
