namespace RideSharing.API.Models.DTO
{
    public record ApiResponse<T>(T Data, bool Success = true, string? Error = null);
    public record PagedResult<T>(IEnumerable<T> Items, int Page, int PageSize, long Total);
}
