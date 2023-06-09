using FruiteShop.Abstraction.Interfaces;
using FruiteShop.Abstraction.Models;
using FruiteShop.Abstraction.Models.ApiModels;
using FruiteShop.Abstraction.Models.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FruiteShop.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUser userService;

        public UserController(IUser userService)
        {
            this.userService = userService;
        }

        [HttpGet()]
        public async Task<IActionResult> GetUsersList()
        {
            try
            {
                return Ok(await userService.GetUserList());
            }
            catch(Exception ex)
            {
                return StatusCode(500,new ExceptionHandler().GetExceptionMessage(ex));
            }
        }

        [HttpGet("add/{a}/{b}")]
        public IActionResult AddNumber(int a, int b)
        {
            try
            {
                return Ok(new { result = (a+b) });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ExceptionHandler().GetExceptionMessage(ex));
            }
        }

        [HttpGet("login/{username}/{password}")]
        public async Task<IActionResult> Login(string username,string password)
        {
            try
            {
                return Ok(await userService.Login(username,password));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ExceptionHandler().GetExceptionMessage(ex));
            }
        }

        [HttpGet("getUserById/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                return Ok(await userService.GetUserById(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ExceptionHandler().GetExceptionMessage(ex));
            }
        }

        [HttpPost()]
        public async Task<IActionResult> AddUser(User Data)
        {
            try
            {
                return Ok(await userService.AddUser(Data));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ExceptionHandler().GetExceptionMessage(ex));
            }
        }

        [HttpPut()]
        public async Task<IActionResult> UpdateUser(User Data)
        {
            try
            {
                return Ok(await userService.UpdateUser(Data));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ExceptionHandler().GetExceptionMessage(ex));
            }
        }


        [HttpGet("UpdateUserStatus/{id}/{status}")]
        public async Task<IActionResult> UpdateUserStatus(int id,string status)
        {
            try
            {
                return Ok(await userService.UpdateStatus(id,status));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ExceptionHandler().GetExceptionMessage(ex));
            }
        }
    }
}
