using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
var mongoConnectionString = Environment.GetEnvironmentVariable("MONGO_CONNECTION") ?? "mongodb://localhost:27017";
builder.Services.AddSingleton<IMongoClient>(new MongoClient(mongoConnectionString));
builder.Services.AddSingleton<IMongoDatabase>(provider => 
    provider.GetService<IMongoClient>().GetDatabase("ecommerce"));

var app = builder.Build();
app.UseRouting();
app.MapControllers();
app.Run("http://0.0.0.0:5002");