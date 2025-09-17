
using BoostRetail.Integrations.SConnect.Data;
using BoostRetail.Integrations.SConnect.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SConnect.Middleware;
using System;
using System.Globalization;

namespace SConnect
{
    public class Program
    {
        private static IConfiguration Config { get; set; }
        public static void Main(string[] args)
        {
            var cultureInfo = new CultureInfo("en-GB");
            cultureInfo.NumberFormat.CurrencySymbol = "£";

            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            var config = new ConfigurationBuilder()
               .AddJsonFile(@"appsettings.json", optional: false, reloadOnChange: true)
               .Build();

            Config = config;

            var builder = WebApplication.CreateBuilder(args);

            //builder.Services.AddDbContext<SConnectDbContext>(options =>
            //options.UseSqlServer(config.GetConnectionString(name: @"DefaultConnection")));
            
            builder.Services.AddTransient(typeof(SConnectDbContext)); builder.Services.AddDbContext<SConnectDbContext>(options =>
               options.UseSqlServer(config.GetConnectionString(name: @"DefaultConnection")));

            builder.Services.AddDbContextFactory<SConnectDbContext>(options =>
               options.UseSqlServer(Config.GetConnectionString(name: @"DefaultConnection"),
               sqlServerOptionsAction: sqlOptions =>
               {
                   sqlOptions.EnableRetryOnFailure(
                       maxRetryCount: 5,
                       maxRetryDelay: TimeSpan.FromSeconds(2),
                       errorNumbersToAdd: null);
               }), ServiceLifetime.Scoped);


            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            //builder.Services.AddScoped<SConnectDbContext>();

            builder.Services.AddScoped<InventoryService>();
            builder.Services.AddScoped<LocationService>();
            builder.Services.AddScoped<TransactionsService>();




            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseMiddleware<TokenAuthMiddleware>();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseDeveloperExceptionPage(); // Shows full error in browser

            app.MapControllers();

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<SConnectDbContext>();

                // Optional: Check that the database is reachable (won’t modify schema)
                if (!dbContext.Database.CanConnect())
                {
                    throw new Exception("Cannot connect to the database.");
                }
            }

            app.Run();
        }
    }
}
