using MeterReadingApi.Models;

namespace MeterReadingApi.Data.Interfaces
{
    public interface IAccountRepository
    {
        List<Account> GetAll();
        Task<Account?> GetByIdAsync(int accountId);
    }
}