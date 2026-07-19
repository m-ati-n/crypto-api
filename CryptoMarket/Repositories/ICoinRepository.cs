using CryptoMarket.DTOs;
using CryptoMarket.Models;

namespace CryptoMarket.Repositories
{
    public interface ICoinRepository
    {
        Task<Coin?> GetByCoinGeckoIdAsync(string id);
        Task<List<Coin>> GetPagedAsync(
            string? search,
            string? sort,
            int page,
            int pageSize);

        Task<int> CountAsync(string? search);

        Task AddAsync(Coin coin);

        Task SaveChangesAsync();

        Task AddPriceHistoryAsync(CoinPriceHistory history);

        Task<List<CoinPriceHistory>> GetHistoryAsync(
        string coinGeckoId,
        int days);
    }
}
