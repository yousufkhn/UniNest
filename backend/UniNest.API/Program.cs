using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using UniNest.Api.Services.Auth;
using UniNest.API.Configurations;
using UniNest.API.Data;
using UniNest.API.Models.User;
using UniNest.API.Services.Auth;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
        ));

builder.Services
    .AddIdentityCore<ApplicationUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// This gets the jwt configurations from appsettings into JwtSettings - making them easy to access
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("Jwt")
);

builder.Services.AddScoped<IJwtService,JwtService>();

// jwt config
var jwtSettings =
    builder.Configuration.GetSection("Jwt");

var key =
    Encoding.UTF8.GetBytes(jwtSettings["Key"]!);
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme =
            JwtBearerDefaults.AuthenticationScheme;

        options.DefaultChallengeScheme =
            JwtBearerDefaults.AuthenticationScheme;

        options.DefaultScheme =
            JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],

                IssuerSigningKey =
                    new SymmetricSecurityKey(key)
            };

    });


builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer",
        new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header
        });
    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
       [new OpenApiSecuritySchemeReference("bearer",document)] = [] 
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
