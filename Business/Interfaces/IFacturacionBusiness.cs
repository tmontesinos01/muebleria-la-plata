namespace Business.Interfaces
{
    public interface IFacturacionBusiness
    {
        double CalcularCostoMensual(string region, long reads, long writes, long deletes, double storedDataGiB);
    }
}
