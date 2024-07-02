using FruiteShop.Abstraction.Models.ApiModels;
using FruiteShop.Abstraction.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FruiteShop.Abstraction.Interfaces;
using Microsoft.EntityFrameworkCore;
using FruiteShop.Abstraction.Models.Common;

namespace FruiteShop.Service
{
    public class PurchaseService : IPurchase
    {
        private readonly FruiteContext dbContext;

        private ResponseObject response;

        public PurchaseService(FruiteContext dbContext)
        {
            this.dbContext = dbContext;
            response = new ResponseObject();
        }

        public async Task<ResponseObject> GetPurchaseByID(int purchaseId)
        {
            if (purchaseId > 0 && dbContext.Purchases.Count(m => m.Id == purchaseId) >= 1)
            {
                response.Status = true;
                var purchase = await dbContext.Purchases.Include(m => m.Orders).ThenInclude(f => f.Fruite).FirstAsync(m => m.Id == purchaseId);
                purchase.UserName = dbContext.Users.First(m => m.Id == purchase.UserId).Email;
                response.Data = purchase;
            }
            else
            {
                response.Status = false;
                response.Message = "Data not found";
            }

            return response;
        }

        public async Task<ResponseObject> GetPurchaseListByUserID(int userId)
        {
            if (userId > 0 && dbContext.Purchases.Count(m => m.UserId == userId) >= 1)
            {
                var purchase = await dbContext.Purchases.Include(m => m.Orders).ThenInclude(f => f.Fruite).Where(m => m.UserId == userId).OrderByDescending(o => o.DeliveredDate).ToListAsync();
                response.Data = purchase;
                response.Status = true;
            }
            else
            {
                response.Status = true;
                var purchase = await dbContext.Purchases.Include(m => m.Orders).ThenInclude(f => f.Fruite).OrderByDescending(o => o.DeliveredDate).ToListAsync();
                purchase.ForEach(m => m.UserName = dbContext.Users.First(u => u.Id == m.UserId).Email);
                response.Data = purchase;
            }

            return response;
        }

        public async Task<ResponseObject> AddPurchase(int cartID)
        {

            if (cartID > 0 && dbContext.Carts.Count(m => m.Id == cartID) == 1)
            {
                using(var dbTrans = dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var cart = await dbContext.Carts.Include(m => m.Orders).FirstAsync(m => m.Id == cartID);
                        
                        var purchaseData = new Purchase();

                        purchaseData.UserId = cart.UserId;
                        purchaseData.TotalPrice = cart.TotalPrice;
                        purchaseData.Address = dbContext.Users.First(m=>m.Id == cart.UserId).Address;
                        purchaseData.PurchasedDate = DateTime.Now;
                        purchaseData.Status = "Order Placed";

                        purchaseData.Orders = new List<Orders>();

                        foreach(var order in cart.Orders)
                        {
                            if(dbContext.Fruites.First(m=>m.Id == order.FruiteId).Availibility >= order.Quantity)
                            {
                                purchaseData.Orders.Add(order);

                                var fruite = await dbContext.Fruites.FirstAsync(m => m.Id == order.FruiteId);

                                fruite.Availibility -= order.Quantity;
                                dbContext.Fruites.Update(fruite);
                            }
                        }
                        
                        dbContext.Purchases.Add(purchaseData);

                        cart.IsInCart = false;
                        dbContext.Carts.Update(cart);

                        await dbContext.SaveChangesAsync();

                        dbTrans.Commit();

                        response.Data = purchaseData.Id;
                    }
                    catch (Exception ex)
                    {
                        dbTrans.Rollback();
                        response = new ExceptionHandler().GetExceptionMessage(ex);
                    }
                }
            }
            else
            {
                response.Status = false;
                response.Message = "Invalid data";
            }

            return response;
        }

        public async Task<ResponseObject> ChangeOrderStatus(Purchase data)
        {
            if (data != null && dbContext.Purchases.Count(m => m.Id == data.Id) > 0)
            {
                var purchase = await dbContext.Purchases.FirstAsync(m => m.Id == data.Id);

                purchase.Status = data.Status;

                if(purchase.Status.ToLower().Contains("cancell") || purchase.Status.ToLower().Contains("cancelled"))
                {
                    purchase.StatusReason = "Order Is Cancelled On " + DateTime.Now.ToString("dd/MM/yyyy hh:MM");
                }
                else
                {
                    purchase.StatusReason = "Successfully Delivered";
                    purchase.DeliveredDate = DateTime.Now;
                }

                dbContext.Purchases.Update(purchase);
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
