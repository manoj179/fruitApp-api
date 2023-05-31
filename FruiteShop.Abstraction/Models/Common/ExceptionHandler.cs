using FruiteShop.Abstraction.Models.ApiModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FruiteShop.Abstraction.Models.Common
{
    public class ExceptionHandler
    {
        public ResponseObject GetExceptionMessage(Exception exception)
        {
            var response = new ResponseObject();
            response.Status = false;

            if (exception.InnerException != null)
            {
                if (exception.InnerException.InnerException != null)
                {
                    response.Exception = exception.InnerException.InnerException.Message;
                }
                else
                {
                    response.Exception = exception.InnerException.Message;
                }
            }
            else
            {
                response.Exception = exception.Message;
            }

            return response;
        }
    }
}
