using Microsoft.EntityFrameworkCore;
using CE_API_V2.Models;

namespace CE_API_V2.Data
{
    public class CEContext : DbContext
    {

        public DbSet<ScoringRequest> ScoringRequests { get; set; }
        public DbSet<ScoringResponse> ScoringResponses { get; set; }
        public DbSet<Biomarkers> Biomarkers { get; set; }
        public DbSet<BiomarkerOrderModel> BiomarkerOrders { get; set; }
        public DbSet<User> Users { get; set; }

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

            modelBuilder.Entity<BiomarkerOrderModel>(entity =>
            {
                entity.HasKey(b => new { b.UserId,
                    BiomarkerName = b.BiomarkerId });
                entity.HasOne(u => u.User)
                .WithMany(b => b.BiomarkerOrders)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_BiomarkerOrders_User");
            });

            modelBuilder.Entity<User>()
                .HasKey(u => u.UserId);
            modelBuilder.Entity<User>()
                .Property(u => u.CreatedOn)
                .HasDefaultValueSql("getdate()");
        }
    }
}