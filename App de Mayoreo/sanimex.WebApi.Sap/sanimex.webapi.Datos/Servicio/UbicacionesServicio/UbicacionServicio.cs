using Dapper;
using MySql.Data.MySqlClient;
using sanimex.webapi.Datos.Comun;
using sanimex.webapi.Datos.Servicio.UsuarioServicio;
using sanimex.webapi.Dominio.Models;
using sanimex.webapi.Dominio.Models.Ubicaciones;
using sanimex.webapi.Dominio.Models.Usuarios;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeZoneConverter;

namespace sanimex.webapi.Datos.Servicio.UbicacionesServicio
{
    public class UbicacionServicio : IUbicacionServicio
    {
        private readonly string _connectionString;
        private readonly IUsuarioServicio _usuarioServicio;

        public UbicacionServicio(IDatos databaseConfig, IUsuarioServicio usuarioServicio)
        {
            _connectionString = databaseConfig.GetConnectionString();
            _usuarioServicio = usuarioServicio;
        }
        public async Task<int> InsertarUbicacion(string direccion, double latitud, double longitud, string claveSucursal, int numeroEmpleado, string idRol, bool tipoIngreso)
        {
            // Nombre del procedimiento almacenado
            string query = "INSERT INTO AppLogsUbicaciones (direccion, latitud, longitud, claveSucursal, fechaUnitaria, horaUnitaria, numeroEmpleado, idRol, tipo)" +
                " SELECT @direccion, @latitud, @longitud, @claveSucursal, @fechaUnitaria, @horaUnitaria, @numeroEmpleado, @idRol, @tipo FROM DUAL " +
                " WHERE NOT EXISTS ( SELECT 1 FROM AppLogsUbicaciones" +
                " WHERE direccion = @direccion AND latitud = @latitud AND longitud = @longitud " +
                " AND claveSucursal = @claveSucursal AND numeroEmpleado = @numeroEmpleado AND idRol = @idRol " +
                " AND ABS(TIMESTAMPDIFF(SECOND, CONCAT(fechaUnitaria, ' ', horaUnitaria), CONCAT(@fechaUnitaria, ' ', @horaUnitaria))) <= 5);";

            // Obtener la zona horaria de Ciudad de México
            TimeZoneInfo mexicoTimeZone = TZConvert.GetTimeZoneInfo("America/Mexico_City");

            // Obtener la fecha y hora actual en UTC y convertirla a la zona horaria de México
            DateTime mexicoNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, mexicoTimeZone);

            // Obtener solo la fecha sin la hora
            DateTime fechaSinHora = mexicoNow.Date;
            TimeSpan horaActual = new TimeSpan(mexicoNow.Hour, mexicoNow.Minute, mexicoNow.Second);
            int tipo = 0;
            if (tipoIngreso)
            {
                tipo = 1;
            }
            // Crear una instancia de DynamicParameters
            var parametros = new DynamicParameters();
            parametros.Add("direccion", direccion, dbType: DbType.String);
            parametros.Add("latitud", latitud, dbType: DbType.Decimal);
            parametros.Add("longitud", longitud, dbType: DbType.Decimal);
            parametros.Add("claveSucursal", claveSucursal, dbType: DbType.String);
            parametros.Add("fechaUnitaria", fechaSinHora, dbType: DbType.Date);
            parametros.Add("horaUnitaria", horaActual, dbType: DbType.Time);
            parametros.Add("numeroEmpleado", numeroEmpleado, dbType: DbType.Int32);
            parametros.Add("idRol", idRol, dbType: DbType.Int32);
            parametros.Add("tipo", tipo, dbType: DbType.Int16);

