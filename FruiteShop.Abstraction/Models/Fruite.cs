using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FruiteShop.Abstraction.Models
{
    public class Fruite
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double PricePerKG { get; set; }
        public double Availibility { get; set; }
        public bool IsActive { get; set; }
        public DateTime? LastModified { get; set; }
    }
}
