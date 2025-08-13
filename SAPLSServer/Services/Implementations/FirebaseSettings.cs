using SAPLSServer.Constants;
using SAPLSServer.Exceptions;
using SAPLSServer.Services.Interfaces;

namespace SAPLSServer.Services.Implementations
{
    public class FirebaseSettings : IFirebaseSettings
    {
        private readonly IConfiguration _configuration;

        public FirebaseSettings(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string ProjectId => _configuration[ConfigurationConstants.FirebaseProjectId] 
            ?? throw new EmptyConfigurationValueException(ConfigurationConstants.FirebaseProjectId);

        public string ServiceAccountKeyPath => _configuration[ConfigurationConstants.FirebaseServiceAccountKeyPath] 
            ?? throw new EmptyConfigurationValueException(ConfigurationConstants.FirebaseServiceAccountKeyPath);

        public string DefaultSound => _configuration[ConfigurationConstants.FirebaseDefaultSound] ?? "default";

        public string DefaultIcon => _configuration[ConfigurationConstants.FirebaseDefaultIcon] ?? "ic_notification";
    }
}