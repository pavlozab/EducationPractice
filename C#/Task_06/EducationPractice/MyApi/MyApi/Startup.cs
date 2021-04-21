using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using AutoMapper;
using AutoWrapper;
using Config;
using Data;
using Entities;
using JwtAuth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MyApi.Profiles;
using Services;

namespace MyApi
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
            var dbConfig = Configuration.GetSection(nameof(DbConfig)).Get<DbConfig>();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(dbConfig.ConnectionString));
            
            // services.AddDbContextFactory<ApplicationDbContext>(
            //     options =>
            //         options.UseNpgsql(dbConfig.ConnectionString));
            
            services.AddIdentity<User, Role>(options => options.SignIn.RequireConfirmedAccount = true) 
                .AddEntityFrameworkStores<ApplicationDbContext>();
            
            // Redis
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = Configuration.GetConnectionString("Redis");
                options.InstanceName = "MyApi_";
            });

            // Mapping
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            services.AddSingleton(mappingConfig.CreateMapper());
            
            // var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            //     .UseNpgsql(dbConfig.ConnectionString)
            //     .Options;

            // services.AddSingleton<ApplicationDbContext>(ServiceProvider => 
            //     new ApplicationDbContext(contextOptions));

            // 
            services.AddSingleton<IAddressRepository, AddressRepository>();
            services.AddSingleton<IAddressService, AddressService>();
            
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            
            services.AddScoped<IJwtAuthManager, JwtAuthManager>();
            
            services.AddSingleton<IOrderRepository, OrderRepository>();
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
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtTokenConfig.SecretKey)),
                    ValidAudience = jwtTokenConfig.Audience,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(1)
                };
            });

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
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
                    In = ParameterLocation.Header, 
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey 
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    { 
                        new OpenApiSecurityScheme 
                        { 
                            Reference = new OpenApiReference 
                            { 
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer" 
                            } 
                        },
                        new string[] { } 
                    } 
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
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
            
            app.UseApiResponseAndExceptionWrapper(new AutoWrapperOptions { 
                UseApiProblemDetailsException = true 
            });

            app.UseHttpsRedirection();

            app.UseRouting();
            
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}