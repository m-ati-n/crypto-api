namespace CryptoMarket.Models;

public class Coin
{
    public  Guid Id {set; get; }
    public string CoinGeckoId { get; set; } = string.Empty;
    public string Name { set; get; } = string.Empty;
    public string Symbol {set; get; } = string.Empty;
    public string Image {set; get; } = string.Empty;
    public decimal? CurrentPrice {set; get; }
    public decimal? MarketCap {set; get;}
    public decimal? PriceChangePercentage24h { set; get; }
    public ICollection<CoinPriceHistory> PriceHistories { get; set; }
    = new List<CoinPriceHistory>();
}
