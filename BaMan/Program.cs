using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using BaManPubSub.Core.Application;
using BaManPubSub.Core.Application.BackgroundServices;
using BaManPubSub.Core.Application.Infra;
using BaManPubSub.Core.Domain;
using BaManPubSub.Core.Infrastructure.DB;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();



var a = builder.Configuration.GetConnectionString("Conn");
builder.Services.AddDbContext<AppDbContext>(x => x.UseSqlServer(a));

builder.Services.AddScoped<ProductService>();

builder.Services.Configure<RabbitSetting>(builder.Configuration.GetSection("RabbitMQ"));
builder.Services.AddScoped(typeof(IMessagePublisher<>), typeof(RabbitPublisher<>));

builder.Services.AddHostedService<NewProductConsumerBackgroundService>();

builder.Services.AddScoped<IAppSubscriber<Product>, ProductSubscriber>();

var app = builder.Build();


app.MapScalarApiReference(); // scalar/v1
app.MapOpenApi();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
        

