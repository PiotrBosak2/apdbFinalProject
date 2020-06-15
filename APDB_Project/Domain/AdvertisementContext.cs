using Microsoft.EntityFrameworkCore;

namespace APDB_Project.Domain
{
    public class AdvertisementContext :DbContext
    {
        public DbSet<Banner> Banners { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<Building> Buildings { get; set; }

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
                entity.ToTable("Building");
               
                entity.HasMany(b => b.ToBuildingCampaigns)
                    .WithOne(c => c.ToBuilding)
                    .HasForeignKey(c => c.ToIdBuilding)
                    .IsRequired();
                
                entity.HasMany(b => b.FromBuildingCampaigns)
                    .WithOne(c => c.FromBuilding)
                    .HasForeignKey(c => c.FromIdBuilding)
                    .IsRequired();
            });
            modelBuilder.Entity<Client>(entity =>
            {
                entity.HasKey(c => c.IdClient);
                entity.Property(c => c.IdClient).ValueGeneratedOnAdd();
                entity.ToTable("Client");
                
                entity.HasMany(c => c.Campaigns)
                    .WithOne(c => c.Client)
                    .HasForeignKey(c => c.IdClient)
                    .IsRequired();
            });
            modelBuilder.Entity<Campaign>(entity =>
            {
                entity.HasKey(c => c.IdCampaign);
                entity.Property(c => c.IdCampaign).ValueGeneratedOnAdd();
                entity.ToTable("Campaign");

                entity.HasMany(c => c.Banners)
                    .WithOne(b => b.Campaign)
                    .HasForeignKey(b => b.IdCampaign)
                    .IsRequired();
                
            });

            modelBuilder.Entity<Banner>(entity =>
            {
                entity.HasKey(c => c.IdBanner);
                entity.Property(c => c.IdBanner).ValueGeneratedOnAdd();
                entity.ToTable("Banner");
            });
        }
        
    }
}