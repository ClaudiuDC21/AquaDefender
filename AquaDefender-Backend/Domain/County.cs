using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AquaDefender_Backend.Domain
{
    public class County
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public List<City> Cities { get; set; }
        public string WaterDeptEmail { get; set; }
    }
}