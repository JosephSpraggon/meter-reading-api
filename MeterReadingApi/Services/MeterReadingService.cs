using System.Globalization;
using System.Text.RegularExpressions;
using CsvHelper;
using MeterReadingApi.Data.Interfaces;
using MeterReadingApi.Dtos;
using MeterReadingApi.Models;
using MeterReadingApi.Services.Interfaces;

namespace MeterReadingApi.Services
{
    public class MeterReadingService : IMeterReadingService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMeterReadingsRepository _meterReadingsRepository;
        public MeterReadingService(IAccountRepository accountRepository, IMeterReadingsRepository meterReadingsRepository)
        {
            _accountRepository = accountRepository;
            _meterReadingsRepository = meterReadingsRepository;
        }

        public async Task<ServiceResult> ProcessMeterReadingCsv(IFormFile file)
        {
            var serviceResult = new ServiceResult();

            if (file == null || file.Length == 0)
            {
                serviceResult.AddCriticalError("No file uploaded or the file is empty");
                return serviceResult;
            }

            if (!Path.GetExtension(file.FileName).Equals(".csv", System.StringComparison.OrdinalIgnoreCase))
            {
                serviceResult.AddCriticalError("Only CSV files are allowed");
                return serviceResult;
            }

            using var reader = new StreamReader(file.OpenReadStream());
            using var csv = new CsvReader(reader, new CultureInfo("en-GB"));
            
            try
            {
                var records = csv.GetRecords<MeterReadingCsvRecordDto>().ToList();

                foreach (var record in records)
                { 
                    var account = await _accountRepository.GetByIdAsync(record.AccountId);
                    if (account == null)
                    {
                        serviceResult.AddErrorMessage($"Account not found for AccountId {record.AccountId}");
                        continue;
                    }

                    string formattedMeterReadValue = record.MeterReadValue.ToString("D5");
                    if (!Regex.IsMatch(formattedMeterReadValue, @"^\d{5}$"))
                    {
                        serviceResult.AddErrorMessage($"Invalid meter reading value '{formattedMeterReadValue}' for AccountId 1 on {record.MeterReadingDateTime:dd/MM/yyyy HH:mm}");
                        continue;
                    }
                    var meterReadingValue = int.Parse(formattedMeterReadValue);

                    bool isDuplicate = await _meterReadingsRepository.DoesMeterReadingExistAsync(
                        record.AccountId, 
                        record.MeterReadingDateTime, 
                        meterReadingValue);
                        
                    if (isDuplicate)
                    {
                        serviceResult.AddErrorMessage($"Duplicate meter reading detected for AccountId {record.AccountId} on {record.MeterReadingDateTime:dd/MM/yyyy HH:mm}");
                        continue;
                    }

                    var latestReadingDate = await _meterReadingsRepository.GetLatestReadingDateAsync(record.AccountId);
                    if (latestReadingDate.HasValue && record.MeterReadingDateTime <= latestReadingDate.Value)
                    {
                        serviceResult.AddErrorMessage($"Meter reading for AccountId {record.AccountId} is older than existing meter reading on {latestReadingDate.Value:dd/MM/yyyy HH:mm}");
                        continue;
                    }

                    var meterReading = new MeterReading
                    {
                        Account = account,
                        DateTime = record.MeterReadingDateTime.ToUniversalTime(),
                        Value = meterReadingValue
                    };
                    _meterReadingsRepository.Add(meterReading);

                    serviceResult.AddSuccessfulResult();
                }

                await _meterReadingsRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                serviceResult.AddCriticalError($"Error processing CSV file: {ex.Message}");
            }

            return serviceResult;
        }
    }

}