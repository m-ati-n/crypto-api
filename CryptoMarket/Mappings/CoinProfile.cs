using AutoMapper;
using CryptoMarket.DTOs;
using CryptoMarket.Models;

namespace CryptoMarket.Mappings
{
    public class CoinProfile : Profile
    {
        public CoinProfile()
        {
            CreateMap<Coin, CoinDto>();

            CreateMap<CoinPriceHistory, CoinHistoryDto>();
        }
    }
}