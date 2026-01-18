namespace HotelMVCPrototype.Services.Interfaces
{
    public interface IAuditLogger
    {
        Task LogAsync(string action, string entityType, int? entityId = null,
                      string? description = null, object? data = null);
    }

}