            try
            {
                using (var con = new MySqlConnection(_connectionString))
                {
                    await con.OpenAsync();

                    // Ejecutar el INSERT
                    await con.ExecuteAsync(query, parametros, commandType: CommandType.Text);

                    // Obtener el último ID insertado
                    var lastId = await con.ExecuteScalarAsync<long>("SELECT LAST_INSERT_ID();");

                    return (int)lastId;
                }
            }
            catch (Exception ex)
            {
                // Log del error (puedes usar un logger o simplemente imprimir en consola)
                Console.WriteLine($"Error al ejecutar el procedimiento almacenado: {ex.Message}");
                return 0;
            }
        }
        public async Task<List<UbicacionesModel>> ListarUbicaciones() // Método asíncrono que devuelve un objeto Empleado
        {
            string query = "Sp_ListarUbicaciones"; // Nombre del procedimiento almacenado
            using (var con = new MySqlConnection(_connectionString))
            {
                await con.OpenAsync(); // Abrir la conexión asíncronamente

                // Ejecutar el procedimiento almacenado y recuperar el primer objeto Empleado
                var empleado = await con.QueryAsync<UbicacionesModel>(query, commandType: CommandType.StoredProcedure);

                return empleado.ToList(); // Devolver el objeto Empleado o null si no existe
            }
        }
        public async Task<List<UbicacionSucursal>> UbicacionSucursal(string fechaConsulta, string idUsuario) // Método asíncrono que devuelve un objeto Empleado
        {
            RolUsuario rolUsuario = await _usuarioServicio.Roles(Convert.ToInt32(idUsuario));
            if (rolUsuario.idRol == 1)
            {
                string query = "SELECT DISTINCT claveSucursal FROM AppLogsUbicaciones WHERE fechaUnitaria = @fechaConsulta AND idRol = 10;";

                var parametros = new DynamicParameters();
                parametros.Add("fechaConsulta", fechaConsulta, dbType: DbType.String);
                using (var con = new MySqlConnection(_connectionString))
                {
                    await con.OpenAsync(); // Abrir la conexión asíncronamente
                                           // Ejecutar el procedimiento almacenado y recuperar el primer objeto Empleado
                    var ubicacionSucursal = await con.QueryAsync<UbicacionSucursal>(query, parametros, commandType: CommandType.Text);
                    return ubicacionSucursal.ToList(); // Devolver el objeto Empleado o null si no existe
                }
            }
            else if (rolUsuario.idRol == 8)
            {
                string query = "SELECT DISTINCT s.idSAP AS claveSucursal from SupervisorSucursales ss " +
                    "INNER JOIN Sucursales s ON ss.idSucursal = s.idSucursal " +
                    "INNER JOIN AppLogsUbicaciones alu ON s.idSAP = alu.claveSucursal " +
                    "WHERE ss.idUsuario = @idUsuario and alu.fechaUnitaria = @fechaConsulta and ss.status = 1 and alu.idRol = 10;";

                var parametros = new DynamicParameters();
                parametros.Add("idUsuario", idUsuario, dbType: DbType.Int32);
                parametros.Add("fechaConsulta", fechaConsulta, dbType: DbType.String);
                using (var con = new MySqlConnection(_connectionString))
                {
                    await con.OpenAsync(); // Abrir la conexión asíncronamente
                                           // Ejecutar el procedimiento almacenado y recuperar el primer objeto Empleado
                    var ubicacionSucursal = await con.QueryAsync<UbicacionSucursal>(query, parametros, commandType: CommandType.Text);
                    return ubicacionSucursal.ToList(); // Devolver el objeto Empleado o null si no existe
                }
            }
            else if (rolUsuario.idRol == 10)
            {
                string query = "SELECT DISTINCT s.idSAP AS claveSucursal, s.nombre AS nombreSucursal from SupervisorSucursales ss " +
                    "INNER JOIN Sucursales s ON ss.idSucursal = s.idSucursal " +
                    "INNER JOIN AppLogsUbicaciones alu ON s.idSAP = alu.claveSucursal " +
                    "WHERE ss.idUsuario = @idUsuario and alu.fechaUnitaria = @fechaConsulta " +
                    "and ss.status = 1 and  alu.idRol != '1' and alu.idRol != '10' and alu.idRol != '8';";
                var parametros = new DynamicParameters();
                parametros.Add("idUsuario", idUsuario, dbType: DbType.String);
                parametros.Add("fechaConsulta", fechaConsulta, dbType: DbType.String);
                using (var con = new MySqlConnection(_connectionString))
                {
                    await con.OpenAsync(); // Abrir la conexión asíncronamente
                                           // Ejecutar el procedimiento almacenado y recuperar el primer objeto Empleado
                    var ubicacionSucursal = await con.QueryAsync<UbicacionSucursal>(query, parametros, commandType: CommandType.Text);
                    return ubicacionSucursal.ToList(); // Devolver el objeto Empleado o null si no existe
                }
            }
            return null;

        }
        public async Task<List<UbicacionesMaps>> UbicacionesMaps(string claveSucursal, string numeroEmpleado, string fechaUnitaria)
        {
            string query = "SELECT DISTINCT id, direccion, latitud, longitud, horaUnitaria from AppLogsUbicaciones WHERE claveSucursal = @claveSucursal and numeroEmpleado = @numeroEmpleado and fechaUnitaria = @fechaUnitaria and tipo = 1 ORDER BY horaUnitaria;";
            var parametros = new DynamicParameters();
            parametros.Add("claveSucursal", claveSucursal, dbType: DbType.String);
            parametros.Add("numeroEmpleado", numeroEmpleado, dbType: DbType.String);
            parametros.Add("fechaUnitaria", fechaUnitaria, dbType: DbType.String);
            using (var con = new MySqlConnection(_connectionString))
            {
                await con.OpenAsync();
                var ubicacionesMaps = await con.QueryAsync<UbicacionesMaps>(query, parametros, commandType: CommandType.Text);
                return ubicacionesMaps.ToList();
            }
        }

