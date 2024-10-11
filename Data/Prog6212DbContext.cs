using Microsoft.EntityFrameworkCore;
using ST10251759_PROG6212_POE.Models;

namespace ST10251759_PROG6212_POE.Data
{
    public class Prog6212DbContext : DbContext
    {
        public Prog6212DbContext(DbContextOptions<Prog6212DbContext> options) : base(options)
        {
        }
        public DbSet<ST10251759_PROG6212_POE.Models.Claim> Claim { get; set; } = default!;
    }
}
