using FruiteShop.Abstraction.Models.ApiModels;
using FruiteShop.Abstraction.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FruiteShop.Abstraction.Interfaces
{
    public interface IPurchase
    {
        Task<ResponseObject> GetPurchaseListByUserID(int userId);

        Task<ResponseObject> GetPurchaseByID(int purchaseId);

        Task<ResponseObject> AddPurchase(int cartId);

        Task<ResponseObject> ChangeOrderStatus(Purchase data);
    }
}
