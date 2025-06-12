using sanimex.webapi.Dominio.Models.Producto;
using System.Collections;
using System.Data;

namespace sanimex.webapi.Negocio.Producto
{
    public interface IProductoNegocio
    {
        Task<PrecioCliente> CalculaDescuentos_Mayoreo(bool tipoEntrega, bool tipoPago, string? ClaveCliente, decimal PrecioProducto);
        Task<ProductoCliente> CargarClienteBusqueda();
        Task<ProductoUnitario> CargarProductoUnitario();
        //Task<ProductoCliente> ClienteUnitario(string CodigoBarra, string centrosCorredor, bool tipoEntrega, bool tipoPago, string ClaveCliente, string idUsuario);
        Task<ProductoCliente> ClienteUnitario(string CodigoBarra, string centrosCorredor, bool tipoEntrega, bool tipoPago, string ClaveCliente, string idUsuario, bool TipoConsulta, int idDireccion);
        Task<DataSet> GetResultadosElasticSearchAsync(string CodigoBarra);
        Hashtable LeerTablasDescuento(DataTable tablaProms, DataTable tablaItems);
        Task<Hashtable> ObtenerDescuentoSimulador(string codebar);
        Task<Hashtable> ObtenerDescuentoSimulador(string codebar, string claveCliente);
        Task<List<BusquedaProducto>> ObtenerProducto(string busqueda, int limite);
        Task<MProducto> ProductoCarrito(string CodigoBarra);
        Task<ProductoUnitario> ProductoUnitario(string CodigoBarra, string centrosCorredor, string idUsuario);
        //Task<ProductoUnitario> ProductoUnitario(string CodigoBarra, string centrosCorredor, string idUsuario, int claveCliente);
    }
}