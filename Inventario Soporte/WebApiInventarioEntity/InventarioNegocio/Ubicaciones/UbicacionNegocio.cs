using InventarioDatos.Datos;
using InventarioDatos.Models;
using InventarioDatos.ModelsDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioNegocio.Ubicaciones
{
    public class UbicacionNegocio : IUbicacionNegocio
    {
        private readonly DatosDbContext _datosDbContext;
        public UbicacionNegocio(DatosDbContext datosDbContext)
        {
            _datosDbContext = datosDbContext;
        }
        public List<UbicacionDto> ObtenerUbicaciones()
        {
            return _datosDbContext.Ubicacion
                .Select(u => new UbicacionDto
                {
                    IdUbicacion = u.IdUbicacion,
                    Zona = u.Zona,
                    Region = u.Region,
                    Centro = u.Centro,
                    Estado = u.Estado,
                    Sucursal = u.Sucursal,
                    Status = u.Status
                })
                .ToList();
        }
        public UbicacionDto ObtenerUbicacionPorId(int id)
        {
            var ubicacion = _datosDbContext.Ubicacion.Find(id);
            if (ubicacion == null)
            {
                return null;
            }
            return new UbicacionDto
            {
                IdUbicacion = ubicacion.IdUbicacion,
                Zona = ubicacion.Zona,
                Region = ubicacion.Region,
                Centro = ubicacion.Centro,
                Estado = ubicacion.Estado,
                Sucursal = ubicacion.Sucursal,
                Status = ubicacion.Status
            };
        }
        public void AgregarUbicacion(UbicacionDto ubicacionDto)
        {
            var ubicacion = new Ubicacion
            {
                Zona = ubicacionDto.Zona,
                Region = ubicacionDto.Region,
                Centro = ubicacionDto.Centro,
                Estado = ubicacionDto.Estado,
                Sucursal = ubicacionDto.Sucursal,
                Status = true
            };
            _datosDbContext.Ubicacion.Add(ubicacion);
            _datosDbContext.SaveChanges();
            ubicacionDto.IdUbicacion = ubicacion.IdUbicacion;
        }
        public void ActualizarUbicacion(UbicacionDto ubicacionDto)
        {
            var ubicacion = _datosDbContext.Ubicacion.Find(ubicacionDto.IdUbicacion);
            if (ubicacion != null)
            {
                ubicacion.Zona = ubicacionDto.Zona;
                ubicacion.Region = ubicacionDto.Region;
                ubicacion.Centro = ubicacionDto.Centro;
                ubicacion.Estado = ubicacionDto.Estado;
                ubicacion.Sucursal = ubicacionDto.Sucursal;
                _datosDbContext.SaveChanges();
            }
        }
        public void EliminarUbicacion(int id)
        {
            var ubicacion = _datosDbContext.Ubicacion.FirstOrDefault(u => u.IdUbicacion == id);
            if (ubicacion != null)
            {
                ubicacion.Status = false;
                _datosDbContext.SaveChanges();
            }
        }
    }
}
