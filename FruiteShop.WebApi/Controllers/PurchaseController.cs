using FruiteShop.Abstraction.Interfaces;
using FruiteShop.Abstraction.Models.Common;
using FruiteShop.Abstraction.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FruiteShop.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseController : ControllerBase
    {
        private readonly IPurchase purchaseService;

        public PurchaseController(IPurchase purchaseService)
        {
            this.purchaseService = purchaseService;
        }

        [HttpGet("GetOrderId/{id}")]
        public async Task<IActionResult> GetOrderId(int id)
        {
            try
            {
                return Ok(await purchaseService.GetPurchaseByID(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ExceptionHandler().GetExceptionMessage(ex));
            }
        }

        [HttpGet("GetOrderByUserId/{userId}")]
        public async Task<IActionResult> GetOrderByUserId(int userId)
        {
            try
            {
                return Ok(await purchaseService.GetPurchaseListByUserID(userId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ExceptionHandler().GetExceptionMessage(ex));
            }
        }

        [HttpGet("addPurchase/{cartId}")]
        public async Task<IActionResult> AddOrder(int cartId)
        {
            try
            {
                return Ok(await purchaseService.AddPurchase(cartId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ExceptionHandler().GetExceptionMessage(ex));
            }
        }

        [HttpPut()]
        public async Task<IActionResult> UpdateOrderStatus(Purchase data)
        {
            try
            {
                return Ok(await purchaseService.ChangeOrderStatus(data));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ExceptionHandler().GetExceptionMessage(ex));
            }
        }
    }
}
