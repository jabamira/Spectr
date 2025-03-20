using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Spectr.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Administrator> Administrators { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<AreaCoordinates> AreaCoordinates { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<ProfileCoordinates> ProfileCoordinates { get; set; }
        public DbSet<Operator> Operators { get; set; }
        public DbSet<GammaSpectrometer> GammaSpectrometers { get; set; }
        public DbSet<Picket> Pickets { get; set; }
        public DbSet<Analyst> Analysts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer("Server=your_server;Database=your_db;Trusted_Connection=True;");
    }
}
