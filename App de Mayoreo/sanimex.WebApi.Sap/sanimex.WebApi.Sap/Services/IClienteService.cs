namespace sanimex.WebApi.Sap.Services
{
    public interface IClienteService
    {
        Task<object> ClientesMayoreo(string idCliente, int canalVenta, string empresa, string rfcCte = "");
    }
}
