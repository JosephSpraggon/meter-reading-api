using CsvHelper.Configuration;
using MeterReadingApi.Models;

namespace MeterReadingApi.Mappings
{
    public class AccountMap : ClassMap<Account>
    {
        public AccountMap()
        {
            Map(m => m.Id).Name("AccountId");
            Map(m => m.FirstName).Name("FirstName");
            Map(m => m.LastName).Name("LastName");
        }
    }
}
