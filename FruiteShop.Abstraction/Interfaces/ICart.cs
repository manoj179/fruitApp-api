using FruiteShop.Abstraction.Models;
using FruiteShop.Abstraction.Models.ApiModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FruiteShop.Abstraction.Interfaces
{
    public interface ICart
    {
        Task<ResponseObject> GetCartListByUserID(int userId);

        Task<ResponseObject> GetCartListByID(int cartID);

        Task<ResponseObject> AddCart(Cart data);

        Task<ResponseObject> RemoveCartItem(int cartID);

    }
}
