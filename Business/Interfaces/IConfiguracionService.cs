namespace Business.Interfaces
{
    public interface IConfiguracionService
    {
        string GetTusFacturasUserToken();
        string GetTusFacturasApiKey();
        string GetTusFacturasApiToken();
        string GetTusFacturasBaseUrl();
    }
}
