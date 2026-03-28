
using Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class AppDBContext : IdentityDbContext<AppUser>
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) 
        {  
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUser>().ToTable("UsersAccount", "security");
            builder.Entity<IdentityRole>().ToTable("Roles", "security");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRole", "security");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaim", "security");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogin", "security");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaim", "security");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserToken", "security");
        }
        public DbSet<FeedBack> FeedBack { get; set; }
        public DbSet<Property> Property { get; set; }
        public DbSet<Save> save { get; set; }
        public DbSet<Transactions> Transactions { get; set; }
        public DbSet<DetailsVilla> DetailsVilla { get; set; }
        public DbSet<DetailsApartment> DetailsApartment { get; set; }
        public DbSet<DetailsFloor> DetailsFloor { get; set; }
        public DbSet<Rent> Rent { get; set; }


    }
}
