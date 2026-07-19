using CryptoMarket.DTOs;
using CryptoMarket.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CryptoMarket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoinsController : ControllerBase
    {
        private readonly ICoinGeckoService _coinGeckoService;

        public CoinsController(ICoinGeckoService coinGeckoService)
        {
            _coinGeckoService = coinGeckoService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCoins([FromQuery] CoinQueryDto query)
        {
            var coins = await _coinGeckoService.GetCoinsAsync(query);

            return Ok(coins);
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetCoins(string id)
        {

            var coin = await _coinGeckoService.GetCoinByIdAsync(id);

            if (coin == null)
                return NotFound();

            return Ok(coin);
        }

        [HttpGet("{id}/history")]
        public async Task<IActionResult> GetHistory(
        string id,
        [FromQuery] int days = 7)
        {
            var history = await _coinGeckoService.GetHistoryAsync(id, days);

            return Ok(history);
        }

        [HttpPost("sync")]
        public async Task<IActionResult> Sync()
        {
            await _coinGeckoService.SyncCoinsAsync();

            return Ok(new
            {
                Message = "Coins synchronized successfully."
            });
        }
    }
}