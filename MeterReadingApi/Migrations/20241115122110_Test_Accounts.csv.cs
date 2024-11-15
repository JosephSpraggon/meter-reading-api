using System.Globalization;
using CsvHelper;
using MeterReadingApi.Mappings;
using MeterReadingApi.Models;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeterReadingApi.Migrations
{
    /// <inheritdoc />
    public partial class Test_Accountscsv : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var csvPath = Path.Combine("Data", "Test_Accounts.csv");

            using var reader = new StreamReader(csvPath);
            using var csv = new CsvReader(reader, new CultureInfo("en-GB"));
            
            csv.Context.RegisterClassMap<AccountMap>();  
            var accounts = csv.GetRecords<Account>().ToList();

            foreach (var account in accounts)
            {
                migrationBuilder.Sql(
                    $@"INSERT INTO ""Accounts"" (""Id"", ""FirstName"", ""LastName"") 
                    VALUES ({account.Id}, '{EscapeSql(account.FirstName)}', '{EscapeSql(account.LastName)}')");
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DELETE FROM ""Accounts""");
        }

        private string EscapeSql(string input)
        {
            return input?.Replace("'", "''");
        }
    }
}
