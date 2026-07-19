using CryptoMarket.Models;

namespace CryptoMarket.DTOs
{
    public class CoinPagedResponseDto
    {
        public List<CoinDto> Items { get; set; } = new();

        public int Page { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public int TotalPages { get; set; }
    }
}
