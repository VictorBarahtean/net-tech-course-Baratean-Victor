using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorProject.Shared
{
    public class TransactionHistoryData
    {
        public TransactionDto[] Transactions { get; set; }
        public int ItemCount { get; set; }
    }
}
