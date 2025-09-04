using Business.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Business.Services
{
    public class ConfiguracionService : IConfiguracionService
    {
        private readonly IConfiguration _configuration;

        public ConfiguracionService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetTusFacturasUserToken()
        {
            return _configuration["TusFacturasAPP:UserToken"] ?? throw new InvalidOperationException("TusFacturasAPP:UserToken no está configurado");
        }

        public string GetTusFacturasApiKey()
        {
            return _configuration["TusFacturasAPP:ApiKey"] ?? throw new InvalidOperationException("TusFacturasAPP:ApiKey no está configurado");
        }

        public string GetTusFacturasApiToken()
        {
            return _configuration["TusFacturasAPP:ApiToken"] ?? throw new InvalidOperationException("TusFacturasAPP:ApiToken no está configurado");
        }

        public string GetTusFacturasBaseUrl()
        {
            return _configuration["TusFacturasAPP:BaseUrl"] ?? "https://www.tusfacturas.app/app/api/v2";
        }
    }
}
