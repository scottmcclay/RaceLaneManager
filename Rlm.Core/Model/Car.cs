using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Rlm.Core
{
    public interface ICar
    {
        int ID { get; }
        string Name { get; }
        string Owner { get; }
        string Den { get; }
    }

    public class Car : ICar
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Owner { get; set; }
        public string Den { get; set; }

        public static Car From(ICar car)
        {
            return new Car()
            {
                ID = car.ID,
                Name = car.Name,
                Owner = car.Owner,
                Den = car.Den
            };
        }

        public void CopyFrom(ICar car)
        {
            this.ID = car.ID;
            this.Name = car.Name;
            this.Owner = car.Owner;
            this.Den = car.Den;
        }
    }
}
