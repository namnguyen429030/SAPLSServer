using SAPLSServer.Exceptions;
using SAPLSServer.Services.Interfaces;

namespace SAPLSServer.Services.Implementations
{
    public class GoogleOAuthSettings : IGoogleOAuthSettings
    {
        private readonly string _clientId;
        public GoogleOAuthSettings(IConfiguration configuration)
        {
            _clientId = configuration["GoogleOAuth:ClientId"] ?? throw new EmptyConfigurationValueException();
        }

        public string ClientId => _clientId;

    }
}
