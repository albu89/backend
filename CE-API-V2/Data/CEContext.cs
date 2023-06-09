using Microsoft.EntityFrameworkCore;

namespace CE_API_V2.Data
{
    public class CEContext : DbContext
    {
        public CEContext() { }

        public CEContext(DbContextOptions<CEContext> options) : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}