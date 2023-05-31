using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FruiteShop.Abstraction.Models
{
    public class Orders
    {
        public int Id { get; set; }

        [ForeignKey("Fruite")]
        public int FruiteId { get; set; }
        
        public double Quantity { get; set; }
        
        public double PricePerKG { get; set; }
        
        public int? CartId	{get;set;}

        public int? PurchaseId	{get;set;}

        public Fruite Fruite { get; set; }
    }
}
