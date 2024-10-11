using Microsoft.EntityFrameworkCore;
using ST10251759_PROG6212_POE.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ST10251759_PROG6212_POE.Data
{
    public class Prog6212DbContext : IdentityDbContext
    {
        public Prog6212DbContext(DbContextOptions<Prog6212DbContext> options) : base(options)
        {
        }

        // DbSet for Claims
        public DbSet<Claim> Claims { get; set; }

        // DbSet for Documents
        public DbSet<Document> Documents { get; set; }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships and table settings here if necessary
            modelBuilder.Entity<Claim>()
                .HasOne(c => c.ApplicationUser)
                .WithMany(u => u.Claims)
                .HasForeignKey(c => c.ApplicationUserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Document>()
                .HasOne(d => d.Claim)
                .WithMany(c => c.Documents)
                .HasForeignKey(d => d.ClaimId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
