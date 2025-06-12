namespace sanimex.WebApi.Sap.Services
{
    public interface ISimuladorPieza
    {
        Task<object> SimuladorPiezaAsync(string barcode, string noCliente, string centrosCorredor);
    }
}
