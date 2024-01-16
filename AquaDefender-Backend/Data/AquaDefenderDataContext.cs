using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AquaDefender_Backend.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace AquaDefender_Backend.Data
{
    public class AquaDefenderDataContext : DbContext
    {
        public AquaDefenderDataContext(DbContextOptions options) : base(options)
        {
        }

        protected AquaDefenderDataContext()
        {
        }

        public DbSet<AppUser> Users { get; set; }
    }
}