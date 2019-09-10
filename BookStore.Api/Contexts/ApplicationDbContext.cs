using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Api.Contexts
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Category> Categories { set; get; }
        public DbSet<Book> Books { set; get; }

        protected override void OnModelCreating(ModelBuilder builder)
        {

//            builder.Entity<IdentityRole>().HasData(
//                new IdentityRole() {Name = "admin", NormalizedName = "admin"},
//                new IdentityRole() {Name = "manager", NormalizedName = "manager"},
//                new IdentityRole() {Name = "customer", NormalizedName = "admin"});
               
            base.OnModelCreating(builder);
        }
    }
}
