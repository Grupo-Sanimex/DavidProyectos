using InventarioDatos.Datos;
using InventarioDatos.Models;
using InventarioDatos.ModelsDto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioNegocio.LincenciasOffice
{
    public class LicenciaOfficeNegocio : ILicenciaOfficeNegocio
    {
        private readonly DatosDbContext _datosDbContext;
        public LicenciaOfficeNegocio(DatosDbContext datosDbContext)
        {
            _datosDbContext = datosDbContext;
        }
        public List<LicenciaOfficeDto> ObtenerLicenciasOffice()
        {
            var licenciasOffice = _datosDbContext.LicenciaOffice.ToList();
            var licenciasOfficeDto = new List<LicenciaOfficeDto>();
            foreach (var licencia in licenciasOffice)
            {
                licenciasOfficeDto.Add(new LicenciaOfficeDto
                {
                    IdLicencia = licencia.IdLicencia,
                    Cuenta = licencia.Cuenta,
                    IdEquipo = licencia.IdEquipo,
                    Status = licencia.Status
                });
            }
            return licenciasOfficeDto;
        }

        public LicenciaOfficeDto ObtenerLicenciaOfficePorId(int id)
        {
            var licenciaOffice = _datosDbContext.LicenciaOffice.FirstOrDefault(l => l.IdLicencia == id);
            if (licenciaOffice != null)
            {
                return new LicenciaOfficeDto
                {
                    IdLicencia = licenciaOffice.IdLicencia,
                    Cuenta = licenciaOffice.Cuenta,
                    IdEquipo = licenciaOffice.IdEquipo,
                    Status = licenciaOffice.Status
                };
            }
            return null;
        }
        public LicenciaOffEqDto ObtenerLicenciaOfficePorEquipo(int idEquipo)
        {
            var equipo = _datosDbContext.Equipo.FirstOrDefault(e => e.IdEquipo == idEquipo);
            var licenciaOffice = _datosDbContext.LicenciaOffice.FirstOrDefault(l => l.IdEquipo == idEquipo);
            if (licenciaOffice != null)
            {
                return new LicenciaOffEqDto
                {
                    IdLicencia = licenciaOffice.IdLicencia,
                    Cuenta = licenciaOffice.Cuenta,
                    NumeroSerie = equipo.NumeroSerie,
                    Marca = equipo.Marca,
                    Status = licenciaOffice.Status
                };
            }
            return null;
        }
        public bool AgregarLicenciaOffice(LicenciaOfficeDto licenciaOfficeDto)
        {
            // Validar entrada
            if (string.IsNullOrEmpty(licenciaOfficeDto.Cuenta) || licenciaOfficeDto.IdEquipo <= 0)
            {
                return false; // O lanzar una excepción según la lógica de negocio
            }

            // Verificar si ya existe una licencia con la misma Cuenta o IdEquipo
            var existeLicencia = _datosDbContext.LicenciaOffice
                .Any(l => l.Cuenta == licenciaOfficeDto.Cuenta || l.IdEquipo == licenciaOfficeDto.IdEquipo);

            if (existeLicencia)
            {
                return false;
            }

            // Crear nueva licencia
            var licenciaOffice = new LicenciaOffice
            {
                Cuenta = licenciaOfficeDto.Cuenta,
                IdEquipo = licenciaOfficeDto.IdEquipo,
                Status = true // Valor por defecto para nuevas licencias
            };

            _datosDbContext.LicenciaOffice.Add(licenciaOffice);

            try
            {
                _datosDbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false; // O lanzar una excepción según la lógica de negocio
            }
        }
        public bool ActualizarLicenciaOffice(LicenciaOfficeDto licenciaOfficeDto)
        {
            // Validar entrada
            if (licenciaOfficeDto.IdLicencia <= 0)
            {
                return false;
            }

            var licenciaOffice = _datosDbContext.LicenciaOffice
                .FirstOrDefault(l => l.IdLicencia == licenciaOfficeDto.IdLicencia);

            if (licenciaOffice == null)
            {
                return false;
            }

            // Actualizar campos
            licenciaOffice.Cuenta = licenciaOfficeDto.Cuenta;
            licenciaOffice.IdEquipo = licenciaOfficeDto.IdEquipo;
            licenciaOffice.Status = licenciaOfficeDto.Status;

            try
            {
                _datosDbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public void EliminarLicenciaOffice(int id)
        {
            var licenciaOffice = _datosDbContext.LicenciaOffice.FirstOrDefault(d => d.IdLicencia == id);
            if (licenciaOffice != null)
            {
                if (licenciaOffice.Status == false)
                {
                    licenciaOffice.Status = true;
                }
                else
                {
                    licenciaOffice.Status = false;
                }
                _datosDbContext.SaveChanges();
            }
        }
    }
}
