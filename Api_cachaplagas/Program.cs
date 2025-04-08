using Data;
using Data.Interfaces;
using Data.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using System.Text;

namespace Api_cachaplagas
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddSignalR();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            MongoConfiguration mongoConfiguration = new(new MongoClient(Environment.GetEnvironmentVariable("CONNECTION_STRING") ?? ""));
            builder.Services.AddSingleton(mongoConfiguration);
            builder.Services.AddScoped<ITrampaServices, TrampaServices>();
            builder.Services.AddScoped<IUsuarioService, UsuariosServices>();
            builder.Services.AddScoped<IAuthServices, AuthServices>();
            builder.Services.AddScoped<ICapturaService,CapturaServices>();
            builder.Services.AddScoped<IservicioEmail, ServiceEMail>();


            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy.WithOrigins("http://localhost:5086", "https://xcdrzvgc-5086.usw3.devtunnels.ms")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
                });
            });

            // Configuraci�n de autenticaci�n JWT
            builder.Services.AddAuthentication(config =>
            {
                config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer("TokenUsuario",config =>
            {
                config.RequireHttpsMetadata = false;
                config.SaveToken = true;
                config.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_KEY") ?? "")),
                    ValidateLifetime = true
                };
            }).AddJwtBearer("TokenTrampa", config =>
            {
                config.RequireHttpsMetadata = false;
                config.SaveToken = true;
                config.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_KEY_IOT") ?? "")),
                    ValidateLifetime = true
                };
            }); 

            builder.Services.AddAuthorization();

            // Configuraci�n de Swagger con autenticaci�n JWT
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Mi API", Version = "v1" });

                // Definir el esquema de seguridad para Swagger
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Ingrese el token en el formato: Bearer {token}"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
            });

            
            builder.Services.AddControllers();

            var app = builder.Build();

            app.UseCors("AllowFrontend"); // Aplicar la pol�tica CORS
            app.UseAuthentication(); // Habilitar autenticaci�n
            app.UseAuthorization();  // Habilitar autorizaci�n

            // Habilitar Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mi API V1");
                c.RoutePrefix = "swagger"; // Ahora Swagger estar� en /swagger en lugar de /
            });

            app.MapControllers();
            app.MapHub<TrampaHub>("/signalrHub");

            app.Run();

        }
    }

}