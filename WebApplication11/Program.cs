using application;
using application.Service;
using application.Service.Interfaces;
using Application.Services;
using Domain.Interfaces;
using Infrastructure;
using Infrastructure.DB;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Npgsql;
//using System;
//using WebApplication11.DB;
//using WebApplication11.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

var dataSourceBuilder = new NpgsqlDataSourceBuilder(
    builder.Configuration.GetConnectionString("DefaultConnection")
);

var dataSource = dataSourceBuilder.Build();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(dataSource)
           .UseSnakeCaseNamingConvention()
);


// Add services to the container.
builder.Services.AddControllers();



// Repositories
builder.Services.AddScoped<ILineRepository, LineRepository>();
builder.Services.AddScoped<INodeRepository, NodeRepository>();
builder.Services.AddScoped<IFloorRepository, FloorRepository>();
builder.Services.AddScoped<IVenueRepository, VenueRepository>();

// Services
builder.Services.AddScoped<ILineService, LineService>();
builder.Services.AddScoped<INodeService, NodeService>();
builder.Services.AddScoped<IFloorService, FloorService>();
builder.Services.AddScoped<IVenueService, VenueService>();



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
//builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
//builder.Services.AddScoped<ISoftDeleteService, SoftDeleteService>();



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
