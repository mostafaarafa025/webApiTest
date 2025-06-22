
using Microsoft.EntityFrameworkCore;
using wabApi.MappinConfig;
using wabApi.Models;
using wabApi.Repositories;
using wabApi.UnitOfWorks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Text;
namespace wabApi
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
            builder.Services.AddDbContext<ItiDbContext>(options =>
            {
                options.UseLazyLoadingProxies();
                options.UseSqlServer(builder.Configuration.GetConnectionString("con1"));
            });
            builder.Services.AddAuthentication(op => op.DefaultAuthenticateScheme = "mySchema")
            .AddJwtBearer("mySchema", option =>
            {
                string key = "this_is_a_very_long_secret_key_456789";
                var scrtKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
                option.TokenValidationParameters = new TokenValidationParameters()
                    {
                        IssuerSigningKey=scrtKey,
                        ValidateIssuer=false,
                        ValidateAudience=false,
                    };
                });
            builder.Services.AddScoped<UnitOfWork,UnitOfWork>();
            builder.Services.AddAutoMapper(typeof(MapperConfig));
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
