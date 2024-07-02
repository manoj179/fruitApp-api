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
        Task<AuthenticateResponse> Login(LoginObject data);

        Task<ResponseObject> GetUserList();

        Task<ResponseObject> GetUserById(int userId);

        Task<ResponseObject> AddUser(User Data);

        Task<ResponseObject> UpdateUser(User User);

        Task<ResponseObject> UpdateStatus(int id, string status);

        Task<ResponseObject> getNewJwtToken(JwtRequest userDetails);
    }
}
