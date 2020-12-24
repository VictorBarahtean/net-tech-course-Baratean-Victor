using Microsoft.AspNetCore.Mvc;
using BlazorProject.Shared;

namespace BlazorProject.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        [HttpGet]
        public CurrencyList GetCurrencies()
        {
            return new CurrencyList
            {
                Currencies = CurrencyManager.Currencies
            };
        }
    }
}
