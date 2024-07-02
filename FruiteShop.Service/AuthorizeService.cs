using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FruiteShop.Service
{
    //[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class AuthorizeService : AuthorizeAttribute, IAuthorizationFilter
    {
        //private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        public AuthorizeService()
        {
            _configuration = new ConfigurationBuilder()
                   .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                   .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                   .Build();
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var authHeader = context.HttpContext.Request.Headers["Authorization"].ToString();

            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
            {
                // Extract the token without the "Bearer " prefix
                var token = authHeader.Substring("Bearer ".Length).Trim();

                try
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("TokenKey")); // Replace with your secret key
                    var validationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };

                    SecurityToken securityToken;
                    var principal = tokenHandler.ValidateToken(token, validationParameters, out securityToken);

                    // Below logic is used When Invoke() method is used.
                    //var jwtSecurityToken = securityToken as JwtSecurityToken;

                    //if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //    context.Result = new UnauthorizedResult(); // Unauthorized
                    //}
                }
                catch (Exception)
                {
                    context.Result = new UnauthorizedResult();
                }
            }
            else
            {
                context.Result = new UnauthorizedResult();
            }
            return;
        }

        //public async Task Invoke(HttpContext context)
        //{
        //    // Retrieve the authorization header
        //    var authHeader = context.Request.Headers["Authorization"].ToString();

        //    // Check if the header is present and starts with "Bearer "
            
        //}
    }

    //public static class ValidateExpiredTokenMiddlewareExtensions
    //{
    //    public static IApplicationBuilder ValidateExpiredTokenMiddleware(this IApplicationBuilder builder)
    //    {
    //        return builder.UseMiddleware<AuthorizeService>();
    //    }
    //}
}
