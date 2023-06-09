using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using FruiteShop.Abstraction.Interfaces;
using FruiteShop.Abstraction.Models;
using FruiteShop.Abstraction.Models.ApiModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
        private readonly string blobConnectionString;
        
        public FruiteService(FruiteContext dbContext,IConfiguration configuration)
        {
            this.dbContext = dbContext;
            response= new ResponseObject();
            blobConnectionString = configuration.GetConnectionString("AzureBlobConnetionString");
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
            data.ImgUrl = await UploadImageToAzureBlob(data.ImgFile);
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

        private async Task<string> UploadImageToAzureBlob(IFormFile file)
        {
            string blobUrl = string.Empty;

            if (file != null)
            {
                var blobContainer = new BlobContainerClient(blobConnectionString, "fruitsimage");
                var containerResponseInfo = await blobContainer.CreateIfNotExistsAsync();
                if (containerResponseInfo!=null && containerResponseInfo.GetRawResponse().Status == 201)
                    await blobContainer.SetAccessPolicyAsync(Azure.Storage.Blobs.Models.PublicAccessType.Blob);

                var blobClient = blobContainer.GetBlobClient(file.FileName+Guid.NewGuid());
                using(var fileStream = file.OpenReadStream())
                {
                    await blobClient.UploadAsync(fileStream, new BlobHttpHeaders { ContentType = file.ContentType});
                }

                blobUrl = blobClient.Uri.ToString();
            }

            return blobUrl;
        }
    }
}
