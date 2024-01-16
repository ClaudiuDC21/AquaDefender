using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AquaDefender_Backend.Domain
{
    public class AppUser
    {   
        [Key]
        public int IdUser { get; set; }

        public string UserName { get; set; }
    }
}