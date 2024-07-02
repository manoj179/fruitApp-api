using FruiteShop.Abstraction.Interfaces;
using FruiteShop.Abstraction.Models;
using FruiteShop.Abstraction.Models.Common;
using FruiteShop.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FruiteShop.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICart cartService;

        public CartController(ICart cartService)
        {
            this.cartService = cartService;
        }

        [HttpGet("GetCartById/{id}")]
        public async Task<IActionResult> GetCartById(int id)
        {
            try
            {
                return Ok(await cartService.GetCartListByID(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ExceptionHandler().GetExceptionMessage(ex));
            }
        }

        [HttpGet("GetCartByUserId/{userId}")]
        public async Task<IActionResult> GetCartByUserId(int userId)
        {
            try
            {
                return Ok(await cartService.GetCartListByUserID(userId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ExceptionHandler().GetExceptionMessage(ex));
            }
        }

        [HttpPost()]
        public async Task<IActionResult> AddCart(Cart data)
        {
            try
            {
                return Ok(await cartService.AddCart(data));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ExceptionHandler().GetExceptionMessage(ex));
            }
        }

        [HttpDelete()]
        public async Task<IActionResult> RemoveCart(int id)
        {
            try
            {
                return Ok(await cartService.RemoveCartItem(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ExceptionHandler().GetExceptionMessage(ex));
            }
        }
    }
}
