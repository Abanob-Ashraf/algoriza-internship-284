using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text.Json.Serialization;
using VezeetaApi.Domain;
using VezeetaApi.Domain.Services;
using VezeetaApi.Domain.Repositories;
using VezeetaApi.Infrastructure;
using VezeetaApi.Infrastructure.AutoMapperConfig;
using VezeetaApi.Infrastructure.Data;
using VezeetaApi.Infrastructure.RepoServices;
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
            //.AddJsonOptions(options =>
            //{
            //    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            //});
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<VezeetaDbContext>(options => 
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
                //options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT"));

            builder.Services.AddCors(corsOption =>
            corsOption.AddPolicy("MyPolicy", corsPolicyBuilder =>
            corsPolicyBuilder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()
            ));

            builder.Services.AddScoped<IInitializeDefaultData, InitializeDefaultDataRepository>();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IAuthSevice, AuthRepository>();

            builder.Services.AddScoped(typeof(IBaseService<>), typeof(BaseRepository<>));

            builder.Services.AddScoped<IAppointmentRepo, AppointmentRepoService>();

            builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<VezeetaDbContext>();
            builder.Services.AddAutoMapper(Assembly.GetAssembly(typeof(MapperProfile)));

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
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["JWT:ValidAudiance"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
                };
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors("MyPolicy");
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}