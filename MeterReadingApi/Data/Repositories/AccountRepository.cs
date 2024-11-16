using MeterReadingApi.Data.Interfaces;
using MeterReadingApi.Models;

namespace MeterReadingApi.Data.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly AppDbContext _context;

        public AccountRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<Account> GetAll()
        {
           return _context.Accounts.ToList();
        }

        public async Task<Account?> GetByIdAsync(int accountId)
        {
            return await _context.Accounts.FindAsync(accountId);
        }
    }
}