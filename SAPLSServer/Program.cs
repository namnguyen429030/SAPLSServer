using SAPLSServer.Extensions;

namespace SAPLSServer
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configure services
            builder.Services.AddControllers();
            builder.Services.AddApplicationServices();
            builder.Services.AddExternalServices();
            builder.Services.AddApiDocumentation();
            builder.Services.AddCorsConfiguration();

            var app = builder.Build();

            // Configure pipeline
            app.ConfigurePipeline();

            app.Run();
        }
    }
}