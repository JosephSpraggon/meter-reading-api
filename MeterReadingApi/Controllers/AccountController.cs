using Microsoft.AspNetCore.Mvc;
using MeterReadingApi.Models;

namespace MeterReadingApi.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
 private readonly ILogger<AccountController> _logger;

    public AccountController(ILogger<AccountController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetAccounts")]
    public IEnumerable<Account> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new Account
        {
            Id = Random.Shared.Next(1, 1000),
            FirstName = "Joe",
            LastName = "Test"
        })
        .ToArray();
    }
}
