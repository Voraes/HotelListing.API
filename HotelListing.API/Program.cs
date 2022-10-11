using HotelListing.API.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Adding DbContext with EntityFramework
var connectionString = builder.Configuration.GetConnectionString("HotelListingDbConnectionString");
builder.Services.AddDbContext<HotelListingDbContext>(options => {
    options.UseSqlServer(connectionString);
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policyBuilder => policyBuilder.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
});

builder.Host.UseSerilog(((context, loggerConfiguration) =>
    loggerConfiguration.WriteTo.Console().ReadFrom.Configuration(context.Configuration)));

var app = builder.Build();
 
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();