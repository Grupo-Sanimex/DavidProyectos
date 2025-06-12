namespace sanimex.WebApi.Sap.Services
{
    public interface ISimuladorPedidos
    {
        Task<object> SimuladarPedidos(string codigosProductos, string noCliente, string canalVenta, string empresa, string claveSap, bool validador = true);
    }
}
