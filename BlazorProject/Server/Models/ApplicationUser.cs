using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorProject.Server.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LasttName { get; set; }
        public DateTime CreateOn { get; set; }
        public List<Wallet> Wallets { get; set; }
    }
}
