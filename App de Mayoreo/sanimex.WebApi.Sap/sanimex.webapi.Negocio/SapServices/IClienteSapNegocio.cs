
namespace sanimex.webapi.Negocio.SapServices
{
    public interface IClienteSapNegocio
    {
        Task<bool> ClientesMayoreo(string idCliente, string empresa, string rfcCte = "");
    }
}