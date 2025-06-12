using Dapper;
using MySql.Data.MySqlClient;
using sanimex.webapi.Datos.Comun;
using sanimex.webapi.Dominio.Models.webMayoreo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace sanimex.webapi.Datos.Servicio.ReportesServicio
{
    public class ReporteServicio : IReporteServicio
    {
        private readonly string _connectionString;
        public ReporteServicio(IDatos databaseConfig)
        {
            _connectionString = databaseConfig.GetConnectionString();
        }

        public async Task<Gerente> ObtenerGerentes()
        {
            // Consulta SQL directa para obtener el empleado
            string query = "SELECT distinct u.idUsuario, u.nombre, u.aPaterno, u.aMaterno, u.numEmpleado, s.idSAP FROM Sucursales s " +
                "INNER JOIN Usuarios u ON s.idSucursal = u.idSucursal " +
                "INNER JOIN SupervisorSucursales sp ON s.idSucursal = sp.idSucursal " +
                "WHERE s.canalVenta = 2 and s.status = 1 and sp.status = 1 and u.idRol = 10 and u.status = 1";

            // Crear una instancia de DynamicParameters
            var parametros = new DynamicParameters();

            using (var con = new MySqlConnection(_connectionString))
            {
                await con.OpenAsync(); // Abrir la conexión asíncronamente

                // Ejecutar la consulta y recuperar el primer objeto Empleado
                var gerente = await con.QueryFirstOrDefaultAsync<Gerente>(query, parametros);

                return gerente; // Devolver el objeto Empleado o null si no existe
            }
        }
        public async Task<List<Gerente>> ObtenerGerentesLista()
        {
            // Consulta SQL directa para obtener el empleado
            string query = "SELECT distinct u.idUsuario, u.nombre, u.aPaterno, u.aMaterno, u.numEmpleado, s.idSAP FROM Sucursales s " +
                "INNER JOIN Usuarios u ON s.idSucursal = u.idSucursal " +
                "INNER JOIN SupervisorSucursales sp ON s.idSucursal = sp.idSucursal " +
                "WHERE s.canalVenta = 2 and s.status = 1 and sp.status = 1 and u.idRol = 10 and u.status = 1";
            // Crear una instancia de DynamicParameters
            var parametros = new DynamicParameters();
            using (var con = new MySqlConnection(_connectionString))
            {
                await con.OpenAsync(); // Abrir la conexión asíncronamente
                // Ejecutar la consulta y recuperar el primer objeto Empleado
                var gerente = await con.QueryAsync<Gerente>(query, parametros);
                return gerente.ToList(); // Devolver el objeto Empleado o null si no existe
            }
        }
        // Consulta SQL directa para obtener el cotizaciones de cada visitador
        public async Task<List<CotizacionS>> Cotizaciones(int idGerente, string Fecha)
        {
            // Consulta SQL directa para obtener el empleado
            string query = "SELECT cm.idDispositivo, cm.tipo_consulta, s.idSAP as claveSucursal, s.nombre AS Sucursal, u.nombre, u.aPaterno, cm.Status, cm.totalCotizacion, cm.idClienteSAP, cm.idventa " +
                "FROM CotizacionMaster cm " +
                "INNER JOIN Usuarios u ON cm.idDispositivo = u.idUsuario " +
                "INNER JOIN Sucursales s ON cm.idSucursal = s.idSucursal " +
                "WHERE cm.idSucursal IN (SELECT ss.idSucursal FROM SupervisorSucursales ss " +
                "WHERE ss.idUsuario = @idGerente and ss.Status = 1) and DATE(cm.fechaAlta) = @Fecha";
            // Crear una instancia de DynamicParameters
            var parametros = new DynamicParameters();
            parametros.Add("idGerente", idGerente, DbType.Int32);
            parametros.Add("Fecha", Fecha, DbType.String);
            using (var con = new MySqlConnection(_connectionString))
            {
                await con.OpenAsync(); // Abrir la conexión asíncronamente
                // Ejecutar la consulta y recuperar el primer objeto Empleado
                var gerente = await con.QueryAsync<CotizacionS>(query, parametros);
                return gerente.ToList(); // Devolver el objeto Empleado o null si no existe
            }
        }
        // Consulta SQL directa para obtener el cotizaciones de cada visitador por rango de fechas

        public async Task<List<CotizacionS>> CotizacionesRango(int idGerente, string FechaInicio, string FechaFin)
        {
            string query = "SELECT cm.idDispositivo, s.nombre AS Sucursal, u.nombre, u.aPaterno, cm.Status, cm.totalCotizacion, cm.idClienteSAP, cm.idventa " +
                "FROM CotizacionMaster cm " +
                "INNER JOIN Usuarios u ON cm.idDispositivo = u.idUsuario " +
                "INNER JOIN Sucursales s ON cm.idSucursal = s.idSucursal " +
                "WHERE cm.idSucursal IN (SELECT ss.idSucursal FROM SupervisorSucursales ss " +
                "WHERE ss.idUsuario = @idGerente and ss.Status = 1) and DATE(cm.fechaAlta) BETWEEN @FechaInicio and @FechaFin;";
            var parametros = new DynamicParameters();
            parametros.Add("idGerente", idGerente, DbType.Int32);
            parametros.Add("FechaInicio", FechaInicio, DbType.String);
            parametros.Add("FechaFin", FechaFin, DbType.String);
            using (var con = new MySqlConnection(_connectionString))
            {
                await con.OpenAsync();
                var gerente = await con.QueryAsync<CotizacionS>(query, parametros);
                return gerente.ToList();
            }
        }

        public async Task<List<ConsultasCliente>> ConsultasClientes(string idUsuario, string Fecha)
        {
            string query = "SELECT numCliente, idSAP, claveArticulo, COUNT(*) as vecesConsultado " +
                "FROM LogsClienteApp WHERE idUsuario = @idUsuario AND DATE(Fecha) = @Fecha GROUP BY numCliente, idSAP, claveArticulo;";
            var parametros = new DynamicParameters();
            parametros.Add("idUsuario", idUsuario, DbType.Int32);
            parametros.Add("Fecha", Fecha, DbType.String);
            using (var con = new MySqlConnection(_connectionString))
            {
                await con.OpenAsync();
                var gerente = await con.QueryAsync<ConsultasCliente>(query, parametros);
                return gerente.ToList();
            }
        }

    }
}
