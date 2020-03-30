using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using src.Models;

namespace src.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<src.Models.Organization> Organization { get; set; }

        public DbSet<src.Models.Product> Product { get; set; }

        public DbSet<src.Models.Customer> Customer { get; set; }

        public DbSet<src.Models.Contact> Contact { get; set; }

        public DbSet<src.Models.SupportAgent> SupportAgent { get; set; }

        public DbSet<src.Models.SupportEngineer> SupportEngineer { get; set; }

        public DbSet<src.Models.Ticket> Ticket { get; set; }

        public DbSet<src.Models.Ticketing> Ticketing { get; set; }

        public DbSet<src.Models.PriceCommodity> PriceCommodity { get; set; }

        public DbSet<src.Models.UserRole> UserRole { get; set; }

        public DbSet<src.Models.Roles> Role { get; set; }

        public DbSet<src.Models.Module> Modules { get; set; }

        public DbSet<src.Models.StallLease> StallLease { get; set; }

        public DbSet<src.Models.TradersTruck> TradersTruck { get; set; }

        public DbSet<src.Models.FarmersTruck> FarmersTruck { get; set; }

        public DbSet<src.Models.ShortTrip> ShortTrip { get; set; }

        public DbSet<src.Models.GatePass> GatePass { get; set; }

        public DbSet<src.Models.PayParking> PayParking { get; set; }

        public DbSet<src.Models.Repair> Repair { get; set; }

        public DbSet<src.Models.InterTrading> InterTrading { get; set; }

        public DbSet<src.Models.CarrotFacility> CarrotFacility { get; set; }

        public DbSet<src.Models.Buyers> AccreditedBuyers { get; set; }

        public DbSet<src.Models.IndividualFarmers> AccreditedIndividualFarmers { get; set; }

        public DbSet<src.Models.InterTraders> AccreditedInterTraders { get; set; }

        public DbSet<src.Models.MarketFacilitators> AccreditedMarketFacilitators { get; set; }

        public DbSet<src.Models.PackersAndPorters> AccreditedPackersAndPorters { get; set; }

        public DbSet<src.Models.SecurityInspectionReport> SecurityInspectionReport { get; set; }

        public DbSet<src.Models.ApplicationUser> ApplicationUser { get; set; }
    }
}
