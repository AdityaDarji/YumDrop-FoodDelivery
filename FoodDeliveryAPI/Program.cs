using FoodDeliveryAPI.Models;
using FoodDeliveryAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Load appsettings.json
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Load database settings from appsettings.json
var databaseSettings = builder.Configuration.GetSection("DatabaseSettings").Get<DatabaseSettings>();
builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettings"));

// Register MongoClient
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    return new MongoClient(builder.Configuration.GetConnectionString("MongoDb"));
});

// Register IMongoDatabase (Main Fix)
builder.Services.AddSingleton<IMongoDatabase>(sp =>
{
    var client = sp.GetRequiredService<IMongoClient>();
    return client.GetDatabase(databaseSettings?.DatabaseName ?? "FoodDeliveryDB");
});


var jwtSettings = builder.Configuration.GetSection("JwtSettings");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["SecretKey"] ?? ""))
        };
    });

// Add authorization middleware
builder.Services.AddAuthorization();


// Register Services (Now services will fetch collections)
builder.Services.AddSingleton<OrderServices>();
builder.Services.AddSingleton<UserServices>();
builder.Services.AddSingleton<MenuServices>();
builder.Services.AddSingleton<RestaurantServices>();

// Add CORS Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFlutterApp",
        builder => builder.WithOrigins("http://localhost:7217", "http://192.168.31.224:7217") // Adjust if needed
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowFlutterApp"); // Apply CORS

app.UseAuthentication(); // Enables authentication middleware
app.UseAuthorization(); // Enables authorization middleware

app.MapControllers();



app.Run();
