using System;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Text.Json;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using ProductRest.Config;
using ProductRest.Infrastructure;
using ProductRest.Infrastructure.Contracts;
using ProductRest.Profiles;
using ProductRest.Repositories;
using ProductRest.Repositories.Contracts;
using ProductRest.Services;
using ProductRest.Services.Contracts;

namespace ProductRest
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
            // Database
            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
            var mongoDbConfig = Configuration.GetSection(nameof(MongoDbConfig)).Get<MongoDbConfig>();
            
            services.AddSingleton<IMongoClient>(ServiceProvider =>
            {
                return new MongoClient(mongoDbConfig.ConnectionString);
            });
            
            // Mapping
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            services.AddSingleton(mappingConfig.CreateMapper());
            
            // 
            services.AddSingleton<IProductsRepository, MongoDbProductsRepository>();
            services.AddSingleton<IProductService, ProductService>();
            
            
            services.AddSingleton<IUserRepository, MongoDbUserRepository>(); 
            services.AddScoped<IAuthService, AuthService>();
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IJwtAuthManager, JwtAuthManager>();

            services.AddSingleton<IOrderRepository, MongoDbOrderRepository>();
            services.AddSingleton<IOrderService, OrderService>();
            
            // JWT
            var jwtTokenConfig = Configuration.GetSection(nameof(JwtTokenConfig)).Get<JwtTokenConfig>();
            services.AddSingleton(jwtTokenConfig);
            services.AddAuthentication(obj =>
            {
                obj.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                obj.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(obj =>
            {
                obj.RequireHttpsMetadata = true;
                obj.SaveToken = true;
                obj.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtTokenConfig.Issuer,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtTokenConfig.Secret)),
                    ValidAudience = jwtTokenConfig.Audience,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(1)
                };
            });
            
            // services.AddSingleton<JwtAuthManager>(ServiceProvider =>
            // {
            //     return new JwtAuthManager(jwtTokenConfig);
            // });
            
            // Controller
            services.AddControllers();

            // Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "ProductRest",
                });
                
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            
            // //Health check
            // services.AddHealthChecks()
            //     .AddMongoDb(
            //         mongoDbSettings.ConnectionString, 
            //         name: "mongodb", 
            //         timeout: TimeSpan.FromSeconds(3),
            //         tags: new []{ "ready" });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger(c =>
                {
                    c.SerializeAsV2 = true;
                });
                app.UseSwaggerUI(c => 
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProductRest v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                //     endpoints.MapHealthChecks("/health/ready", new HealthCheckOptions
                //     {
                //         Predicate = (check) => check.Tags.Contains("ready"),
                //         ResponseWriter = async(context, report) =>
                //         {
                //             var result = JsonSerializer.Serialize(
                //                 new
                //                 {
                //                     status = report.Status.ToString(),
                //                     checks = report.Entries.Select(entry => new
                //                     {
                //                         name = entry.Key,
                //                         status = entry.Value.Status.ToString(),
                //                         exception = entry.Value.Exception != null ? entry.Value.Exception.Message : "none",
                //                         duration = entry.Value.Duration.ToString()
                //                     })
                //                 });
                //     
                //             context.Response.ContentType = MediaTypeNames.Application.Json;
                //             await context.Response.WriteAsync(result);
                //         }
                //     });
                //     
                //     endpoints.MapHealthChecks("/health/live", new HealthCheckOptions
                //     {
                //         Predicate = (_) => false
                //     });
            });
        }
    }
}