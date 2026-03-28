
using Core.Interfaces;
using Core.Settings;
using Infrastructure.UnitOfWork;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using ProjectApi.Factory;
using ProjectApi.FactoryImplementation;
using Core.Models;
using Microsoft.AspNetCore.Identity;
using Core.Servises;
using Infrastructure.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using ProjectApi.Servises;

namespace ProjectApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddDbContext<AppDBContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("ProdcutionConnection"),
                    options => options.MigrationsAssembly(typeof(AppDBContext).Assembly.FullName));
            });
            builder.Services.Configure<JWTSettings>(builder.Configuration.GetSection("jwt"));

            builder.Services.AddTransient(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));

            builder.Services.AddTransient(typeof(IPropertyFactory), typeof(PropertyFactory));
            builder.Services.AddTransient<Floor>();
            builder.Services.AddTransient<Apartment>();
            builder.Services.AddTransient<Villa>();
           


            builder.Services.AddIdentity<AppUser, IdentityRole>().
                AddEntityFrameworkStores<AppDBContext>();


            builder.Services.AddScoped<PasswordHasher<AppUser>>();
            builder.Services.AddTransient<ImageService>();
            builder.Services.AddTransient<PropertyServices>();
            builder.Services.AddTransient<SearchService>();

            builder.Services.AddTransient<IAuthentication, Authentication>();
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["jwt:Key"]))

                };

            });

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddCors();
            builder.Services.AddSwaggerGen(options =>
            {
                


                options.AddSecurityDefinition(name: "Bearer", securityScheme: new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,

                });



                options.AddSecurityRequirement(securityRequirement: new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()

                    }

                });

            });

            var app = builder.Build();
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseHttpsRedirection();
            app.UseCors(c => c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
