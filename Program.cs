using POSWindowFormAPI.Data.Repositories;
using POSWindowFormAPI.Data.Repositories.Interfaces;
using POSWindowFormAPI.Services;
using POSWindowFormAPI.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IBookingTableService, BookingTableService>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IBookingTableRepository, BookingTableRepository>();
builder.Services.AddScoped<ITrackingRepository, TrackingRepository  >();
builder.Services.AddScoped<ISqlConnectionFactory, SqlConnectionFactory>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
