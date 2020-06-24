using System;
using System.Collections.Generic;
using System.Text;
using GraniteCore2_1.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GraniteCore2_1.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<SpecialTag> SpecialTags { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<ProductsSelectedForAppointment> ProductsSelectedForAppointments { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    }
}
