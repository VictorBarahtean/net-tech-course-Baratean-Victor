using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BlazorProject.Server.Models;
using BlazorProject.Server.Data;
using BlazorProject.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Wallet = BlazorProject.Server.Models.Wallet;
using BlazorProject.Server.Helpers;

namespace BlazorProject.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;

        public WalletController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }


        [HttpGet]
        public List<Wallet> GetWallets()
        {
            var userId = userManager.GetUserId(User);
            var wallets = context.Users.Include(x => x.Wallets).FirstOrDefault(x => x.Id == userId).Wallets;
            return wallets;
        }

        [HttpGet]
        [Route("{id}")]
        public Wallet GetWallet(Guid id)
        {
            var userId = userManager.GetUserId(User);
            var wallet = context.Users.Include(x => x.Wallets).FirstOrDefault(x => x.Id == userId).Wallets.FirstOrDefault(x => x.Id == id);
            return wallet;
        }

        [HttpPost]
        public IActionResult CreateWallet([FromQuery] string currency)
        {
            if(!CurrencyManager.Currencies.Contains(currency))
            {
                return BadRequest();
            }
            var userId = userManager.GetUserId(User);

            var user = context.Users.Include(x => x.Wallets).FirstOrDefault(x => x.Id == userId);

            if (user.Wallets.Any(x => x.Currency == currency))
            {
                return BadRequest();
            }

            var wallet = new Wallet
            {
                Amount = 0,
                Currency = currency
            };

            

            if (user.Wallets == null)
            {
                user.Wallets = new List<Wallet>();
            }

            user.Wallets.Add(wallet);

            context.SaveChanges();

            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteWallet([FromRoute] Guid id)
        {
            var userId = userManager.GetUserId(User);

            var user = context.Users.Include(x => x.Wallets).FirstOrDefault(x => x.Id == userId);

            if (!user.Wallets.Any(x => x.Id == id))
            {
                return BadRequest();
            }
            var wallet = context.Wallets.Find(id);
            context.Wallets.Remove(wallet);
            context.SaveChanges();

            return Ok();
        }
        [HttpPost]
        [Route("transfer")]
        public ActionResult MakeTransfer([FromBody] TransferDTO data)
        {
            var userId = userManager.GetUserId(User);

            var user = context.Users.Include(x => x.Wallets).FirstOrDefault(x => x.Id == userId);

            if (!user.Wallets.Any(x => x.Currency == data.Currency))
            {
                return BadRequest();
            }
            var source = user.Wallets.FirstOrDefault(x => x.Currency == data.Currency);

            if (source.Amount < data.Amount)
            {
                return BadRequest();
            }

            var destinationUser = context.Users.Include(x => x.Wallets).FirstOrDefault(x => x.UserName == data.Username);

            var destination = destinationUser.Wallets.FirstOrDefault(x => x.Currency == data.Currency);

            if(destination == null)
            {
                destination = new Wallet
                {
                    Amount = 0,
                    Currency = data.Currency 
                };
                destinationUser.Wallets.Add(destination);
            }

            source.Amount -= data.Amount;
            destination.Amount += data.Amount;

            var transaction = new Transaction
            {
                Amount = data.Amount,
                Date = DateTime.Now,
                DestinationWalletId = destination.Id,
                SourceWalletId = source.Id
            };

            context.Add(transaction);
            context.SaveChanges();

            return Ok();
        }
        [HttpGet]
        [Route("transfers/{itemsPerPage}/{pageNumber}")]
        public TransactionHistoryData GetTransactions(int itemsPerPage, int pageNumber, [FromQuery] Direction direction)
        {
            var userId = userManager.GetUserId(User);

            var walletIds = context.Wallets.Where(w => w.ApplicationUserId == userId).Select(w => w.Id).ToList();

            var query = context.Transactions.Where(t => walletIds.Contains(t.DestinationWalletId) || walletIds.Contains(t.SourceWalletId));

            Transaction[] transactions = null;
            switch (direction)
            {
                case Direction.None:
                    transactions = query.OrderByDescending(x => x.Date).Skip((pageNumber - 1) * itemsPerPage).Take(itemsPerPage).ToArray();
                    break;
                case Direction.Inbound:
                    transactions = query.OrderByDescending(x => x.Date).Skip((pageNumber - 1) * itemsPerPage).Take(itemsPerPage).ToArray();
                    break;
                case Direction.Outbound:
                    transactions = query.OrderByDescending(x => x.Date).Skip((pageNumber - 1) * itemsPerPage).Take(itemsPerPage).ToArray();
                    break;
            }
            

            var transactionsData = new TransactionHistoryData
            {
                Transactions = transactions.Select(DomainMapper.ToDto).ToArray(),
                ItemCount = query.Count() 
            };

            return transactionsData;
        }
    }
}
