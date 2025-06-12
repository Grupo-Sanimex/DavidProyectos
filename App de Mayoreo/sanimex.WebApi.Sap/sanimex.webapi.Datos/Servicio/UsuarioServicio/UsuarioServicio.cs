using Dapper;
using MySql.Data.MySqlClient;
using sanimex.webapi.Datos.Comun;
using sanimex.webapi.Dominio.Models;
using sanimex.webapi.Dominio.Models.Usuarios;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sanimex.webapi.Datos.Servicio.UsuarioServicio
{
    public class UsuarioServicio : IUsuarioServicio
    {
        private readonly string _connectionString;

        public UsuarioServicio(IDatos databaseConfig)
        {
            _connectionString = databaseConfig.GetConnectionString();
        }
        public async Task<Usuario> ObtenerAcceso(int id) // Método asíncrono que devuelve un objeto Empleado
        {
            // Consulta SQL directa para obtener el empleado
            string query = "SELECT idUsuario,idUsuario as _id, idSucursal, idPermiso, idRol, numEmpleado, correo, nombre, aPaterno, contrasena, status FROM Usuarios WHERE numEmpleado = @id"; // Asegúrate de que el nombre de la tabla y los campos sean correctos

            // Crear una instancia de DynamicParameters
            var parametros = new DynamicParameters();
            parametros.Add("id", id, dbType: DbType.Int32); // Usar el nombre correcto del parámetro

            using (var con = new MySqlConnection(_connectionString))
            {
                await con.OpenAsync(); // Abrir la conexión asíncronamente

                // Ejecutar la consulta y recuperar el primer objeto Empleado
                var usuario = await con.QueryFirstOrDefaultAsync<Usuario>(query, parametros);

                return usuario; // Devolver el objeto Empleado o null si no existe
            }
        }
        public async Task<Usuario> Perfil(string id) // Método asíncrono que devuelve un objeto Empleado
        {
            // Consulta SQL directa para obtener el empleado
            string query = "SELECT idUsuario, idUsuario as _id, idSucursal,idPermiso, idRol, numEmpleado, correo, nombre, aPaterno, telefono, status FROM Usuarios WHERE idUsuario = @id"; // Asegúrate de que el nombre de la tabla y los campos sean correctos

            // Crear una instancia de DynamicParameters
            var parametros = new DynamicParameters();
            parametros.Add("id", id, dbType: DbType.String); // Usar el nombre correcto del parámetro

            using (var con = new MySqlConnection(_connectionString))
            {
                await con.OpenAsync(); // Abrir la conexión asíncronamente

                // Ejecutar la consulta y recuperar el primer objeto Empleado
                var usuario = await con.QueryFirstOrDefaultAsync<Usuario>(query, parametros);

                return usuario; // Devolver el objeto Empleado o null si no existe
            }
        }
        public async Task<RolUsuario> Roles(int id) // Método asíncrono que devuelve un objeto Empleado
        {
            // Consulta SQL directa para obtener el empleado
            string query = "SELECT idPermiso, idRol, idSucursal FROM Usuarios WHERE idUsuario = @id"; // Asegúrate de que el nombre de la tabla y los campos sean correctos

            // Crear una instancia de DynamicParameters
            var parametros = new DynamicParameters();
            parametros.Add("id", id, dbType: DbType.String); // Usar el nombre correcto del parámetro

            using (var con = new MySqlConnection(_connectionString))
            {
                await con.OpenAsync(); // Abrir la conexión asíncronamente

                // Ejecutar la consulta y recuperar el primer objeto Empleado
                var usuario = await con.QueryFirstOrDefaultAsync<RolUsuario>(query, parametros);

                return usuario; // Devolver el objeto Empleado o null si no existe
            }
        }
        public async Task<int> RolePermisos(int id) // Método asíncrono que devuelve un objeto Empleado
        {
            // Consulta SQL directa para obtener el empleado
            string query = "SELECT nivelRol FROM RolPermisos WHERE idRol = @id AND status = 1"; // Asegúrate de que el nombre de la tabla y los campos sean correctos

            // Crear una instancia de DynamicParameters
            var parametros = new DynamicParameters();
            parametros.Add("id", id, dbType: DbType.String); // Usar el nombre correcto del parámetro

            using (var con = new MySqlConnection(_connectionString))
            {
                await con.OpenAsync(); // Abrir la conexión asíncronamente

                // Ejecutar la consulta y recuperar el primer objeto Empleado
                int nivelRol = await con.QueryFirstOrDefaultAsync<int?>(query, parametros) ?? 0;

                return nivelRol; // Devolver el objeto Empleado o null si no existe
            }
        }

        public string GuardarUsuarioApp(int numEmpleado, DateTime fechaInicio, TimeSpan horaInicio)
        {
            using (var conexion = new MySqlConnection(_connectionString))
            {
                conexion.Open();
                string query = "INSERT INTO validarUsuarioApp (numEmpleado, fechaInicio, horaInicio) VALUES(@numEmpleado, @fechaInicio, @horaInicio)";
                var cmd = new MySqlCommand(query, conexion);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("numEmpleado", numEmpleado);
                cmd.Parameters.AddWithValue("fechaInicio", fechaInicio);
                cmd.Parameters.AddWithValue("horaInicio", horaInicio);
                cmd.ExecuteNonQuery();
            }
            return "Usuario guardado correctamente";
        }

        public string DesactivarUsuarioApp(int numEmpleado)
        {
            using (var conexion = new MySqlConnection(_connectionString))
            {
                conexion.Open();
                string query = "UPDATE validarUsuarioApp SET status = 0 WHERE numEmpleado = @numEmpleado";
                var cmd = new MySqlCommand(query, conexion);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("numEmpleado", numEmpleado);
                cmd.ExecuteNonQuery();
            }
            return "Usuario desactivado solicita activacion al area de Desarrollo";
        }
        public string ActivarUsuarioApp(int numEmpleado)
        {
            using (var conexion = new MySqlConnection(_connectionString))
            {
                conexion.Open();
                string query = "UPDATE validarUsuarioApp SET status = 1 WHERE numEmpleado = @numEmpleado";
                var cmd = new MySqlCommand(query, conexion);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("numEmpleado", numEmpleado);
                cmd.ExecuteNonQuery();
            }
            return "Usuario activado correctamente";
        }
        public string ValidarUsuarioApp(int numEmpleado)
        {
            using (var conexion = new MySqlConnection(_connectionString))
            {
                conexion.Open();
                string query = "SELECT status FROM validarUsuarioApp WHERE numEmpleado = @numEmpleado";
                var cmd = new MySqlCommand(query, conexion);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("numEmpleado", numEmpleado);
                var status = cmd.ExecuteScalar();
                if (status != null)
                {
                    return status.ToString();
                }
                else
                {
                    return "Usuario no encontrado";
                }
            }
        }
    }
}
