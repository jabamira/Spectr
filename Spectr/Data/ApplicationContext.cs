using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Spectr.Data
{
    public class ApplicationContext : DbContext
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
        public DbSet<ProfileOperator> ProfileOperator { get; set; }
        public DbSet<Picket> Pickets { get; set; }
        public DbSet<Analyst> Analysts { get; set; }

        static string servername_ = "DBSRV\\ag2024";
        static string dbName = "LesnikovAA_2207g2_spectr4";
        static string servername = "ZALMAN\\MSSQLSERVER01";
        public string connectionString = $"Server={servername};Database={dbName};Integrated Security=True;TrustServerCertificate=True;";


        static bool UseSqlLite = false;
        private const string DatabasePath = "C:\\Users\\artem\\Desktop\\Spectr.db";

        public ApplicationContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (UseSqlLite)
            {
                
                optionsBuilder.UseSqlite($"Data Source={DatabasePath}");
            }
            else
            {
                optionsBuilder.UseSqlServer($"Server={servername};Database={dbName};Trusted_Connection=True;TrustServerCertificate=True;");

            }

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ContractAnalyst>()
         .HasKey(ca => new { ca.ContractID, ca.AnalystID });
            modelBuilder.Entity<ProfileOperator>()
        .HasKey(po => new { po.ProfileID, po.OperatorID });



            base.OnModelCreating(modelBuilder);
        }
    }
}
