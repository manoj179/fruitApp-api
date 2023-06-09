using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FruiteShop.Abstraction.Models
{
    public class Cart
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public double TotalPrice { get; set; }

        public bool IsInCart { get; set; }
        
        public DateTime CreatedOn { get; set; }
        
        public int? UpdatedBy { get; set; }
        
        public DateTime? UpdatedOn { get; set; }

        public List<Orders>? Orders { get; set; }

        [NotMapped]
        public string UserName { get; set; }
    }
}
