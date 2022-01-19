using graduationProject.Models;
using graduationProject.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace graduationProject.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        //to change name of identity tables
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>().ToTable("Users", "security");
            builder.Entity<IdentityRole>().ToTable("Roles", "security");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles", "security");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims", "security");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins", "security");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims", "security");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens", "security");
            //to make composite key on userreservation
            //builder.Entity<UserReservation>()
            //.HasKey(p => new { p.User_Id, p.Hos_Id });

        }
        //to add database set of models table
        public DbSet<Governorate> Governorates { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<UserReservation> UserReservations { get; set; }
        //public DbSet<graduationProject.Models.Hospital> Hospitals { get; set; }
        public DbSet<Hospital> Hospitals { get; set; }
        public DbSet<BloodBank> BloodBanks { get; set; }
        public DbSet<OxygenTube> OxygenTubes { get; set; }
        public DbSet<UserReservationOxegin> UserReservationOxegins { get; set; }

    }
}
