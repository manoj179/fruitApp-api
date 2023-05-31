using FruiteShop.Abstraction.Interfaces;
using FruiteShop.Abstraction.Models;
using FruiteShop.Service;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<FruiteContext>(option =>
                        option.UseSqlServer(builder.Configuration.GetConnectionString("fruiteContext")
                        ,b=>b.MigrationsAssembly("FruiteShop.WebApi")));

builder.Services.AddScoped<IUser, UserService>();
builder.Services.AddScoped<IFruite, FruiteService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
