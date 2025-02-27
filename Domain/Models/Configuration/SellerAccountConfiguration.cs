using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Configuration
{
    public class SellerAccountConfiguration
    {
        public const string Position = "SellerAccount";
        public decimal MinBalanceToProceed { get; set; }
        public decimal MinBalanceWarning { get; set; }
    }
}
