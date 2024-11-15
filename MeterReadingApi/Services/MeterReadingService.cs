using MeterReadingApi.Services.Interfaces;

namespace MeterReadingApi.Services
{
    public class MeterReadingService : IMeterReadingService
    {
        public async Task<ServiceResult> ProcessMeterReadingCsv(IFormFile file)
        {
            var serviceResult = new ServiceResult();

            if (file == null || file.Length == 0)
            {
                serviceResult.AddErrorMessage("No file uploaded or the file is empty.");
                return serviceResult;
            }

            if (!Path.GetExtension(file.FileName).Equals(".csv", System.StringComparison.OrdinalIgnoreCase))
            {
                serviceResult.AddErrorMessage("Only CSV files are allowed.");
                return serviceResult;
            }

            using var reader = new StreamReader(file.OpenReadStream());
            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();
                // Process each line 
                // Add logic validate
                // If valid save to db
            }

            return serviceResult;
        }
    }

}