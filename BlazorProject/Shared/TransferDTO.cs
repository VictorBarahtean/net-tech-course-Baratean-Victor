using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorProject.Shared
{
    public class TransferDTO
    {
        public string SourceWalletId { get; set; }
        public string Username { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
    }
}
