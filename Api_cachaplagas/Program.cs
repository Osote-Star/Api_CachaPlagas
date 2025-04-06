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
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            MongoConfiguration mongoConfiguration = new(new MongoClient(Environment.GetEnvironmentVariable("CONNECTION_STRING") ?? ""));
            builder.Services.AddSingleton(mongoConfiguration);
            builder.Services.AddScoped<ITrampaServices, TrampaServices>();
            builder.Services.AddScoped<IUsuarioService, UsuariosServices>();
            builder.Services.AddScoped<IAuthServices, AuthServices>();
<<<<<<< HEAD
=======
            builder.Services.AddScoped<ICapturaService,CapturaServices>();
            builder.Services.AddScoped<IservicioEmail, ServiceEMail>();
>>>>>>> 573fb9a81d4650cda782779fdef313da5880ada6


            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
<<<<<<< HEAD
                    policy.WithOrigins("http://localhost:4200")
=======
                    policy.WithOrigins()
>>>>>>> 573fb9a81d4650cda782779fdef313da5880ada6
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });

<<<<<<< HEAD
            // Configuración de autenticación JWT
=======
            // Configuraciï¿½n de autenticaciï¿½n JWT
>>>>>>> 573fb9a81d4650cda782779fdef313da5880ada6
            builder.Services.AddAuthentication(config =>
            {
                config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
<<<<<<< HEAD
            }).AddJwtBearer(config =>
=======
            }).AddJwtBearer("TokenUsuario",config =>
>>>>>>> 573fb9a81d4650cda782779fdef313da5880ada6
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
<<<<<<< HEAD
            });

            builder.Services.AddAuthorization();

            // Configuración de Swagger con autenticación JWT
=======
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

            // Configuraciï¿½n de Swagger con autenticaciï¿½n JWT
>>>>>>> 573fb9a81d4650cda782779fdef313da5880ada6
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

<<<<<<< HEAD
            app.UseAuthentication(); // Habilitar autenticación
            app.UseAuthorization();  // Habilitar autorización
=======
            app.UseAuthentication(); // Habilitar autenticaciï¿½n
            app.UseAuthorization();  // Habilitar autorizaciï¿½n
>>>>>>> 573fb9a81d4650cda782779fdef313da5880ada6

            // Habilitar Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mi API V1");
<<<<<<< HEAD
                c.RoutePrefix = "swagger"; // Ahora Swagger estará en /swagger en lugar de /
=======
                c.RoutePrefix = "swagger"; // Ahora Swagger estarï¿½ en /swagger en lugar de /
>>>>>>> 573fb9a81d4650cda782779fdef313da5880ada6
            });

            app.MapControllers();

            app.Run();

        }
    }

}