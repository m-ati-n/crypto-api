namespace CryptoMarket.DTOs
{
    public class CoinDto
    {
        public string CoinGeckoId { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public string Image { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal MarketCap { get; set; }
        public decimal PriceChangePercentage24h { get; set; }
    }
}
