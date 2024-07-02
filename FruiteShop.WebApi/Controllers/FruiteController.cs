using FruiteShop.Abstraction.Interfaces;
using FruiteShop.Abstraction.Models;
using FruiteShop.Abstraction.Models.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FruiteShop.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FruiteController : ControllerBase
    {
        private readonly IFruite fruiteService;

        public FruiteController(IFruite fruiteService)
        {
            this.fruiteService = fruiteService;
        }

        [HttpGet]
        public async Task<IActionResult> GetFruitesList()
        {
            try
            {
                return Ok(await fruiteService.GetFruitesList());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ExceptionHandler().GetExceptionMessage(ex));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFruiteById(int id)
        {
            try
            {
                return Ok(await fruiteService.GetFruiteById(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ExceptionHandler().GetExceptionMessage(ex));
            }
        }

        [HttpPost()]
        public async Task<IActionResult> AddFruite([FromForm]Fruite data)
        {
            try
            {
                return Ok(await fruiteService.AddFruite(data));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ExceptionHandler().GetExceptionMessage(ex));
            }
        }

        [HttpPut()]
        public async Task<IActionResult> UpdateFruite(Fruite data)
        {
            try
            {
                return Ok(await fruiteService.UpdateFruite(data));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ExceptionHandler().GetExceptionMessage(ex));
            }
        }
    }
}
