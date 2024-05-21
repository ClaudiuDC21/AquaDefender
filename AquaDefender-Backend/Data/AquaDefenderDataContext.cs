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
        public DbSet<Report> Reports { get; set; }
        public DbSet<ReportImage> ReportImage { get; set; }
        public DbSet<WaterInfo> WaterInfos { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<County> County { get; set; }
        public DbSet<UserRole> UserRole { get; set; }
        public DbSet<WaterValues> WaterValues { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurarea relației dintre AppUser și UserRole
            modelBuilder.Entity<AppUser>()
                .HasOne(au => au.Role)
                .WithMany()
                .HasForeignKey(au => au.RoleId)
                .OnDelete(DeleteBehavior.NoAction);

            // Configurarea relației dintre AppUser și County
            modelBuilder.Entity<AppUser>()
                .HasOne(au => au.County)
                .WithMany()
                .HasForeignKey(au => au.CountyId)
                .OnDelete(DeleteBehavior.NoAction);

            // Configurarea relației dintre AppUser și City
            modelBuilder.Entity<AppUser>()
                .HasOne(au => au.City)
                .WithMany()
                .HasForeignKey(au => au.CityId)
                .OnDelete(DeleteBehavior.NoAction);

            // Configurarea relației dintre Report și AppUser
            modelBuilder.Entity<Report>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reports)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configurarea relației dintre Report și County
            modelBuilder.Entity<Report>()
                .HasOne(r => r.County)
                .WithMany()
                .HasForeignKey(r => r.CountyId)
                .OnDelete(DeleteBehavior.NoAction);

            // Configurarea relației dintre Report și City
            modelBuilder.Entity<Report>()
                .HasOne(r => r.City)
                .WithMany()
                .HasForeignKey(r => r.CityId)
                .OnDelete(DeleteBehavior.NoAction);

            // Configurarea relației dintre ReportImage și Report
            modelBuilder.Entity<ReportImage>()
                .HasOne(ri => ri.Report)
                .WithMany(r => r.Images)
                .HasForeignKey(ri => ri.ReportId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configurarea relației dintre City și County
            modelBuilder.Entity<City>()
                .HasOne(c => c.County)
                .WithMany(county => county.Cities)
                .HasForeignKey(c => c.CountyId)
                .OnDelete(DeleteBehavior.NoAction);

            // Configurarea tabelului WaterInfo
            modelBuilder.Entity<WaterInfo>()
                .HasOne(wi => wi.County)
                .WithMany()
                .HasForeignKey(wi => wi.CountyId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<WaterInfo>()
                .HasOne(wi => wi.City)
                .WithMany()
                .HasForeignKey(wi => wi.CityId)
                .OnDelete(DeleteBehavior.NoAction);

            // Configurarea relației dintre WaterValues și WaterInfo
            modelBuilder.Entity<WaterValues>()
                .HasOne(wv => wv.WaterInfo)
                .WithMany(wi => wi.WaterValues)
                .HasForeignKey(wv => wv.IdWaterInfo)
                .OnDelete(DeleteBehavior.Cascade); // Modificat de la NoAction la Cascade

        }
    }
}
