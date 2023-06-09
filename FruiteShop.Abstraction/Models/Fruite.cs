using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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

        public string ImgUrl { get; set; }

        [NotMapped]
        public IFormFile ImgFile { get; set; } 
    }
}
