using FruiteShop.Abstraction.Models;
using FruiteShop.Abstraction.Models.ApiModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FruiteShop.Abstraction.Interfaces
{
    public interface IFruite
    {
        Task<ResponseObject> GetFruitesList();

        Task<ResponseObject> GetFruiteById(int id);

        Task<ResponseObject> AddFruite(Fruite data);

        Task<ResponseObject> UpdateFruite(Fruite data);
    }
}
