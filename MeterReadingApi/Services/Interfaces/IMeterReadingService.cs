namespace MeterReadingApi.Services.Interfaces
{
    public interface IMeterReadingService
    {
        Task<ServiceResult> ProcessMeterReadingCsv(IFormFile file);
    }
}