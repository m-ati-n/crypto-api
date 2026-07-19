namespace CryptoMarket.DTOs;

public class ErrorResponseDto
{
    public int StatusCode { get; set; }

    public string Message { get; set; } = string.Empty;

    public DateTime TimeStamp { get; set; }

    public string? Path { get; set; }
}