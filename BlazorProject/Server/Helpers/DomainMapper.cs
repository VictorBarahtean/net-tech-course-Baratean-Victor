using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorProject.Server.Models;
using BlazorProject.Shared;

namespace BlazorProject.Server.Helpers
{
    public static class DomainMapper
    {
        public static TransactionDto ToDto(Transaction transaction)
        {
            return transaction == null ? null : new TransactionDto
            {
                Id = transaction.Id,
                Amount = transaction.Amount,
                DestinationWalletId = transaction.DestinationWalletId,
                SourceWalletId = transaction.SourceWalletId,
                Date = transaction.Date
            };
        }
    }
}
