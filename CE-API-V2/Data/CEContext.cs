using Microsoft.EntityFrameworkCore;
using CE_API_V2.Models;

namespace CE_API_V2.Data
{
    public class CEContext : DbContext
    {

        public DbSet<ScoringRequest> ScoringRequests { get; set; }
        public DbSet<ScoringResponse> ScoringResponses { get; set; }

        public CEContext() { }

        public CEContext(DbContextOptions<CEContext> options) : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Biomarkers>()
                .HasOne(b => b.Request)
                .WithOne(r => r.Biomarkers)
                .HasForeignKey<Biomarkers>(b => b.RequestId)
                ;

            modelBuilder.Entity<ScoringResponse>()
                .HasOne(b => b.Request)
                .WithOne(r => r.Response)
                .HasForeignKey<ScoringResponse>(r => r.RequestId)
                ;
            modelBuilder.Entity<ScoringResponse>()
                .Property(r => r.CreatedOn)
                .HasDefaultValueSql("getdate()");
            
            modelBuilder.Entity<ScoringRequest>()
                .Property(r => r.CreatedOn)
                .HasDefaultValueSql("getdate()");
            
            modelBuilder.Entity<Biomarkers>()
                .Property(r => r.CreatedOn)
                .HasDefaultValueSql("getdate()");

        }
    }
}