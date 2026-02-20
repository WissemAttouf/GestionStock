using Microsoft.EntityFrameworkCore;
using FirstProject.Models;

namespace FirstProject.dta  // ← Attention : c'est dta, pas Data !
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
    }
}