using FruiteShop.Abstraction.Interfaces;
using FruiteShop.Abstraction.Models;
using FruiteShop.Abstraction.Models.Common;
using FruiteShop.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("http://localhost:4200")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials(); // Allow credentials if needed
        });
});

builder.Services.AddSignalR();

builder.Services.AddGraphQLServer().AddQueryType<GenerictGraphQL>().AddProjections().AddFiltering().AddSorting();

builder.Services.AddDbContext<FruiteContext>(option =>
                        option.UseSqlServer(builder.Configuration.GetConnectionString("fruiteContext")
                        ,b=>b.MigrationsAssembly("FruiteShop.WebApi")));

builder.Services.AddAuthentication(opt => {
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration.GetValue<string>("TokenIssuer:Issue"),
            ValidAudience = builder.Configuration.GetValue<string>("TokenIssuer:audience"),
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("TokenKey")))
        };
    });
//Adding Jwt Authorization Services
builder.Services.AddAuthorization();

builder.Services.AddScoped<IUser, UserService>();
builder.Services.AddScoped<IFruite, FruiteService>();
builder.Services.AddScoped<ICart, CartService>();
builder.Services.AddScoped<IPurchase, PurchaseService>();
builder.Services.AddScoped<GenerictGraphQL>();
builder.Services.AddSingleton<HubService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowSpecificOrigin");
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<ChatHub>("/chathub");
    endpoints.MapControllers();
});

app.MapGraphQL("/graphql");

app.MapControllers();

app.Run();
