using Dapper;
using MySql.Data.MySqlClient;
using sanimex.webapi.Datos.Comun;
using sanimex.webapi.Dominio.Models.Logs;
using sanimex.webapi.Dominio.Models.Sucursales;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sanimex.webapi.Datos.Servicio.LogServicio.implementacion
{
    public class LogsServicio : ILogsServicio
    {
        private readonly string _connectionString;

        public LogsServicio(IDatos databaseConfig)
        {
            _connectionString = databaseConfig.GetConnectionString();
        }

        public async Task<bool> GuardarConsultas(ClienteLogs cliente)
        {
            int tipo = 0;
            if (cliente.TipoConsulta == false)
            {
                tipo = 0;
            }
            else
            {
                tipo = 1;
            }
            // Consulta SQL directa para realizar el INSERT
            string query = "INSERT INTO LogsClienteApp (idUsuario, numCliente, idSAP, claveArticulo, numeroPedido, Fecha, TipoConsulta, idDireccion) VALUES (@idUsuario, @numCliente, @idSAP, @claveArticulo, @numeroPedido, NOW(), @TipoConsulta, @idDireccion);";

            // Crear una instancia de DynamicParameters
            var parametros = new DynamicParameters();
            parametros.Add("idUsuario", cliente.idUsuario, DbType.String);
            parametros.Add("numCliente", cliente.numCliente, DbType.String);
            parametros.Add("idSAP", cliente.idSAP, DbType.String);
            parametros.Add("claveArticulo", cliente.claveArticulo, DbType.String);
            parametros.Add("numeroPedido", cliente.numeroPedido, DbType.String);
            parametros.Add("TipoConsulta", tipo, DbType.Int16);
            parametros.Add("idDireccion", cliente.idDireccion, DbType.Int64);

            using (var con = new MySqlConnection(_connectionString))
            {
                await con.OpenAsync(); // Abrir la conexión asíncronamente

                // Ejecutar el INSERT y obtener el número de filas afectadas
                int filasAfectadas = await con.ExecuteAsync(query, parametros);

                // Si al menos una fila fue afectada, devolver true
                return filasAfectadas > 0;
            }
        }
    }
}
