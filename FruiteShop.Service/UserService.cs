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
    public class UserService : IUser
    {
        private readonly FruiteContext dbContext;

        private ResponseObject response = new ResponseObject();

       
        public UserService(FruiteContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<ResponseObject> Login(string username, string password)
        {
            var userData = await dbContext.Users.FirstOrDefaultAsync(m => m.Email == username && m.Password == password);

            if (userData == null)
            {
                response.Status = false;
                response.Message = "User Data not found";
            }
            else
            {
                if (!userData.IsActive)
                {
                    response.Status = false;
                    response.Message = "User is Inactive";
                }
                else
                {
                    userData.Password = null;
                    response.Data = userData;
                    response.Status = true;
                }
               
            }

            return response;
        }


        public async Task<ResponseObject> AddUser(User Data)
        {
            Data.CreatedOn = DateTime.Now;
            Data.IsActive = true;

            dbContext.Users.Add(Data);
            await dbContext.SaveChangesAsync();

            response.Status = true;
            response.Data = Data.Id;

            return response;
        }

        public async Task<ResponseObject> GetUserById(int userId)
        {
            
            var userData = await dbContext.Users.FirstOrDefaultAsync(m => m.Id == userId);

            if(userData == null)
            {
                response.Status = false;
                response.Message = "User Data not found";
            }
            else
            {
                response.Data = userData;
                response.Status = true;
            }

            return response;
        }

        public async Task<ResponseObject> GetUserList()
        {
            response.Status = true;

            var userData = await dbContext.Users.ToListAsync();
            userData.ForEach(m => m.Password = null);
            response.Data = userData;

            return response;
        }

        public async Task<ResponseObject> UpdateUser(User User)
        {
            if(User == null || User.Id == 0)
            {
                response.Status = false;
                response.Message = "Invalid data";
            }
            else
            {
                response.Status = true;
                dbContext.Users.Update(User);
                await dbContext.SaveChangesAsync();
            }

            return response;
        }

        public async Task<ResponseObject> UpdateStatus(int id, string status)
        {
            if (id != 0)
            {
                var user = dbContext.Users.First(m => m.Id == id);

                user.IsActive = status == "Active" ? true : false;

                dbContext.Users.Update(user);
                await dbContext.SaveChangesAsync();

                response.Status = true;
            }
            else
            {
                response.Status = false;
                response.Message = "Invalid user data";
            }

            return response;
        }
    }
}
