
using MeterReadingApi.Data.Interfaces;
using MeterReadingApi.Models;
using Microsoft.EntityFrameworkCore;

namespace MeterReadingApi.Data.Repositories
{
    public class MeterReadingsRepository : IMeterReadingsRepository
    {
        private readonly AppDbContext _context;

        public MeterReadingsRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(MeterReading meterReading)
        {
            _context.MeterReadings.Add(meterReading);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DoesMeterReadingExistAsync(int accountId, DateTime meterReadingDateTime, int meterReadingValue)
        {
            return await _context.Set<MeterReading>()
                .Where(mr => mr.Account.Id == accountId
                && mr.DateTime == meterReadingDateTime.ToUniversalTime()
                && mr.Value == meterReadingValue)
                .AnyAsync();
        }

        public async Task<DateTime?> GetLatestReadingDateAsync(int accountId)
        {
            return await _context.MeterReadings
                .Where(mr => mr.Account.Id == accountId)
                .OrderByDescending(mr => mr.DateTime)
                .Select(mr => (DateTime?)mr.DateTime)
                .FirstOrDefaultAsync();
        }

    }
}
