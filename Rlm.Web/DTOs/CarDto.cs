using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rlm.Core;

namespace Rlm.Web.DTOs
{
    public class CarDto
    {
        public int ID { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
        public string Owner { get; set; }
        public string Den { get; set; }

        public static CarDto FromCar(ICar car)
        {
            if (car == null) return null;

            CarDto result = new CarDto()
            {
                ID = car.ID,
                Number = car.Number,
                Name = car.Name,
                Owner = car.Owner,
                Den = car.Den
            };

            return result;
        }
    }
}
