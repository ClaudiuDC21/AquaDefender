using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AquaDefender_Backend.Domain
{
    public class WaterValues
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string MaximumAllowedValue { get; set; }
        public string UserProvidedValue { get; set; }
        public string MeasurementUnit { get; set; }
        public int IdWaterInfo { get; set; }
        public WaterInfo WaterInfo { get; set; }

    }
}