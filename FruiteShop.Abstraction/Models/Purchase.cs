using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FruiteShop.Abstraction.Models
{
    public class Purchase
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public double TotalPrice { get; set; }

        public string Address { get; set; }

        public DateTime PurchasedDate { get; set; }
        public string Status { get; set; }

        public DateTime? DeliveredDate { get; set; }

        public string StatusReason { get; set; }

        public List<Orders>? Orders { get; set; }

        [NotMapped]
        public string UserName { get; set; }
    }
}
