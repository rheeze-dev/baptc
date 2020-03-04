﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using src.Data;
using src.Enum;
using System;

namespace src.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("src.Models.AbsenceRequest", b =>
                {
                    b.Property<Guid>("absenceRequestId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("absenceType")
                        .IsRequired()
                        .HasMaxLength(300);

                    b.Property<string>("approvalStatus")
                        .HasMaxLength(300);

                    b.Property<DateTime>("fillingDate")
                        .HasMaxLength(300);

                    b.Property<DateTime>("inclusiveDates")
                        .HasMaxLength(300);

                    b.Property<string>("reasons");

                    b.Property<string>("remarks");

                    b.Property<string>("supervisor");

                    b.Property<int>("totalNumberOfDays");

                    b.HasKey("absenceRequestId");

                    b.ToTable("AbsenceRequest");
                });

            modelBuilder.Entity("src.Models.Accreditation", b =>
                {
                    b.Property<Guid>("accreditationId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("address");

                    b.Property<string>("areaPlanted")
                        .HasMaxLength(300);

                    b.Property<string>("crops")
                        .HasMaxLength(300);

                    b.Property<string>("farmerName")
                        .HasMaxLength(300);

                    b.Property<string>("monthHarvested");

                    b.Property<string>("monthPlanted");

                    b.Property<string>("municipality");

                    b.Property<string>("plateNumber")
                        .IsRequired()
                        .HasMaxLength(300);

                    b.Property<string>("totalLandArea");

                    b.HasKey("accreditationId");

                    b.ToTable("Accreditation");
                });

            modelBuilder.Entity("src.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FullName")
                        .HasMaxLength(100);

                    b.Property<bool>("IsCustomer");

                    b.Property<bool>("IsSuperAdmin");

                    b.Property<bool>("IsSupportAgent");

                    b.Property<bool>("IsSupportEngineer");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("ProfilePictureUrl")
                        .HasMaxLength(250);

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<int>("UserId");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.Property<string>("WallpaperPictureUrl")
                        .HasMaxLength(250);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("src.Models.Clerk", b =>
                {
                    b.Property<Guid>("clerkId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("classification");

                    b.Property<DateTime>("clerkDate")
                        .HasMaxLength(300);

                    b.Property<string>("monthPaid")
                        .HasMaxLength(300);

                    b.Property<string>("orNumber")
                        .HasMaxLength(300);

                    b.Property<string>("payor");

                    b.Property<double>("totalAmount")
                        .HasMaxLength(300);

                    b.HasKey("clerkId");

                    b.ToTable("Clerk");
                });

            modelBuilder.Entity("src.Models.Compensatory", b =>
                {
                    b.Property<Guid>("compensatoryId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("applicationDate")
                        .HasMaxLength(300);

                    b.Property<string>("approvalStatus");

                    b.Property<string>("daysAvailable");

                    b.Property<DateTime>("requestDate")
                        .HasMaxLength(300);

                    b.Property<string>("supervisor")
                        .HasMaxLength(300);

                    b.HasKey("compensatoryId");

                    b.ToTable("Compensatory");
                });

            modelBuilder.Entity("src.Models.Contact", b =>
                {
                    b.Property<Guid>("contactId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreateAt");

                    b.Property<string>("CreateBy");

                    b.Property<string>("applicationUserId");

                    b.Property<string>("contactName")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<Guid>("customerId");

                    b.Property<string>("description")
                        .HasMaxLength(200);

                    b.Property<string>("email")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("linkedin")
                        .HasMaxLength(100);

                    b.Property<string>("phone")
                        .HasMaxLength(20);

                    b.Property<string>("secondaryEmail")
                        .HasMaxLength(100);

                    b.Property<string>("thumbUrl")
                        .HasMaxLength(255);

                    b.Property<string>("website")
                        .HasMaxLength(100);

                    b.HasKey("contactId");

                    b.HasIndex("applicationUserId");

                    b.HasIndex("customerId");

                    b.ToTable("Contact");
                });

            modelBuilder.Entity("src.Models.Customer", b =>
                {
                    b.Property<Guid>("customerId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreateAt");

                    b.Property<string>("CreateBy");

                    b.Property<string>("address")
                        .HasMaxLength(100);

                    b.Property<string>("customerName")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<int>("customerType");

                    b.Property<string>("description")
                        .HasMaxLength(200);

                    b.Property<string>("email")
                        .HasMaxLength(100);

                    b.Property<string>("linkedin")
                        .HasMaxLength(100);

                    b.Property<Guid>("organizationId");

                    b.Property<string>("phone")
                        .HasMaxLength(20);

                    b.Property<string>("thumbUrl")
                        .HasMaxLength(255);

                    b.Property<string>("website")
                        .HasMaxLength(100);

                    b.HasKey("customerId");

                    b.HasIndex("organizationId");

                    b.ToTable("Customer");
                });

            modelBuilder.Entity("src.Models.DayOffAuthorization", b =>
                {
                    b.Property<Guid>("dayOffAuthorizationId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("absenceId");

                    b.Property<string>("approveStatus")
                        .HasMaxLength(300);

                    b.Property<string>("expectedOutput")
                        .HasMaxLength(300);

                    b.Property<string>("remarks")
                        .HasMaxLength(300);

                    b.Property<string>("supervisor");

                    b.Property<string>("tasks")
                        .IsRequired()
                        .HasMaxLength(300);

                    b.HasKey("dayOffAuthorizationId");

                    b.ToTable("DayOffAuthorization");
                });

            modelBuilder.Entity("src.Models.Employee", b =>
                {
                    b.Property<Guid>("employeeId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("designationOffice")
                        .HasMaxLength(300);

                    b.Property<string>("employeeName")
                        .IsRequired()
                        .HasMaxLength(300);

                    b.Property<DateTime>("employmentDate");

                    b.Property<string>("position");

                    b.Property<string>("totalAttendance")
                        .HasMaxLength(300);

                    b.Property<string>("userPassword")
                        .HasMaxLength(300);

                    b.HasKey("employeeId");

                    b.ToTable("Employee");
                });

            modelBuilder.Entity("src.Models.Finance", b =>
                {
                    b.Property<Guid>("financeId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("financeName")
                        .IsRequired()
                        .HasMaxLength(300);

                    b.HasKey("financeId");

                    b.ToTable("Finance");
                });

            modelBuilder.Entity("src.Models.Hr", b =>
                {
                    b.Property<Guid>("hrId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("hrName")
                        .IsRequired()
                        .HasMaxLength(300);

                    b.HasKey("hrId");

                    b.ToTable("Hr");
                });

            modelBuilder.Entity("src.Models.HrForm", b =>
                {
                    b.Property<Guid>("HrFormId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("absence")
                        .HasMaxLength(300);

                    b.Property<string>("compensatory")
                        .IsRequired()
                        .HasMaxLength(300);

                    b.Property<string>("dayOffReport");

                    b.Property<DateTime>("requestDate");

                    b.HasKey("HrFormId");

                    b.ToTable("HrForm");
                });

            modelBuilder.Entity("src.Models.Inspector", b =>
                {
                    b.Property<Guid>("inspectorId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("controlId");

                    b.Property<DateTime>("dateChecked")
                        .HasMaxLength(300);

                    b.Property<string>("inspectorName")
                        .IsRequired()
                        .HasMaxLength(300);

                    b.Property<string>("typeOfTransaction");

                    b.HasKey("inspectorId");

                    b.ToTable("Inspector");
                });

            modelBuilder.Entity("src.Models.Module", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.Property<bool>("Selected");

                    b.HasKey("Id");

                    b.ToTable("Modules");
                });

            modelBuilder.Entity("src.Models.Organization", b =>
                {
                    b.Property<Guid>("organizationId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreateAt");

                    b.Property<string>("CreateBy");

                    b.Property<string>("description")
                        .HasMaxLength(200);

                    b.Property<string>("organizationName")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("organizationOwnerId");

                    b.Property<string>("thumbUrl")
                        .HasMaxLength(255);

                    b.HasKey("organizationId");

                    b.ToTable("Organization");
                });

            modelBuilder.Entity("src.Models.PriceCommodity", b =>
                {
                    b.Property<Guid>("priceCommodityId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("classVariety");

                    b.Property<string>("commodity");

                    b.Property<DateTime>("commodityDate");

                    b.Property<string>("commodityRemarks");

                    b.Property<double>("priceRange");

                    b.Property<DateTime>("time");

                    b.HasKey("priceCommodityId");

                    b.ToTable("PriceCommodity");
                });

            modelBuilder.Entity("src.Models.Product", b =>
                {
                    b.Property<Guid>("productId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreateAt");

                    b.Property<string>("CreateBy");

                    b.Property<string>("description")
                        .HasMaxLength(200);

                    b.Property<Guid>("organizationId");

                    b.Property<int>("productCategory");

                    b.Property<string>("productName")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("thumbUrl")
                        .HasMaxLength(255);

                    b.HasKey("productId");

                    b.HasIndex("organizationId");

                    b.ToTable("Product");
                });

            modelBuilder.Entity("src.Models.Repair", b =>
                {
                    b.Property<Guid>("repairId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("contactNumber");

                    b.Property<string>("destination")
                        .HasMaxLength(300);

                    b.Property<string>("driverName");

                    b.Property<string>("ownerName")
                        .HasMaxLength(300);

                    b.Property<string>("remarks");

                    b.Property<string>("repairDetails")
                        .HasMaxLength(300);

                    b.Property<DateTime>("repairTime");

                    b.Property<string>("requestedName");

                    b.Property<string>("sideMarkings")
                        .IsRequired()
                        .HasMaxLength(300);

                    b.HasKey("repairId");

                    b.ToTable("Repair");
                });

            modelBuilder.Entity("src.Models.Roles", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateAdded");

                    b.Property<string>("FullName");

                    b.Property<string>("Module");

                    b.Property<string>("Name");

                    b.Property<string>("Remarks");

                    b.Property<bool>("Selected");

                    b.Property<string>("ShortName");

                    b.HasKey("Id");

                    b.ToTable("Role");
                });

            modelBuilder.Entity("src.Models.Stalls", b =>
                {
                    b.Property<Guid>("stallsId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("payment")
                        .HasMaxLength(300);

                    b.Property<string>("remarks");

                    b.Property<string>("stallOwner")
                        .IsRequired()
                        .HasMaxLength(300);

                    b.Property<string>("transferRequest");

                    b.HasKey("stallsId");

                    b.ToTable("Stalls");
                });

            modelBuilder.Entity("src.Models.SupportAgent", b =>
                {
                    b.Property<Guid>("supportAgentId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreateAt");

                    b.Property<string>("CreateBy");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("applicationUserId");

                    b.Property<Guid>("organizationId");

                    b.Property<string>("supportAgentName")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.HasKey("supportAgentId");

                    b.HasIndex("applicationUserId");

                    b.HasIndex("organizationId");

                    b.ToTable("SupportAgent");
                });

            modelBuilder.Entity("src.Models.SupportEngineer", b =>
                {
                    b.Property<Guid>("supportEngineerId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("applicationUserId");

                    b.Property<Guid>("organizationId");

                    b.Property<string>("supportEngineerName")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.HasKey("supportEngineerId");

                    b.HasIndex("applicationUserId");

                    b.HasIndex("organizationId");

                    b.ToTable("SupportEngineer");
                });

            modelBuilder.Entity("src.Models.Ticket", b =>
                {
                    b.Property<Guid>("ticketId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreateAt");

                    b.Property<string>("CreateBy");

                    b.Property<Guid>("contactId");

                    b.Property<Guid>("customerId");

                    b.Property<string>("description")
                        .HasMaxLength(200);

                    b.Property<string>("email")
                        .HasMaxLength(100);

                    b.Property<Guid>("organizationId");

                    b.Property<string>("phone")
                        .HasMaxLength(20);

                    b.Property<Guid>("productId");

                    b.Property<Guid>("supportAgentId");

                    b.Property<Guid>("supportEngineerId");

                    b.Property<int>("ticketChannel");

                    b.Property<string>("ticketName")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<int>("ticketPriority");

                    b.Property<int>("ticketStatus");

                    b.Property<int>("ticketType");

                    b.HasKey("ticketId");

                    b.HasIndex("organizationId");

                    b.ToTable("Ticket");
                });

            modelBuilder.Entity("src.Models.Ticketing", b =>
                {
                    b.Property<Guid>("ticketingId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("gatePassDate");

                    b.Property<string>("plateNumber");

                    b.Property<DateTime>("timeIn");

                    b.Property<DateTime>("timeOut");

                    b.Property<string>("typeOfTransaction");

                    b.HasKey("ticketingId");

                    b.ToTable("Ticketing");
                });

            modelBuilder.Entity("src.Models.Traders", b =>
                {
                    b.Property<Guid>("tradersId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("address")
                        .HasMaxLength(300);

                    b.Property<string>("contactNumber");

                    b.Property<int>("stallId");

                    b.Property<string>("traderName")
                        .IsRequired()
                        .HasMaxLength(300);

                    b.HasKey("tradersId");

                    b.ToTable("Traders");
                });

            modelBuilder.Entity("src.Models.UserRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateAdded");

                    b.Property<string>("Modules");

                    b.Property<string>("Remarks");

                    b.Property<string>("RoleId");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.ToTable("UserRole");
                });

            modelBuilder.Entity("src.Models.Watchmen", b =>
                {
                    b.Property<Guid>("watchmenId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("otherReports");

                    b.Property<string>("repairCheck")
                        .HasMaxLength(300);

                    b.Property<string>("watchmenName")
                        .IsRequired()
                        .HasMaxLength(300);

                    b.HasKey("watchmenId");

                    b.ToTable("Watchmen");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("src.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("src.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("src.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("src.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("src.Models.Contact", b =>
                {
                    b.HasOne("src.Models.ApplicationUser", "applicationUser")
                        .WithMany()
                        .HasForeignKey("applicationUserId");

                    b.HasOne("src.Models.Customer", "customer")
                        .WithMany("contacts")
                        .HasForeignKey("customerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("src.Models.Customer", b =>
                {
                    b.HasOne("src.Models.Organization", "organization")
                        .WithMany("customers")
                        .HasForeignKey("organizationId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("src.Models.Product", b =>
                {
                    b.HasOne("src.Models.Organization", "organization")
                        .WithMany("products")
                        .HasForeignKey("organizationId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("src.Models.SupportAgent", b =>
                {
                    b.HasOne("src.Models.ApplicationUser", "applicationUser")
                        .WithMany()
                        .HasForeignKey("applicationUserId");

                    b.HasOne("src.Models.Organization", "organization")
                        .WithMany("supportAgents")
                        .HasForeignKey("organizationId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("src.Models.SupportEngineer", b =>
                {
                    b.HasOne("src.Models.ApplicationUser", "applicationUser")
                        .WithMany()
                        .HasForeignKey("applicationUserId");

                    b.HasOne("src.Models.Organization", "organization")
                        .WithMany("supportEngineers")
                        .HasForeignKey("organizationId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("src.Models.Ticket", b =>
                {
                    b.HasOne("src.Models.Organization", "organization")
                        .WithMany("tickets")
                        .HasForeignKey("organizationId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
