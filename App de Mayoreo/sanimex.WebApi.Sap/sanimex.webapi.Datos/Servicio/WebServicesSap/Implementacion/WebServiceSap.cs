using Dapper;
using MySql.Data.MySqlClient;
using sanimex.webapi.Datos.Comun;
using sanimex.webapi.Datos.Servicio.WebServicesSap.Interfaces;
using sanimex.webapi.Dominio.Models;
using sanimex.webapi.Dominio.Models.Sucursales;
using sanimex.webapi.Dominio.Models.WebServiceSap;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sanimex.webapi.Datos.Servicio.WebServicesSap.Implementacion
{
    public class WebServiceSap : IWebServiceSap
    {
        private readonly string _connectionString;
        public WebServiceSap(IDatos databaseConfig)
        {
            _connectionString = databaseConfig.GetConnectionString();
        }
        public async Task<Webservice> AccesoWebservice(string nombreServicio)
        {
            // Consulta SQL directa para obtener el empleado
            string query = "Select IFNULL(rutaNet,'')rutaNet, IFNULL(rutaMobil,'')rutaMobil,usuario,pwd FROM ConfiguracionWS WHERE nombre = @nombreServicio AND Status=1"; // Asegúrate de que el nombre de la tabla y los campos sean correctos

            // Crear una instancia de DynamicParameters
            var parametros = new DynamicParameters();
            parametros.Add("nombreServicio", nombreServicio, dbType: DbType.String); // Usar el nombre correcto del parámetro

            using (var con = new MySqlConnection(_connectionString))
            {
                await con.OpenAsync(); // Abrir la conexión asíncronamente

                // Ejecutar la consulta y recuperar el primer objeto Empleado
                var webservice = await con.QueryFirstOrDefaultAsync<Webservice>(query, parametros);

                return webservice; // Devolver el objeto Empleado o null si no existe
            }
        }
        // sucursales hijo de un centro corredor
        public async Task<string> CorredorCentro(string centro)
        {
            // Consulta SQL directa para obtener el empleado
            string query = "SELECT sucHijoSap FROM SucursalTraslados WHERE sucPadre = @centro"; // Asegúrate de que el nombre de la tabla y los campos sean correctos

            // Crear una instancia de DynamicParameters
            var parametros = new DynamicParameters();
            parametros.Add("centro", centro, dbType: DbType.String); // Usar el nombre correcto del parámetro

            using (var con = new MySqlConnection(_connectionString))
            {
                await con.OpenAsync(); // Abrir la conexión asíncronamente

                string? sucHijos = await con.QueryFirstOrDefaultAsync<string>(query, parametros);

                return sucHijos!;
            }
        }
    }
}
