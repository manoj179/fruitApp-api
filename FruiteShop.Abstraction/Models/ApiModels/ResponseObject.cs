using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FruiteShop.Abstraction.Models.ApiModels
{
    public class ResponseObject
    {
        public bool Status { get; set; }

        public string Message { get; set; }

        public string Exception { get; set; }

        public dynamic Data { get; set; }
    }
}
