using Dapper;
using MySql.Data.MySqlClient;
using sanimex.webapi.Datos.Comun;
using sanimex.webapi.Dominio.Models;
using sanimex.webapi.Dominio.Models.GerentesSucursal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeZoneConverter;
using static Mysqlx.Crud.Order.Types;

namespace sanimex.webapi.Datos.Servicio.GerentesSucursalSer
{
    public class GerenteSucurServicio : IGerenteSucurServicio
    {
        private readonly string _connectionString;
        public GerenteSucurServicio(IDatos databaseConfig)
        {
            _connectionString = databaseConfig.GetConnectionString();
        }

        public async Task<GerenteSucursal> ObtenerSucursalGerente(int id) // Método asíncrono que devuelve un objeto Empleado
        {
            // Consulta SQL directa para obtener el empleado
            string query = "SELECT idSucursalApp, idGerente, claveSucursal, fechaCreacion, fechaModificacion, status FROM SucursalGerentesApp;"; // Asegúrate de que el nombre de la tabla y los campos sean correctos
            using (var con = new MySqlConnection(_connectionString))
            {
                await con.OpenAsync(); // Abrir la conexión asíncronamente

                // Ejecutar la consulta y recuperar el primer objeto Empleado
                var usuario = await con.QueryFirstOrDefaultAsync<GerenteSucursal>(query);

                return usuario; // Devolver el objeto Empleado o null si no existe
            }
        }

        public async Task<bool> GuardarSucursalGerente(int idGerente, string Sucursales)
        {
            // Nombre del procedimiento almacenado
            string query = "INSERT INTO SucursalGerentesApp (idGerente,claveSucursal,fechaCreacion,fechaModificacion)" +
                " SELECT @idGerente, @claveSucursal, @fechaCreacion, @fechaModificacion FROM DUAL " +
                " WHERE NOT EXISTS ( SELECT 1 FROM SucursalGerentesApp" +
                " WHERE idGerente = @idGerente AND claveSucursal = @claveSucursal);";

            // Obtener la zona horaria de Ciudad de México
            TimeZoneInfo mexicoTimeZone = TZConvert.GetTimeZoneInfo("America/Mexico_City");

            // Obtener la fecha y hora actual en UTC y convertirla a la zona horaria de México
            DateTime mexicoNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, mexicoTimeZone);

            // Obtener solo la fecha sin la hora
            DateTime fechaSinHora = mexicoNow.Date;
            TimeSpan horaActual = new TimeSpan(mexicoNow.Hour, mexicoNow.Minute, mexicoNow.Second);
            // Crear una instancia de DynamicParameters
            var parametros = new DynamicParameters();
            parametros.Add("idGerente", idGerente, dbType: DbType.Int32);
            parametros.Add("claveSucursal", Sucursales, dbType: DbType.String);
            parametros.Add("fechaCreacion", fechaSinHora, dbType: DbType.DateTime);
            parametros.Add("fechaModificacion", fechaSinHora, dbType: DbType.DateTime);

            try
            {
                using (var con = new MySqlConnection(_connectionString))
                {
                    await con.OpenAsync();
                    int filasAfectadas = await con.ExecuteAsync(query, parametros, commandType: CommandType.Text);
                    return filasAfectadas > 0;
                }
            }
            catch (Exception ex)
            {
                // Log del error (puedes usar un logger o simplemente imprimir en consola)
                Console.WriteLine($"Error al ejecutar el procedimiento almacenado: {ex.Message}");
                return false;
            }
        }


    }
}
