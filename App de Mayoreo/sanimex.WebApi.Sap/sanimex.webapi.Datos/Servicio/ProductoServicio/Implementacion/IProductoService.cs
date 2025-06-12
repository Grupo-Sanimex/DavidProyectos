using sanimex.webapi.Dominio.Models.Producto;
using System.Data;

namespace sanimex.webapi.Datos.Servicio.ProductoServicio.Implementacion
{
    public interface IProductoService
    {
        Task<decimal> Descuentos_Especiales_Mayoreo(string Tipo_Cliente_Clasificado, string Codebar, string Tipo_Entrega, string ClaveSap);
        Task<DataSet> GetResultadosElasticSearchAsync(string textoBuscar, string proveedor = "todos", string color = "todos", string formato = "todos");
        Task<float> MetroXCaja(string Codebar);
        Task<string> Nombre_Cliente_Cotiza_May(string idCliente);
        Task<List<BusquedaProducto>> ObtenerProducto(string busqueda, int limite);
        Task<MProducto> Producto(string CodigoBarra);
        Task<int> Productos_Bloqueados_NC(string Codebar, string claveSap);
        Task<string> Tipo_Cliente_Clas_May(string idCliente);
    }
}