using SAPLSServer.Extensions;

namespace SAPLSServer
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add Azure Key Vault if needed
            //builder.AddAzureKeyVault();
            //TODO: Uncomment the above line if you want to use Azure Key Vault for configuration.
            // Configure services
            builder.Services.AddControllers();
            builder.Services.AddApplicationServices();
            builder.Services.AddExternalServices();
            builder.Services.AddApiDocumentation();
            builder.Services.AddCorsConfiguration();
            builder.Services.AddJwtAuthentication(builder.Configuration);

            var app = builder.Build();

            // Configure pipeline
            app.ConfigurePipeline();
            app.Run();
        }
    }
}