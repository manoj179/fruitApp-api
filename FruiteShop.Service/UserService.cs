using FruiteShop.Abstraction.Interfaces;
using FruiteShop.Abstraction.Models;
using FruiteShop.Abstraction.Models.ApiModels;
using FruiteShop.Abstraction.Models.Common;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FruiteShop.Service
{
    public class UserService : IUser
    {
        private readonly FruiteContext dbContext;

        private readonly static double TokenExpiryInMin = 1;
        private readonly static double RefreshTokenExpiryInMin = 2;

        private ResponseObject response = new ResponseObject();
        private readonly IConfiguration configService;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly HubService _hubService;

        public UserService(FruiteContext dbContext,IConfiguration configService, IHubContext<ChatHub> hubContext, HubService hubService)
        {
            this.dbContext = dbContext;
            this.configService = configService;
            this._hubContext = hubContext;
            _hubService = hubService;
        }

        public async Task<AuthenticateResponse> Login(LoginObject data)
        {
            var response = new AuthenticateResponse();
            var userData = await dbContext.Users.Where(m => m.Email == data.EmailAddress && m.Password == data.Password).AsNoTracking().FirstOrDefaultAsync();

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
                    response.Data = userData;
                    response.Data.Password = null;
                    response.Token = GenerateJwtToken();
                    response.RefreshToken = GenerateRefreshToken();

                    var userDataToUpdate = await dbContext.Users.FirstAsync(m => m.Email == data.EmailAddress && m.Password == data.Password);

                    userDataToUpdate.RefreshToken = response.RefreshToken;
                    userDataToUpdate.RefreshTokenExpiry = DateTime.Now.AddMinutes(RefreshTokenExpiryInMin);

                    dbContext.Users.Update(userDataToUpdate);
                    await dbContext.SaveChangesAsync();

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

                ChatHub hub = new ChatHub(_hubService,_hubContext);

                await hub.SendUserStatusChangeNotification(status,id);


                response.Status = true;
            }
            else
            {
                response.Status = false;
                response.Message = "Invalid user data";
            }

            return response;
        }
        
        private string GenerateJwtToken()
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configService.GetValue<string>("TokenKey")));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokeOptions = new JwtSecurityToken(
                issuer: configService.GetValue<string>("TokenIssuer:Issuer"),
                audience: configService.GetValue<string>("TokenIssuer:audience"),
                claims: new List<Claim>(),
                expires: DateTime.Now.AddMinutes(TokenExpiryInMin),
                signingCredentials: signinCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(tokeOptions);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        private async Task<bool> isValidRefreshToken(int id, string refreshToken)
        {
            return (await dbContext.Users.AnyAsync(m => m.Id == id && m.RefreshToken == refreshToken && m.RefreshTokenExpiry >= DateTime.Now));
        }

        public async Task<ResponseObject> getNewJwtToken(JwtRequest userDetails)
        {
            response = new ResponseObject();

            if (await isValidRefreshToken(userDetails.Id, userDetails.RefreshToken))
            {
                string jwtToken = GenerateJwtToken();
                string refreshToken = GenerateRefreshToken();

                User userData = await dbContext.Users.FirstAsync(m=>m.Id == userDetails.Id);

                userData.RefreshToken = refreshToken;
                userData.RefreshTokenExpiry = DateTime.Now.AddMinutes(RefreshTokenExpiryInMin);

                response.Data = new { token = jwtToken, refreshToken };
                response.Status = true;
                return response;
            }
            else
            {
                response.Status = false;
                response.Message = "Invalid Request OR Refresh Token had expired. Try relogging-in again";
            }

            return response;

        }
    }
}
