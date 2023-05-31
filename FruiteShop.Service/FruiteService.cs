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
    public class FruiteService : IFruite
    {
        private readonly FruiteContext dbContext;

        public ResponseObject response;

        public FruiteService(FruiteContext dbContext)
        {
            this.dbContext = dbContext;
            response= new ResponseObject();
        }

        public async Task<ResponseObject> GetFruitesList()
        {
            response.Status = true;
            response.Data = await dbContext.Fruites.ToListAsync();

            return response;
        }

        public async Task<ResponseObject> GetFruiteById(int id)
        {
            if(await dbContext.Fruites.CountAsync(m=>m.Id == id)==0)
            {
                response.Status = false;
                response.Message = "No found data";
            }
            else
            {
                response.Status = true;
                response.Data = await dbContext.Fruites.FirstOrDefaultAsync(m=>m.Id == id);
            }
            return response;
        }

        public async Task<ResponseObject> AddFruite(Fruite data)
        {
            dbContext.Fruites.Add(data);
            await dbContext.SaveChangesAsync();

            response.Status = true;
            response.Data = data.Id;
            return response;
        }

        public async Task<ResponseObject> UpdateFruite(Fruite data)
        {
            if(data==null || data.Id == 0)
            {
                response.Status = false;
                response.Message = "Invalid data";
            }
            else
            {
                dbContext.Fruites.Update(data);
                await dbContext.SaveChangesAsync();

                response.Status = true;
                response.Data = data.Id;
            }
            return response;
        }
    }
}
