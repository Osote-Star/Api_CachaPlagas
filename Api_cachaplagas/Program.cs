
using Data;
using Data.Interfaces;
using Data.Services;
using MongoDB.Driver;

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

            MongoConfiguration mongoConfiguration = new (new MongoClient(Environment.GetEnvironmentVariable("CONNECTION_STRING") ?? ""));
            builder.Services.AddSingleton(mongoConfiguration);
            builder.Services.AddScoped<ITrampaServices, TrampaServices>();
            builder.Services.AddScoped<IUsuarioService, UsuariosServices>();
            builder.Services.AddScoped<ICapturaService, CapturaServices>();
            builder.Services.AddScoped<IservicioEmail, ServiceEMail>();
            builder.Services.AddScoped<IAuthServices, AuthServices>();

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
        }
    }
}
