using CryptoMarket.Models;
using Microsoft.EntityFrameworkCore;

namespace CryptoMarket.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Coin> Coins { get; set; }
        public DbSet<CoinPriceHistory> CoinPriceHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Coin>()
                .Property(c => c.CurrentPrice)
                .HasPrecision(18, 8);

            modelBuilder.Entity<Coin>()
                .Property(c => c.MarketCap)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Coin>()
                .Property(c => c.PriceChangePercentage24h)
                .HasPrecision(18, 8);

            modelBuilder.Entity<CoinPriceHistory>()
                .Property(c => c.Price)
                .HasPrecision(18, 8);

            modelBuilder.Entity<CoinPriceHistory>()
                .Property(c => c.MarketCap)
                .HasPrecision(18, 2);

            modelBuilder.Entity<CoinPriceHistory>()
                .Property(c => c.PriceChangePercentage24h)
                .HasPrecision(18, 8);
        }
    }
}