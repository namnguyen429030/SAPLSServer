using SAPLSServer.Services.Interfaces;
using SAPLSServer.Services.Implementations;
using Azure.Identity;

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
            //// Core Business Services
            //services.AddScoped<IParkingLotService, ParkingLotService>();
            //services.AddScoped<IParkingSessionService, ParkingSessionService>();
            //services.AddScoped<IPaymentSourceService, PaymentSourceService>();
            //services.AddScoped<IRequestService, RequestService>();
            //services.AddScoped<ISharedVehicleService, SharedVehicleService>();
            //services.AddScoped<IShiftDiaryService, ShiftDiaryService>();
            //services.AddScoped<IVehicleService, VehicleService>();
            //services.AddScoped<IWhitelistService, WhitelistService>();

            //// User Management Services
            //services.AddScoped<IUserService, UserService>();
            //services.AddScoped<IAdminProfileService, AdminProfileService>();
            //services.AddScoped<IClientProfileService, ClientProfileService>();
            //services.AddScoped<IParkingLotOwnerProfileService, ParkingLotOwnerProfileService>();
            //services.AddScoped<IStaffProfileService, StaffProfileService>();

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
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "SAPLS Server API",
                    Version = "v1",
                    Description = "Smart Automated Parking Lot System API"
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
    }
}


