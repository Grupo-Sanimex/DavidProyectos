using sanimex.webapi.Dominio.Models;
using sanimex.webapi.Dominio.Models.Producto;
using System.Data;

namespace sanimex.WebApi.Sap.Services
{
    public interface IDispoMayoreoNegocio
    {
        //Task<DataSet> DisponibilidadxMayoreoAsync(string barcode, string centrosCorredor, string sucursalesDepend, bool soloExistencias = true);
        Task<List<MDisponibilidadMayoreo>> DisponibilidadxMayoreoAsync(string barcode, string centrosCorredor, bool soloExistencias = true);
        //Task<DataSet> DisponibilidadxMayoreoAsync(string barcode, string centrosCorredor, string sucursalesDepend, bool soloExistencias = true);
    }
}
