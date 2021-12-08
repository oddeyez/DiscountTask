using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using DiscountCodeAPI.Data;
using DiscountCodeAPI.DataAccess;
using DiscountCodeAPI.Controllers;
using DiscountCodeAPI.Services;

namespace DiscountCodeAPI
{

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.Configure<DBSettings>(options =>
            {
                options.Connection = Configuration.GetSection("TestDatabase:ConnectionString").Value;
                options.DatabaseName = Configuration.GetSection("TestDatabase:DatabaseName").Value;
            });
            services.AddScoped<IMongoDBContext, MongoDBContext>();
            services.AddScoped<IDiscountService, DiscountService>();
            services.AddScoped<ICodeGenerator, CodeGenerator>();
            services.AddScoped<INotificationService, EmailNotificationService>();

            MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<FixedAmountDiscount>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
