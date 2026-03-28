using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Transactions :TypeOp
    {
        public string BuyerID { get; set; }

        public string SellerID { get; set; }

    }
}
