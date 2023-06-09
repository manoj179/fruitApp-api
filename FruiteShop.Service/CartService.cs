using FruiteShop.Abstraction.Interfaces;
using FruiteShop.Abstraction.Models;
using FruiteShop.Abstraction.Models.ApiModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FruiteShop.Service
{
    public class CartService : ICart
    {
        private readonly FruiteContext dbContext;

        private ResponseObject response;

        public CartService(FruiteContext dbContext)
        {
            this.dbContext = dbContext;
            response = new ResponseObject();
        }

        public async Task<ResponseObject> GetCartListByID(int cartID)
        {
            if (cartID > 0 && dbContext.Carts.Count(m => m.Id == cartID) >= 1)
            {
                response.Status = true;
                var cart = await dbContext.Carts.Include(m => m.Orders).FirstAsync(m => m.Id == cartID && m.IsInCart == true);

                cart.UserName = dbContext.Users.First(m => m.Id == cart.UserId).Name;

                response.Data = cart;
            }
            else
            {
                response.Status = false;
                response.Message = "Data not found";
            }

            return response;
        }

        public async Task<ResponseObject> GetCartListByUserID(int userId)
        {
            if (userId > 0 && dbContext.Carts.Count(m => m.UserId == userId) >= 0)
            {
                response.Status = true;
                var cart = await dbContext.Carts.Include(m => m.Orders).Where(m => m.UserId == userId && m.IsInCart == true).ToListAsync();

                response.Data = cart;
            }
            else
            {
                response.Status = false;
                response.Message = "Data not found";
            }

            return response;
        }

        public async Task<ResponseObject> AddCart(Cart data)
        {

            if (data != null && data.Id == 0)
            {
                dbContext.Carts.Add(data);
                await dbContext.SaveChangesAsync();

                response.Status = true;
                response.Data = data.Id;
            }
            else
            {
                response.Status = false;
                response.Message = "Invalid data";
            }

            return response;
        }

        public async Task<ResponseObject> RemoveCartItem(int cartID)
        {
            if (cartID != 0 && dbContext.Carts.Count(m => m.Id == cartID) == 1)
            {
                var cart = await dbContext.Carts.FirstAsync(m => m.Id == cartID);
                cart.IsInCart = false;

                dbContext.Carts.Update(cart);
                await dbContext.SaveChangesAsync();

                response.Status = true;

            }
            else
            {
                response.Status = false;
                response.Message = "Invalid data";
            }

            return response;
        }
    }
}
