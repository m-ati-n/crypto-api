using CryptoMarket.DTOs;
using CryptoMarket.Models;

namespace CryptoMarket.Interfaces
{
    public interface ICoinGeckoService
    {
        Task SyncCoinsAsync();

        Task<CoinDto?> GetCoinByIdAsync(string id);

        Task<CoinPagedResponseDto> GetCoinsAsync(CoinQueryDto query);

        Task<List<CoinHistoryDto>> GetHistoryAsync(
        string coinGeckoId,
        int days);

    }
}