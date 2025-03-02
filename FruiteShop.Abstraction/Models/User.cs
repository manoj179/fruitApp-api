﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FruiteShop.Abstraction.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string UserType { get; set; }

        public string Password { get; set; }

        public bool IsActive { get; set; }

        public string Address { get; set; }

        public int Pincode { get; set; }

        public string RefreshToken { get; set; }

        public DateTime RefreshTokenExpiry { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
