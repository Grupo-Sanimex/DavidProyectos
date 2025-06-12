using InventarioDatos.Datos;
using InventarioDatos.Models;
using InventarioDatos.ModelsDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioNegocio.Equipos
{
    public class EquipoNegocio : IEquipoNegocio
    {
        private readonly DatosDbContext _datosDbContext;
        public EquipoNegocio(DatosDbContext datosDbContext)
        {
            _datosDbContext = datosDbContext;
        }
        public List<EquipoDto> ObtenerEquipos()
        {
            var equipos = _datosDbContext.Equipo.ToList();
            var equiposDto = new List<EquipoDto>();
            foreach (var equipo in equipos)
            {
                equiposDto.Add(new EquipoDto
                {
                    IdEquipo = equipo.IdEquipo,
                    NumeroSerie = equipo.NumeroSerie,
                    Etiqueta = equipo.Etiqueta,
                    Marca = equipo.Marca,
                    Modelo = equipo.Modelo,
                    Ip = equipo.Ip,
                    Ram = equipo.Ram,
                    DiscoDuro = equipo.DiscoDuro,
                    Procesador = equipo.Procesador,
                    So = equipo.So,
                    EquipoEstatus = equipo.EquipoEstatus,
                    Empresa = equipo.Empresa,
                    Renovar = equipo.Renovar,
                    FechaUltimaCaptura = equipo.FechaUltimaCaptura,
                    FechaUltimoMantto = equipo.FechaUltimoMantto,
                    ElaboroResponsiva = equipo.ElaboroResponsiva,
                    IdUbicacion = equipo.IdUbicacion,
                    IdDepartamento = equipo.IdDepartamento,
                    IdEmpleado = equipo.IdEmpleado,
                    Status = equipo.Status
                });
            }
            return equiposDto;
        }

        // regresar lista de equipos disponibles 
        public List<EquipoDto> ObtenerEquiposDisponibles()
        {
            // Filtra equipos donde IdEmpleado es 0 (vacío)
            var equipos = _datosDbContext.Equipo
                .Where(e => e.IdEmpleado == 0)
                .ToList();

            var equiposDto = new List<EquipoDto>();
            foreach (var equipo in equipos)
            {
                equiposDto.Add(new EquipoDto
                {
                    IdEquipo = equipo.IdEquipo,
                    NumeroSerie = equipo.NumeroSerie,
                    Etiqueta = equipo.Etiqueta,
                    Marca = equipo.Marca,
                    Modelo = equipo.Modelo,
                    Ip = equipo.Ip,
                    Ram = equipo.Ram,
                    DiscoDuro = equipo.DiscoDuro,
                    Procesador = equipo.Procesador,
                    So = equipo.So,
                    EquipoEstatus = equipo.EquipoEstatus,
                    Empresa = equipo.Empresa,
                    Renovar = equipo.Renovar,
                    FechaUltimaCaptura = equipo.FechaUltimaCaptura,
                    FechaUltimoMantto = equipo.FechaUltimoMantto,
                    ElaboroResponsiva = equipo.ElaboroResponsiva,
                    IdUbicacion = equipo.IdUbicacion,
                    IdDepartamento = equipo.IdDepartamento,
                    IdEmpleado = equipo.IdEmpleado,
                    Status = equipo.Status
                });
            }
            return equiposDto;
        }
        public EquipoDto ObtenerEquipoPorId(int id)
        {
            var equipo = _datosDbContext.Equipo.FirstOrDefault(e => e.IdEquipo == id);
            if (equipo != null)
            {
                return new EquipoDto
                {
                    IdEquipo = equipo.IdEquipo,
                    NumeroSerie = equipo.NumeroSerie,
                    Etiqueta = equipo.Etiqueta,
                    Marca = equipo.Marca,
                    Modelo = equipo.Modelo,
                    Ip = equipo.Ip,
                    Ram = equipo.Ram,
                    DiscoDuro = equipo.DiscoDuro,
                    Procesador = equipo.Procesador,
                    So = equipo.So,
                    EquipoEstatus = equipo.EquipoEstatus,
                    Empresa = equipo.Empresa,
                    Renovar = equipo.Renovar,
                    FechaUltimaCaptura = equipo.FechaUltimaCaptura,
                    FechaUltimoMantto = equipo.FechaUltimoMantto,
                    ElaboroResponsiva = equipo.ElaboroResponsiva,
                    IdUbicacion = equipo.IdUbicacion,
                    IdDepartamento = equipo.IdDepartamento,
                    IdEmpleado = equipo.IdEmpleado,
                    Status = equipo.Status
                };
            }
            return null;
        }

        public IEnumerable<EquipoDto> ObtenerEquipoPorIdEmpleado(int id)
        {
            var equipos = _datosDbContext.Equipo
                .Where(e => e.IdEmpleado == id)
                .ToList();

            var equiposDto = new List<EquipoDto>();
            foreach (var equipo in equipos)
            {
                equiposDto.Add(new EquipoDto
                {
                    IdEquipo = equipo.IdEquipo,
                    NumeroSerie = equipo.NumeroSerie,
                    Etiqueta = equipo.Etiqueta,
                    Marca = equipo.Marca,
                    Modelo = equipo.Modelo,
                    Ip = equipo.Ip,
                    Ram = equipo.Ram,
                    DiscoDuro = equipo.DiscoDuro,
                    Procesador = equipo.Procesador,
                    So = equipo.So,
                    EquipoEstatus = equipo.EquipoEstatus,
                    Empresa = equipo.Empresa,
                    Renovar = equipo.Renovar,
                    FechaUltimaCaptura = equipo.FechaUltimaCaptura,
                    FechaUltimoMantto = equipo.FechaUltimoMantto,
                    ElaboroResponsiva = equipo.ElaboroResponsiva,
                    IdUbicacion = equipo.IdUbicacion,
                    IdDepartamento = equipo.IdDepartamento,
                    IdEmpleado = equipo.IdEmpleado,
                    Status = equipo.Status
                });
            }
            return equiposDto;
        }
        public EquipoDto BuscarEquipos(string busqueda)
        {
            if (string.IsNullOrEmpty(busqueda))
            {
                return new EquipoDto();
            }
            var equipo = _datosDbContext.Equipo.FirstOrDefault(
                 e => e.NumeroSerie.Contains(busqueda) ||
                 e.Etiqueta.Contains(busqueda));

            if (equipo == null)
                return new EquipoDto();

            return new EquipoDto
            {
                IdEquipo = equipo.IdEquipo,
                NumeroSerie = equipo.NumeroSerie,
                Etiqueta = equipo.Etiqueta,
                Marca = equipo.Marca,
                Modelo = equipo.Modelo,
                Ip = equipo.Ip,
                Ram = equipo.Ram,
                DiscoDuro = equipo.DiscoDuro,
                Procesador = equipo.Procesador,
                So = equipo.So,
                EquipoEstatus = equipo.EquipoEstatus,
                Empresa = equipo.Empresa,
                Renovar = equipo.Renovar,
                FechaUltimaCaptura = equipo.FechaUltimaCaptura,
                FechaUltimoMantto = equipo.FechaUltimoMantto,
                ElaboroResponsiva = equipo.ElaboroResponsiva,
                IdUbicacion = equipo.IdUbicacion,
                IdDepartamento = equipo.IdDepartamento,
                IdEmpleado = equipo.IdEmpleado,
                Status = equipo.Status
            };
        }
        public void AgregarEquipo(EquipoDto equipoDto)
        {
            var nuevoEquipo = new Equipo
            {
                NumeroSerie = equipoDto.NumeroSerie,
                Etiqueta = equipoDto.Etiqueta,
                Marca = equipoDto.Marca,
                Modelo = equipoDto.Modelo,
                Ip = equipoDto.Ip,
                Ram = equipoDto.Ram,
                DiscoDuro = equipoDto.DiscoDuro,
                Procesador = equipoDto.Procesador,
                So = equipoDto.So,
                EquipoEstatus = equipoDto.EquipoEstatus,
                Empresa = equipoDto.Empresa,
                Renovar = equipoDto.Renovar,
                FechaUltimaCaptura = DateTime.Now,
                FechaUltimoMantto = DateTime.Now,
                ElaboroResponsiva = equipoDto.ElaboroResponsiva,
                IdUbicacion = equipoDto.IdUbicacion,
                IdDepartamento = null,
                IdEmpleado = null,
                Status = true
            };
            _datosDbContext.Equipo.Add(nuevoEquipo);
            _datosDbContext.SaveChanges();
        }
        public void ActualizarEquipo(EquipoDto equipo)
        {
            var equipoExistente = _datosDbContext.Equipo.FirstOrDefault(e => e.IdEquipo == equipo.IdEquipo);
            if (equipoExistente != null)
            {
                equipoExistente.NumeroSerie = equipo.NumeroSerie;
                equipoExistente.Etiqueta = equipo.Etiqueta;
                equipoExistente.Marca = equipo.Marca;
                equipoExistente.Modelo = equipo.Modelo;
                equipoExistente.Ip = equipo.Ip;
                equipoExistente.Ram = equipo.Ram;
                equipoExistente.DiscoDuro = equipo.DiscoDuro;
                equipoExistente.Procesador = equipo.Procesador;
                equipoExistente.So = equipo.So;
                equipoExistente.EquipoEstatus = equipo.EquipoEstatus;
                equipoExistente.Empresa = equipo.Empresa;
                equipoExistente.Renovar = equipo.Renovar;
                equipoExistente.FechaUltimaCaptura = equipo.FechaUltimaCaptura;
                equipoExistente.FechaUltimoMantto = equipo.FechaUltimoMantto;
                equipoExistente.ElaboroResponsiva = equipo.ElaboroResponsiva;
                equipoExistente.IdUbicacion = equipo.IdUbicacion;
                equipoExistente.IdDepartamento = equipo.IdDepartamento;
                equipoExistente.IdEmpleado = equipo.IdEmpleado;
                _datosDbContext.SaveChanges();
            }
        }
        public void EliminarEquipo(int id)
        {
            var equipo = _datosDbContext.Equipo.FirstOrDefault(d => d.IdEquipo == id);
            if (equipo != null)
            {
                equipo.Status = false;
                _datosDbContext.SaveChanges();
            }
        }
    }
}
