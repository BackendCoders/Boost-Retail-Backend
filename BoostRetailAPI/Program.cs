
using Boost.Admin.Configuration;
using Boost.Admin.Data;
using Boost.Admin.Data.Models;
using Boost.Admin.Logic;
using Boost.Admin.Logic.Implementiation;
using Boost.Retail.Data;
using Boost.Retail.Data.Models;
using Boost.Retail.Services;
using Boost.Retail.Services.Interfaces;
using Boost.Shared;
using BoostRetailLib.Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using System.Text;



namespace BoostRetailAPI
{
    public class Program
    {
        private static IConfiguration Config { get; set; }

        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
               .AddJsonFile(@"appsettings.json", optional: false)
               .Build();

            Config = config;


            var builder = WebApplication.CreateBuilder(args);

            var logDateFolder = Path.Combine("Logs", DateTime.Now.ToString("yyyy-MM-dd"));
            if (!Directory.Exists(logDateFolder))
            {
                Directory.CreateDirectory(logDateFolder);
            }

            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Fatal)
            .MinimumLevel.Override("System", LogEventLevel.Fatal)
            .WriteTo.File(Path.Combine(logDateFolder, "info.txt"), restrictedToMinimumLevel: LogEventLevel.Information, rollingInterval: RollingInterval.Infinite, shared: true)
            .WriteTo.File(Path.Combine(logDateFolder, "warnings.txt"), restrictedToMinimumLevel: LogEventLevel.Warning, rollingInterval: RollingInterval.Infinite, shared: true)
            .WriteTo.File(Path.Combine(logDateFolder, "errors.txt"), restrictedToMinimumLevel: LogEventLevel.Error, rollingInterval: RollingInterval.Infinite, shared: true)
            .WriteTo.File(Path.Combine(logDateFolder, "fatal.txt"), restrictedToMinimumLevel: LogEventLevel.Fatal, rollingInterval: RollingInterval.Infinite, shared: true)
            .CreateLogger();

            builder.Host.UseSerilog();


            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    policy => policy.AllowAnyOrigin()
                                    .AllowAnyMethod()
                                    .AllowAnyHeader()
                                    .WithOrigins("http://localhost:3000", "http://localhost:5173", "http://localhost:5174")
                                    .AllowCredentials());
            });


            // Add services to the container.

            builder.Services.AddControllers();



            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Boost Retail API", Version = "v1" });
             
               

                // Add JWT Authentication
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your valid token."
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


            // Add Database Context
            builder.Services.AddDbContext<SimDbContext>(options =>
                options.UseSqlServer(Config.GetConnectionString(name: @"DefaultConnection")));


            //For Identity
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(o =>
            {
                o.SignIn.RequireConfirmedEmail = true;
                o.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
            })
            .AddEntityFrameworkStores<SimDbContext>()
            .AddDefaultTokenProviders();

            // Adding Authentication
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            // Adding Jwt Bearer
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtConfig:Secret"])),
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                };
            })
            .AddCookie();



            builder.Services.AddScoped<ITenantDbContextFactory, TenantDbContextFactory>();
            builder.Services.AddHttpContextAccessor(); // Needed for token extraction

            builder.Services.AddScoped<IImportLogic, ImportLogic>();
            builder.Services.AddScoped<ICategoryLogic, CategoryLogic>();

            builder.Services.AddScoped<IEmailSender, EmailSender>();
            builder.Services.AddTransient<ISMSSender, SMSSender>();

            builder.Services.AddScoped(typeof(IGenericService<,>), typeof(GenericService<,>));

            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IInventoryService, InventoryService>();
            builder.Services.AddScoped<ISaleService, SaleService>();
            builder.Services.AddScoped<ITillService, TillService>();
            builder.Services.AddScoped<ILayawayService, LayawayService>();

            builder.Services.AddAutoMapper(typeof(MappingProfile));
            builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

           

            var app = builder.Build();
            app.UseCors("AllowAll");


            // Configure the HTTP request pipeline.
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Boost Retail API v1");
            });

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();

        }
    }
}
