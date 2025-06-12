using Dapper;
using MySql.Data.MySqlClient;
using sanimex.webapi.Datos.Comun;
using sanimex.webapi.Datos.Servicio.ClienteServicio.Interfaces;
using sanimex.webapi.Dominio.Models;
using sanimex.webapi.Dominio.Models.Clientes;
using sanimex.webapi.Dominio.Models.Usuarios;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sanimex.webapi.Datos.Servicio.ClienteServicio.Implementacion
{
    public class ClienteServicio : IClienteServicio
    {
        private readonly string _connectionString;

        public ClienteServicio(IDatos databaseConfig)
        {
            _connectionString = databaseConfig.GetConnectionString();
        }
        public async Task<ClienteBd> ObtenerCliente(int id) // Método asíncrono que devuelve un objeto Empleado
        {
            // Consulta SQL directa para obtener el empleado
            string query = "SELECT idClienteMayoreo, nombre, apaterno, amaterno, rfc, status, creditoTotal, creditoDisponible, diasCredito,correo,telefonoMovil,telefonoCasa FROM ClientesMayoreo WHERE idClienteMayoreo = @id"; // Asegúrate de que el nombre de la tabla y los campos sean correctos

            // Crear una instancia de DynamicParameters
            var parametros = new DynamicParameters();
            parametros.Add("id", id, dbType: DbType.Int32); // Usar el nombre correcto del parámetro

            using (var con = new MySqlConnection(_connectionString))
            {
                await con.OpenAsync(); // Abrir la conexión asíncronamente

                // Ejecutar la consulta y recuperar el primer objeto Empleado
                var cliente = await con.QueryFirstOrDefaultAsync<ClienteBd>(query, parametros);

                return cliente!; // Devolver el objeto Empleado o null si no existe
            }
        }
        public async Task<List<Historico_Vta_May>> HistoricoVtaMay(int ClienteSap) // Método asíncrono que devuelve un objeto Empleado
        {
            string query = "Historico_Vta_May_App"; // Nombre del procedimiento almacenado
            var parametros = new DynamicParameters(); // Crear una instancia de DynamicParameters
            parametros.Add("ClienteSap", ClienteSap, dbType: DbType.Int32); // Asegúrate de usar el nombre correcto del parámetro

            using (var con = new MySqlConnection(_connectionString))
            {
                await con.OpenAsync(); // Abrir la conexión asíncronamente

                // Ejecutar el procedimiento almacenado y recuperar el primer objeto Empleado
                var empleado = await con.QueryAsync<Historico_Vta_May>(query, parametros, commandType: CommandType.StoredProcedure);

                return empleado.ToList(); // Devolver el objeto Empleado o null si no existe
            }
        }

        public async Task<string> maxNumeroClientes(string idUsuario, int numCliente)
        {
            // Nombre del procedimiento almacenado
            string query = "LogsClienteContadorApp";

            // Obtener la fecha actual (sin tiempo)
            DateTime hoy = DateTime.Today;

            // Crear una instancia de DynamicParameters
            var parametros = new DynamicParameters();
            parametros.Add("p_idUsuario", idUsuario, dbType: DbType.String); // Parámetro de usuario
            parametros.Add("p_numCliente", numCliente, dbType: DbType.Int32); // Parámetro de cliente
            parametros.Add("p_fecha", hoy, dbType: DbType.Date); // Fecha actual (sin tiempo)
            parametros.Add("p_total_clientes", dbType: DbType.Int32, direction: ParameterDirection.Output); // Parámetro de salida
            parametros.Add("p_mensaje", dbType: DbType.String, direction: ParameterDirection.Output, size: 255); // Mensaje de salida

            using (var con = new MySqlConnection(_connectionString))
            {
                await con.OpenAsync();

                // Ejecutar la consulta y obtener los parámetros de salida
                await con.ExecuteAsync(query, parametros, commandType: CommandType.StoredProcedure);

                // Leer los parámetros de salida
                string mensaje = parametros.Get<string>("p_mensaje");

                // Retornar el resultado
                return mensaje;
            }
        }
        public async Task<Metros_Importe_Mayoreo> MetrosImporteMayoreo(string ClaveCliente) // Método asíncrono que devuelve un objeto Empleado
        {
            string query = "Metros_Imp_May_app"; // Nombre del procedimiento almacenado
            var parametros = new DynamicParameters(); // Crear una instancia de DynamicParameters
            parametros.Add("ClienteSap", Convert.ToUInt32(ClaveCliente), dbType: DbType.Int32); // Asegúrate de usar el nombre correcto del parámetro

            using (var con = new MySqlConnection(_connectionString))
            {
                await con.OpenAsync(); // Abrir la conexión asíncronamente

                // Ejecutar el procedimiento almacenado y recuperar el primer objeto Empleado
                var metrosMeta = await con.QueryFirstOrDefaultAsync<Metros_Importe_Mayoreo>(query, parametros, commandType: CommandType.StoredProcedure);

                return metrosMeta!; // Devolver el objeto Empleado o null si no existe
            }
        }

    }
}
