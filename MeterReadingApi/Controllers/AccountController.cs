using Microsoft.AspNetCore.Mvc;
using MeterReadingApi.Data.Interfaces;

namespace MeterReadingApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;

        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        [HttpGet("get-all")]
        public ActionResult GetAll()
        {
            try
            {
                var accounts = _accountRepository.GetAll();

                return StatusCode(200, accounts);
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }

        }
    }

}