        public async Task<List<UbicacionesMaps>> UbicacionesVisitaMaps(string claveSucursal, string numeroEmpleado, string fechaUnitaria)
        {
            string query = "SELECT DISTINCT id, direccion, latitud, longitud, horaUnitaria from AppLogsUbicaciones WHERE claveSucursal = @claveSucursal and numeroEmpleado = @numeroEmpleado and fechaUnitaria = @fechaUnitaria and tipo = 0 ORDER BY horaUnitaria;";
            var parametros = new DynamicParameters();
            parametros.Add("claveSucursal", claveSucursal, dbType: DbType.String);
            parametros.Add("numeroEmpleado", numeroEmpleado, dbType: DbType.String);
            parametros.Add("fechaUnitaria", fechaUnitaria, dbType: DbType.String);
            using (var con = new MySqlConnection(_connectionString))
            {
                await con.OpenAsync();
                var ubicacionesMaps = await con.QueryAsync<UbicacionesMaps>(query, parametros, commandType: CommandType.Text);
                return ubicacionesMaps.ToList();
            }
        }

        public async Task<List<VisitadorActivo>> VisitadorActivos(string claveSucursal, string fechaUnitaria, string usuarioId) // Método asíncrono que devuelve un objeto Empleado
        {
            RolUsuario rolUsuario = await _usuarioServicio.Roles(Convert.ToInt32(usuarioId));
            if (rolUsuario.idRol == 1)
            {
                string query = "SELECT DISTINCT al.claveSucursal ,al.numeroEmpleado, us.nombre, us.aPaterno, us.aMaterno FROM AppLogsUbicaciones al JOIN Usuarios us ON al.numeroEmpleado = us.idUsuario WHERE al.claveSucursal = @claveSucursal and fechaUnitaria = @fechaUnitaria and al.idRol = 10;";
                var parametros = new DynamicParameters();
                parametros.Add("claveSucursal", claveSucursal, dbType: DbType.String);
                parametros.Add("fechaUnitaria", fechaUnitaria, dbType: DbType.String);

                using (var con = new MySqlConnection(_connectionString))
                {
                    await con.OpenAsync(); // Abrir la conexión asíncronamente
                    var visitadorActivo = await con.QueryAsync<VisitadorActivo>(query, parametros, commandType: CommandType.Text);
                    return visitadorActivo.ToList(); // Devolver la lista de visitadores activos
                }
            }
            else if (rolUsuario.idRol == 8)
            {
                string query = "SELECT DISTINCT al.claveSucursal, s.nombre AS nombreSucursal, us.idUsuario ,us.numEmpleado AS numeroEmpleado, us.nombre, us.aPaterno, us.aMaterno" +
                  " FROM AppLogsUbicaciones al JOIN Usuarios us ON al.numeroEmpleado = us.idUsuario" +
                  " JOIN Sucursales s ON al.claveSucursal = s.idSAP " +
                  " WHERE al.claveSucursal = @claveSucursal and fechaUnitaria = @fechaUnitaria and al.Tipo = 1 and al.idRol != '1' and al.idRol != '10' and al.idRol != '8';";
                var parametros = new DynamicParameters();
                parametros.Add("claveSucursal", claveSucursal, dbType: DbType.String);
                parametros.Add("fechaUnitaria", fechaUnitaria, dbType: DbType.String);

                using (var con = new MySqlConnection(_connectionString))
                {
                    await con.OpenAsync(); // Abrir la conexión asíncronamente
                    var visitadorActivo = await con.QueryAsync<VisitadorActivo>(query, parametros, commandType: CommandType.Text);
                    return visitadorActivo.ToList(); // Devolver la lista de visitadores activos
                }
            }
            else
            {
                string query = "SELECT DISTINCT al.claveSucursal, s.nombre AS nombreSucursal, us.idUsuario ,us.numEmpleado AS numeroEmpleado, us.nombre, us.aPaterno, us.aMaterno" +
                    " FROM AppLogsUbicaciones al JOIN Usuarios us ON al.numeroEmpleado = us.idUsuario" +
                    " JOIN Sucursales s ON al.claveSucursal = s.idSAP " +
                    " WHERE al.claveSucursal = @claveSucursal and fechaUnitaria = @fechaUnitaria and al.Tipo = 1 and al.idRol != '1' and al.idRol != '10' and al.idRol != '8';";
                var parametros = new DynamicParameters();
                parametros.Add("claveSucursal", claveSucursal, dbType: DbType.String);
                parametros.Add("fechaUnitaria", fechaUnitaria, dbType: DbType.String);

                using (var con = new MySqlConnection(_connectionString))
                {
                    await con.OpenAsync(); // Abrir la conexión asíncronamente
                    var visitadorActivo = await con.QueryAsync<VisitadorActivo>(query, parametros, commandType: CommandType.Text);
                    return visitadorActivo.ToList(); // Devolver la lista de visitadores activos
                }
            }
        }
        public async Task<UbicacionesModel> VerUbicacion(int id) // Método asíncrono que devuelve un objeto Empleado
        {
            string query = "Sp_ListarUbicacionID"; // Nombre del procedimiento almacenado
            var parametros = new DynamicParameters(); // Crear una instancia de DynamicParameters
            parametros.Add("p_id", id, dbType: DbType.Int32); // Asegúrate de usar el nombre correcto del parámetro

            using (var con = new MySqlConnection(_connectionString))
            {
                await con.OpenAsync(); // Abrir la conexión asíncronamente

                // Ejecutar el procedimiento almacenado y recuperar el primer objeto Empleado
                var ubicacion = await con.QueryFirstOrDefaultAsync<UbicacionesModel>(query, parametros, commandType: CommandType.StoredProcedure);

                return ubicacion; // Devolver el objeto Empleado o null si no existe
            }
        }
    }
}
