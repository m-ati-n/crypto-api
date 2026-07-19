namespace CryptoMarket.Models
{
    public class CoinPriceHistory
    {
        public Guid Id { get; set; }

        public Guid CoinId { get; set; }

        public Coin Coin { get; set; } = null!;

        public decimal Price { get; set; }

        public decimal MarketCap { get; set; }

        public decimal PriceChangePercentage24h { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
