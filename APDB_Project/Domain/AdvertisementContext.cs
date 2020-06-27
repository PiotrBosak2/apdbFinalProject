using APDB_Project.Utilities;
using Microsoft.EntityFrameworkCore;

namespace APDB_Project.Domain
{
    public class AdvertisementContext :DbContext
    {
        public DbSet<Banner> Banners { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<Token> Tokens { get; set; }

        public AdvertisementContext()
        {
            
        }

        public AdvertisementContext(DbContextOptions options):base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Building>(entity =>
            {
                entity.HasKey(b => b.IdBuilding);
                entity.Property(b => b.IdBuilding).ValueGeneratedOnAdd();
                entity.ToTable("Building2");
                
            });
            modelBuilder.Entity<Client>(entity =>
            {
                entity.HasKey(c => c.IdClient);
                entity.Property(c => c.IdClient).ValueGeneratedOnAdd();
                entity.ToTable("Client2");
                entity.HasIndex(e => e.Login).IsUnique();
                entity.Property(c => c.FirstName).HasMaxLength(100);

                entity.HasMany(c => c.Campaigns)
                    .WithOne(c => c.Client)
                    .HasForeignKey(c => c.IdClient)
                    .OnDelete(DeleteBehavior.NoAction);
            });
            modelBuilder.Entity<Campaign>(entity =>
            {
                entity.HasKey(c => c.IdCampaign);
                entity.Property(c => c.IdCampaign).ValueGeneratedOnAdd();
                entity.ToTable("Campaign2");

                entity.HasMany(c => c.Banners)
                    .WithOne(b => b.Campaign)
                    .HasForeignKey(b => b.IdCampaign)
                    .IsRequired();
                entity.HasOne(c => c.ToBuilding)
                    .WithMany(c => c.ToBuildingCampaigns)
                    .HasForeignKey(c => c.ToIdBuilding)
                    .OnDelete(DeleteBehavior.NoAction)
                    .IsRequired();

                entity.HasOne(c => c.FromBuilding)
                    .WithMany(c => c.FromBuildingCampaigns)
                    .HasForeignKey(c => c.FromIdBuilding)
                    .OnDelete(DeleteBehavior.NoAction)
                    .IsRequired();

            });

            modelBuilder.Entity<Banner>(entity =>
            {
                entity.HasKey(c => c.IdBanner);
                entity.Property(c => c.IdBanner).ValueGeneratedOnAdd();
                entity.ToTable("Banner2");
            });

            modelBuilder.Entity<Token>(entity =>
            {
                entity.HasKey(c => new {c.AccessToken, c.RefreshToken});
            });
        }
        
    }
}