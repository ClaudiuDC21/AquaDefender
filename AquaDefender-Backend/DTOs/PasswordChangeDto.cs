using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AquaDefender_Backend.DTOs
{
    public class PasswordChangeDto
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}