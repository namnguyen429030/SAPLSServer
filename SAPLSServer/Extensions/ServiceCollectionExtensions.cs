using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SAPLSServer.Services.Interfaces;
using SAPLSServer.Services.Implementations;
using Azure.Identity;
using SAPLSServer.Repositories.Interfaces;
using SAPLSServer.Repositories.Implementations;
using SAPLSServer.Models;
using Microsoft.EntityFrameworkCore;

namespace SAPLSServer.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Configure Azure Key Vault integration
        /// </summary>
        /// <param name="builder">Web application builder</param>
        /// <returns>Web application builder for chaining</returns>
        public static WebApplicationBuilder AddAzureKeyVault(this WebApplicationBuilder builder)
        {
            // Get Key Vault configuration from appsettings
            var keyVaultUrl = builder.Configuration["AzureKeyVault:VaultUrl"];

            if (!string.IsNullOrEmpty(keyVaultUrl))
            {
                builder.Configuration.AddAzureKeyVault(
                    new Uri(keyVaultUrl),
                    new DefaultAzureCredential(new DefaultAzureCredentialOptions
                    {
                        // For local development, you might want to specify the tenant
                        TenantId = builder.Configuration["AzureKeyVault:TenantId"]
                    })
                );
            }

            return builder;
        }
        /// <summary>
        /// Register all application services
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <returns>Service collection for chaining</returns>
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddDbContext<SaplsContext>(opt =>
            {
                opt.UseSqlServer(Environment.GetEnvironmentVariable("DefaultConnection"));
            });
            //Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAdminProfileRepository, AdminProfileRepository>();
            services.AddScoped<IClientProfileRepository, ClientProfileRepository>();
            services.AddScoped<IParkingLotOwnerProfileRepository, ParkingLotOwnerProfileRepository>();
            services.AddScoped<IStaffProfileRepository, StaffProfileRepository>();
            services.AddScoped<IParkingSessionRepository, ParkingSessionRepository>();
            services.AddScoped<IVehicleRepository, VehicleRepository>();
            services.AddScoped<ISharedVehicleRepository, SharedVehicleRepository>();
            services.AddScoped<IPaymentSourceRepository, PaymentSourceRepository>();
            services.AddScoped<IParkingLotRepository, ParkingLotRepository>();

            services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
            //Services
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<IParkingLotOwnerService, ParkingLotOwnerService>();
            services.AddScoped<IStaffService, StaffService>();
            services.AddScoped<IParkingSessionService, ParkingSessionService>();
            services.AddScoped<IVehicleService, VehicleService>();
            services.AddScoped<ISharedVehicleService, SharedVehicleService>();
            services.AddScoped<ISubscriptionService, SubscriptionService>();




            services.AddScoped<IPaymentSourceService, PaymentSourceService>();
            services.AddScoped<IParkingLotService, ParkingLotService>();
            return services;
        }

        /// <summary>
        /// Register external API services
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <returns>Service collection for chaining</returns>
        public static IServiceCollection AddExternalServices(this IServiceCollection services)
        {
            // HTTP Client for external API calls
            services.AddHttpClient();

            // AI/OCR Services
            services.AddScoped<IGeminiOcrService, GeminiOcrService>();

            return services;
        }

        /// <summary>
        /// Register API documentation services
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <returns>Service collection for chaining</returns>
        public static IServiceCollection AddApiDocumentation(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddControllers().AddJsonOptions(opts =>
            {
                opts.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.KebabCaseLower;
            });
            services.AddRouting(opts =>
            {
                opts.LowercaseUrls = true;
            });
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "SAPLS Server API",
                    Version = "v1",
                    Description = "Smart Automated Parking Lot System API"
                });

                // ADD JWT SUPPORT TO SWAGGER
                options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference
                            {
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });

                // Include XML documentation if available
                var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    options.IncludeXmlComments(xmlPath);
                }
            });

            return services;
        }

        /// <summary>
        /// Register CORS policies
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <returns>Service collection for chaining</returns>
        public static IServiceCollection AddCorsConfiguration(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("DefaultPolicy", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });

                options.AddPolicy("ProductionPolicy", policy =>
                {
                    policy.WithOrigins("https://yourdomain.com", "https://www.yourdomain.com")
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials();
                });
            });

            return services;
        }

        /// <summary>
        /// Add JWT authentication to the service collection
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <param name="configuration">Application configuration</param>
        /// <returns>Service collection for chaining</returns>
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JwtSettings");
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSettings["SecretKey"] ?? "DefaultSecretKey"))
                };
            });

            return services;
        }
    }
}


