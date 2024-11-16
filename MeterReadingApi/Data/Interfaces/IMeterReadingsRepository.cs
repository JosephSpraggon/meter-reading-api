using MeterReadingApi.Models;

namespace MeterReadingApi.Data.Interfaces
{
    public interface IMeterReadingsRepository
    {
        void Add(MeterReading meterReading);
        Task SaveChangesAsync();
        Task<bool> DoesMeterReadingExistAsync(int accountId, DateTime meterReadingDateTime, int meterReadingValue);
        Task<DateTime?> GetLatestReadingDateAsync(int accountId);

    }
}
