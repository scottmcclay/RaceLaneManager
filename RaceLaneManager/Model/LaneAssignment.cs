using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RaceLaneManager;

namespace RaceLaneManager.Model
{
    public interface ILaneAssignment
    {
        int Lane { get; }
        ICar Car { get; }
        long ElapsedTime { get; }
        int Position { get; }
        int Points { get; }
    }

    public class LaneAssignment : ILaneAssignment
    {
        public int Lane { get; set; }
        [JsonConverter(typeof(ConcreteConverter<Car>))]
        public ICar Car { get; set; }
        public long ElapsedTime { get; set; }
        public int Position { get; set; }
        public int Points { get; set; }

        public LaneAssignment()
        {

        }
    }
}
