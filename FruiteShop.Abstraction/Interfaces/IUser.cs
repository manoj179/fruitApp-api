using FruiteShop.Abstraction.Models;
using FruiteShop.Abstraction.Models.ApiModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FruiteShop.Abstraction.Interfaces
{
    public interface IUser
    {
        Task<ResponseObject> Login(string username, string password);

        Task<ResponseObject> GetUserList();

        Task<ResponseObject> GetUserById(int userId);

        Task<ResponseObject> AddUser(User Data);

        Task<ResponseObject> UpdateUser(User User);
    }
}
