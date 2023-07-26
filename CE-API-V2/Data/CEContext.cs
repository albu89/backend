using Microsoft.EntityFrameworkCore;
using CE_API_V2.Models;

namespace CE_API_V2.Data
{
    public class CEContext : DbContext
    {

        public DbSet<ScoringRequestModel> ScoringRequests { get; set; }
        public DbSet<ScoringResponseModel> ScoringResponses { get; set; }
        public DbSet<Biomarkers> Biomarkers { get; set; }
        public DbSet<BiomarkerOrderModel> BiomarkerOrders { get; set; }
        public DbSet<UserModel> Users { get; set; }

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
            modelBuilder.Entity<Biomarkers>()
                .Property(r => r.CreatedOn)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<ScoringResponseModel>()
                .ToTable("ScoringResponses")
                .HasOne(b => b.Request)
                .WithOne(r => r.Response)
                .HasForeignKey<ScoringResponseModel>(r => r.RequestId)
                ;
            modelBuilder.Entity<ScoringResponseModel>()
                .Property(r => r.CreatedOn)
                .HasDefaultValueSql("getdate()");
            
            modelBuilder.Entity<ScoringRequestModel>()
                .ToTable("ScoringRequests")
                .Property(r => r.CreatedOn)
                .HasDefaultValueSql("getdate()");
            
            modelBuilder.Entity<UserModel>()
                .ToTable("Users");
            
            modelBuilder.Entity<BiomarkerOrderModel>(entity =>
            {
                entity.HasKey(b => new { b.UserId,
                    BiomarkerName = b.BiomarkerId });
                entity.HasOne(u => u.User)
                .WithMany(b => b.BiomarkerOrders)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_BiomarkerOrders_User");
                entity.ToTable("BiomarkerOrders");
            });

            modelBuilder.Entity<UserModel>()
                .HasKey(u => u.UserId);
            modelBuilder.Entity<UserModel>()
                .Property(u => u.CreatedOn)
                .HasDefaultValueSql("getdate()");
        }
    }
}