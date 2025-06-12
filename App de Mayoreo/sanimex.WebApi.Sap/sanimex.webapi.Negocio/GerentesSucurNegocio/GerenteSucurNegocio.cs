using sanimex.webapi.Datos.Servicio.GerentesSucursalSer;
using sanimex.webapi.Dominio.Models.GerentesSucursal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sanimex.webapi.Negocio.GerentesSucurNegocio
{
    public class GerenteSucurNegocio : IGerenteSucurNegocio
    {
        private readonly IGerenteSucurServicio _gerenteSucurServicio;

        public GerenteSucurNegocio(IGerenteSucurServicio gerenteSucurServicio)
        {
            _gerenteSucurServicio = gerenteSucurServicio;
        }

        public async Task<GerenteSucursal> GerenteSucursal(int id)
        {
            return await _gerenteSucurServicio.ObtenerSucursalGerente(id);
        }
        public async Task<bool> GuardarSucursalGerente(int idGerente, string Sucursales)
        {
            return await _gerenteSucurServicio.GuardarSucursalGerente(idGerente, Sucursales);
        }
    }
}
