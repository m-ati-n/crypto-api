using CryptoMarket.Data;
using CryptoMarket.Models;
using Microsoft.EntityFrameworkCore;

namespace CryptoMarket.Repositories
{

    public class CoinRepository : ICoinRepository
    {
        private readonly ApplicationDbContext _context;

        public CoinRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Coin?> GetByCoinGeckoIdAsync(string id)
        {
            return await _context.Coins
                .FirstOrDefaultAsync(c => c.CoinGeckoId == id);
        }

        public async Task AddAsync(Coin coin)
        {
            await _context.Coins.AddAsync(coin);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<int> CountAsync(string? search)
        {
            var coins = _context.Coins.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                coins = coins.Where(c =>
                    EF.Functions.Like(c.Name, $"%{search}%") ||
                    EF.Functions.Like(c.Symbol, $"%{search}%"));
            }

            return await coins.CountAsync();
        }

        public async Task<List<Coin>> GetPagedAsync(
            string? search,
            string? sort,
            int page,
            int pageSize)
        {
            var coins = _context.Coins.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                coins = coins.Where(c =>
                    EF.Functions.Like(c.Name, $"%{search}%") ||
                    EF.Functions.Like(c.Symbol, $"%{search}%"));
            }

            switch (sort)
            {
                case "priceAsc":
                    coins = coins.OrderBy(c => c.CurrentPrice);
                    break;

                case "priceDesc":
                    coins = coins.OrderByDescending(c => c.CurrentPrice);
                    break;

                case "marketCapAsc":
                    coins = coins.OrderBy(c => c.MarketCap);
                    break;

                case "marketCapDesc":
                    coins = coins.OrderByDescending(c => c.MarketCap);
                    break;

                case "nameAsc":
                    coins = coins.OrderBy(c => c.Name);
                    break;

                case "nameDesc":
                    coins = coins.OrderByDescending(c => c.Name);
                    break;

                default:
                    coins = coins.OrderByDescending(c => c.MarketCap);
                    break;
            }

            return await coins
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        public async Task AddPriceHistoryAsync(CoinPriceHistory history)
        {
            await _context.CoinPriceHistories.AddAsync(history);
        }
        public async Task<List<CoinPriceHistory>> GetHistoryAsync(
        string coinGeckoId,
        int days)
        {
            var fromDate = DateTime.UtcNow.AddDays(-days);

            return await _context.CoinPriceHistories
                .Where(h =>
                    h.Coin.CoinGeckoId == coinGeckoId &&
                    h.CreatedAt >= fromDate)
                .OrderBy(h => h.CreatedAt)
                .ToListAsync();
        }
    }
}

