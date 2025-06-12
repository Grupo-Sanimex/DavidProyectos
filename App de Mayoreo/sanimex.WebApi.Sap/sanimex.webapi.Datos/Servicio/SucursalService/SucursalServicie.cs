using Dapper;
using MySql.Data.MySqlClient;
using sanimex.webapi.Datos.Comun;
using sanimex.webapi.Dominio.Models.Producto;
using sanimex.webapi.Dominio.Models.Sucursales;
using sanimex.webapi.Dominio.Models.Usuarios;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sanimex.webapi.Datos.Servicio.SucursalService
{
    public class SucursalServicie : ISucursalService
    {
        private readonly string _connectionString;

        public SucursalServicie(IDatos databaseConfig)
        {
            _connectionString = databaseConfig.GetConnectionString();
        }
        public async Task<List<Sucursal>> ObtenerSucursalSupervisor(int idUsuario) // Método asíncrono que devuelve un objeto Empleado
        {
            string query = "SELECT suc.idSucursal,suc.nombre as nombre, suc.idSAP, sup.idUsuario FROM Sucursales suc INNER JOIN SupervisorSucursales sup ON suc.idSucursal = sup.idSucursal WHERE sup.idUsuario = " + idUsuario + " AND suc.status = 1 AND IdSAP IS NOT NULL AND sup.status=1 ORDER BY nombre ASC"; // Nombre del procedimiento almacenado
            var parametros = new DynamicParameters(); // Crear una instancia de DynamicParameters
            parametros.Add("idUsuario", idUsuario, dbType: DbType.Int32); // Asegúrate de usar el nombre correcto del parámetro

            using (var con = new MySqlConnection(_connectionString))
            {
                await con.OpenAsync(); // Abrir la conexión asíncronamente
                // Ejecutar el procedimiento almacenado y recuperar el primer objeto Empleado
                var lista = await con.QueryAsync<Sucursal>(query, parametros, commandType: CommandType.Text);

                return lista.ToList(); // Devolver el objeto Empleado o null si no existe
            }
        }
        public async Task<List<Sucursal>> ObtenerSucursales(int idSucursal) // Método asíncrono que devuelve un objeto Empleado
        {
            string query = "SELECT idSucursal, nombre AS nombre, idSAP FROM Sucursales WHERE idSucursal = " + idSucursal + " AND status = 1 AND idSAP IS NOT NULL"; // Nombre del procedimiento almacenado
            var parametros = new DynamicParameters(); // Crear una instancia de DynamicParameters
            parametros.Add("idUsuario", idSucursal, dbType: DbType.Int32); // Asegúrate de usar el nombre correcto del parámetro

            using (var con = new MySqlConnection(_connectionString))
            {
                await con.OpenAsync(); // Abrir la conexión asíncronamente
                // Ejecutar el procedimiento almacenado y recuperar el primer objeto Empleado
                var lista = await con.QueryAsync<Sucursal>(query, parametros, commandType: CommandType.Text);

                return lista.ToList(); // Devolver el objeto Empleado o null si no existe
            }
        }
        public async Task<List<Sucursal>> ObtenerSucursalesAdmin() // Método asíncrono que devuelve un objeto Empleado
        {
            string query = "SELECT idSucursal, nombre, empresa, idSAP FROM Sucursales WHERE status = 1 AND IdSAP IS NOT NULL AND canalVenta = 2 ORDER BY empresa DESC, nombre ASC"; // Nombre del procedimiento almacenado

            using (var con = new MySqlConnection(_connectionString))
            {
                await con.OpenAsync(); // Abrir la conexión asíncronamente
                // Ejecutar el procedimiento almacenado y recuperar el primer objeto Empleado
                var lista = await con.QueryAsync<Sucursal>(query, commandType: CommandType.Text);

                return lista.ToList(); // Devolver el objeto Empleado o null si no existe
            }
        }
        public async Task<TipoVenta> ObtenerTipoVenta(string ClaveSap) // Método asíncrono que devuelve un objeto Empleado
        {
            // Consulta SQL directa para obtener el empleado
            string query = "SELECT empresa, canalVenta FROM Sucursales WHERE idSAP = @ClaveSap"; // Asegúrate de que el nombre de la tabla y los campos sean correctos
            // Crear una instancia de DynamicParameters
            var parametros = new DynamicParameters();
            parametros.Add("ClaveSap", ClaveSap, dbType: DbType.String)
                ; // Usar el nombre correcto del parámetro

            using (var con = new MySqlConnection(_connectionString))
            {
                await con.OpenAsync(); // Abrir la conexión asíncronamente

                // Ejecutar la consulta y recuperar el primer objeto Empleado
                var tipoVenta = await con.QueryFirstOrDefaultAsync<TipoVenta>(query, parametros);

                return tipoVenta!; // Devolver el objeto Empleado o null si no existe
            }
        }
    }
}
