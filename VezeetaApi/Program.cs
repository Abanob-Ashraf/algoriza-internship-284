using Microsoft.EntityFrameworkCore;
using System.Reflection;
using VezeetaApi.Domain;
using VezeetaApi.Domain.Services;
using VezeetaApi.Infrastructure;
using VezeetaApi.Infrastructure.AutoMapperConfig;
using VezeetaApi.Infrastructure.Data;
using VezeetaApi.Infrastructure.Repositories;
using VezeetaApi.Domain.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace VezeetaApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT"));
            builder.Services.Configure<Mail>(builder.Configuration.GetSection("Mail"));

            builder.Services.AddScoped<IAuthService, AuthRepository>();
            builder.Services.AddScoped<ISendingEmailService, SendingEmailService>();

            builder.Services.AddScoped<IInitializeDefaultData, InitializeDefaultDataRepository>();
            builder.Services.AddHostedService<InitializeDefaultDataService>();

            builder.Services.AddAutoMapper(Assembly.GetAssembly(typeof(MapperProfile)));

            builder.Services.AddDbContext<VezeetaDbContext>(options => 
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
            );

            builder.Services.AddIdentity<AppUser, IdentityRole>(option =>
            {
                option.Password.RequiredLength = 8;
                option.Password.RequireDigit = false;
                option.Password.RequireUppercase = true;
                option.Password.RequireNonAlphanumeric = false;
            }).AddEntityFrameworkStores<VezeetaDbContext>();


            builder.Services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(option =>
            {
                option.SaveToken = true;
                option.RequireHttpsMetadata = false;
                option.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["JWT:ValidAudiance"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
                    ClockSkew = TimeSpan.Zero
                };
            });

            builder.Services.AddCors(corsOption =>
            corsOption.AddPolicy("MyPolicy", corsPolicyBuilder =>
            corsPolicyBuilder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()
            ));

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped(typeof(IBaseService<>), typeof(BaseRepository<>));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors("MyPolicy");
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}