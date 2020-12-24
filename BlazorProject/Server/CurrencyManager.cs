using System.Collections.Generic;

namespace BlazorProject.Server
{
    public static class CurrencyManager
    {
        public static List<string> Currencies {get;set;}

        static CurrencyManager()
        {
            Currencies = new List<string>
            {
                "EUR",
                "USD",
                "MDL",
                "EC"
            };
        }
    }
}
