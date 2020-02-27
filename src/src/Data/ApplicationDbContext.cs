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

        public DbSet<src.Models.Repair> Repair { get; set; }

        public DbSet<src.Models.Watchmen> Watchmen { get; set; }

        public DbSet<src.Models.Inspector> Inspector { get; set; }

        public DbSet<src.Models.Ticketing> Ticketing { get; set; }

        public DbSet<src.Models.PriceCommodity> PriceCommodity { get; set; }

        public DbSet<src.Models.Employee> Employee { get; set; }

        public DbSet<src.Models.Accreditation> Accreditation { get; set; }

        public DbSet<src.Models.HrForm> HrForm { get; set; }

        public DbSet<src.Models.Hr> Hr { get; set; }

        public DbSet<src.Models.Compensatory> Compensatory { get; set; }

        public DbSet<src.Models.AbsenceRequest> AbsenceRequest { get; set; }

        public DbSet<src.Models.DayOffAuthorization> DayOffAuthorization { get; set; }

        public DbSet<src.Models.Finance> Finance { get; set; }

        public DbSet<src.Models.Clerk> Clerk { get; set; }

        public DbSet<src.Models.Stalls> Stalls { get; set; }

        public DbSet<src.Models.Traders> Traders { get; set; }

        public DbSet<src.Models.UserRole> UserRole { get; set; }

        public DbSet<src.Models.Roles> Role { get; set; }

        public DbSet<src.Models.ApplicationUser> ApplicationUser { get; set; }
    }
}
