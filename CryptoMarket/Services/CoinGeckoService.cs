using AutoMapper;
using Azure;
using CryptoMarket.Caching;
using CryptoMarket.DTOs;
using CryptoMarket.Interfaces;
using CryptoMarket.Models;
using CryptoMarket.Repositories;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;


namespace CryptoMarket.Services
{
    public class CoinGeckoService : ICoinGeckoService
    {
        private readonly HttpClient _httpClient;
        private readonly ICoinRepository _repository;
        private readonly IMapper _mapper;
        private readonly ICacheService _cache;

        public CoinGeckoService(HttpClient httpClient , ICoinRepository repository, IMapper mapper, ICacheService cache)
        {
            _httpClient = httpClient;
            _repository = repository;
            _mapper = mapper;
            _cache = cache;
        }
        public async Task SyncCoinsAsync()
        {
            var Url = "https://api.coingecko.com/api/v3/coins/markets?vs_currency=usd&order=market_cap_desc&per_page=200&page=1&sparkline=false";
            _httpClient.DefaultRequestHeaders.Clear();

            _httpClient.DefaultRequestHeaders.Add(
                "User-Agent",
                "CryptoMarketApp/1.0"
            );
            var response = await _httpClient.GetAsync(Url);
            //response.EnsureSuccessStatusCode();
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();

                throw new Exception(
                    $"Status: {response.StatusCode}\n{error}"
                );
            }
            var json = await response.Content.ReadAsStringAsync();
            var dto = JsonSerializer.Deserialize<List<CoinGeckoCoinDto>>(json)
                    ?? new List<CoinGeckoCoinDto>();
            foreach (var item in dto)
            {
                var coin = await _repository.GetByCoinGeckoIdAsync(item.id ?? "");

                if (coin == null)
                {
                    coin = new Coin
                    {
                        CoinGeckoId = item.id ?? string.Empty,
                        Name = item.name ?? string.Empty,
                        Symbol = item.symbol ?? string.Empty,
                        Image = item.image ?? string.Empty,
                        CurrentPrice = item.current_price ?? 0,
                        MarketCap = item.market_cap ?? 0,
                        PriceChangePercentage24h = item.price_change_percentage_24h ?? 0
                    };
                    await _repository.AddAsync(coin);
                }
                else
                {
                    coin.CurrentPrice = item.current_price ?? 0;
                    coin.MarketCap = item.market_cap ?? 0;
                    coin.PriceChangePercentage24h = item.price_change_percentage_24h ?? 0;
                }
                var history = new CoinPriceHistory
                {
                    CoinId = coin.Id,
                    Price = coin.CurrentPrice ?? 0,
                    MarketCap = coin.MarketCap ?? 0,
                    PriceChangePercentage24h = coin.PriceChangePercentage24h ?? 0,
                    CreatedAt = DateTime.UtcNow
                };

                await _repository.AddPriceHistoryAsync(history);

            }
            await _repository.SaveChangesAsync();
            _cache.RemoveByPrefix("coin_");
            _cache.RemoveByPrefix("coins_");

        }
        public async Task<CoinDto?> GetCoinByIdAsync(string id)
        {
            var cacheKey = $"coin_{id}";
            var cached = _cache.Get<CoinDto>(cacheKey);
            if (cached != null)
            {
                return cached;
            }

            var coin = await _repository.GetByCoinGeckoIdAsync(id);

            if (coin == null)
                return null;

            var coinDto = _mapper.Map<CoinDto>(coin);
            _cache.Set(
            cacheKey,
            coinDto,
            TimeSpan.FromMinutes(5));
            return coinDto;

        }

        public async Task<CoinPagedResponseDto> GetCoinsAsync(CoinQueryDto query)
        {
            var cacheKey =
            $"coins_{query.Page}_{query.PageSize}_{query.Search}_{query.Sort}";
            var totalCount = await _repository.CountAsync(query.Search);
            var cached = _cache.Get<CoinPagedResponseDto>(cacheKey);
            if (cached != null)
            {
                return cached;
            }
            var items = await _repository.GetPagedAsync(
                query.Search,
                query.Sort,
                query.Page,
                query.PageSize);
            var coinDtos = _mapper.Map<List<CoinDto>>(items);
            var totalPages =
            (int)Math.Ceiling((double)totalCount / query.PageSize);
            _cache.Set(
                cacheKey,
                coinDtos,
                TimeSpan.FromMinutes(5));
            return new CoinPagedResponseDto
            {
                Items = coinDtos,
                Page = query.Page,
                PageSize = query.PageSize,
                TotalCount = totalCount,
                TotalPages = totalPages
            };

        }

        public async Task<List<CoinHistoryDto>> GetHistoryAsync(
        string coinGeckoId,
        int days)
        {
            var history = await _repository.GetHistoryAsync(
                coinGeckoId,
                days);

            return _mapper.Map<List<CoinHistoryDto>>(history);
        }

    }
}