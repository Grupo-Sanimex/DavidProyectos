using Dapper;
using MySql.Data.MySqlClient;
using sanimex.webapi.Datos.Comun;
using sanimex.webapi.Dominio.Models;
using sanimex.webapi.Dominio.Models.webMayoreo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeZoneConverter;

namespace sanimex.webapi.Datos.Servicio.WebServicios
{
    public class WebServicio : IWebServicio
    {
        private readonly string _connectionString;

        public WebServicio(IDatos databaseConfig)
        {
            _connectionString = databaseConfig.GetConnectionString();
        }
        public async Task<UsuarioWeb> ObtenerAccesoWeb(int numEmpleado)
        {
            // Consulta SQL directa para obtener el empleado
            string query = "SELECT idUsuario, idPermiso, numEmpleado, nombre, aPaterno, aMaterno, contrasena, fechaCreacion, status FROM UsuariosWebMayoreo WHERE numEmpleado = @numEmpleado";

            // Crear una instancia de DynamicParameters
            var parametros = new DynamicParameters();
            parametros.Add("numEmpleado", numEmpleado, dbType: DbType.Int32); // Usar el nombre correcto del parámetro

            using (var con = new MySqlConnection(_connectionString))
            {
                await con.OpenAsync(); // Abrir la conexión asíncronamente

                // Ejecutar la consulta y recuperar el primer objeto Empleado
                var usuario = await con.QueryFirstOrDefaultAsync<UsuarioWeb>(query, parametros);

                return usuario; // Devolver el objeto Empleado o null si no existe
            }
        }
        public async Task<string> GuardarRol(string perfil)
        {
            try
            {
                TimeZoneInfo mexicoTimeZone = TZConvert.GetTimeZoneInfo("America/Mexico_City");

                DateTime mexicoNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, mexicoTimeZone);

                string query = "INSERT INTO rolPermisosWebMayoreo (perfil, fechaCreacion) VALUES (@perfil, @fechaCreacion)";
                // Crear una instancia de DynamicParameters
                var parametros = new DynamicParameters();
                parametros.Add("perfil", perfil, dbType: DbType.String); // Usar el nombre correcto del parámetro
                parametros.Add("fechaCreacion", mexicoNow, dbType: DbType.DateTime); // Usar el nombre correcto del parámetro
                using (var con = new MySqlConnection(_connectionString))
                {
                    await con.OpenAsync(); // Abrir la conexión asíncronamente
                                           // Ejecutar la consulta y recuperar el primer objeto Empleado
                    await con.ExecuteAsync(query, parametros);
                    return "Rol Guardado"; // Devolver el objeto Empleado o null si no existe
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores (puedes registrarlo si es necesario)
                return "Error al Guardar"; // Devuelve false si ocurre un error
            }
        }
        public async Task<object> listarRoles()
        {
            List<object> roles = new List<object>();
            using (var conexion = new MySqlConnection(_connectionString))
            {
                conexion.Open();
                string query = "select idRol, perfil, fechaCreacion, status from rolPermisosWebMayoreo";
                var cmd = new MySqlCommand(query, conexion);
                cmd.CommandType = CommandType.Text;
                using (var rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        roles.Add(new
                        {
                            idRol = Convert.ToInt32(rd["idRol"]),
                            perfil = rd["perfil"].ToString(),
                            fechaCreacion = rd["fechaCreacion"].ToString(),
                            status = rd["status"].ToString()
                        });
                    }
                }
            }
            return roles;
        }
        public object ListarRol(int idRol)
        {
            object rol = new object();
            using (var conexion = new MySqlConnection(_connectionString))
            {
                conexion.Open();
                string query = "select idRol, perfil, fechaCreacion, status from rolPermisosWebMayoreo where idRol = @idRol;";
                var cmd = new MySqlCommand(query, conexion);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("idRol", idRol);
                using (var rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        rol = (new
                        {
                            idRol = Convert.ToInt32(rd["idRol"]),
                            perfil = rd["perfil"].ToString(),
                            fechaCreacion = rd["fechaCreacion"].ToString(),
                            status = rd["status"].ToString()
                        });
                    }
                }
            }
            return rol;
        }

        public bool BajaRol(int idRol)
        {
            try
            {
                using (var conexion = new MySqlConnection(_connectionString))
                {
                    conexion.Open();
                    string query = "UPDATE rolPermisosWebMayoreo SET status = 0 WHERE idRol = @p_idRol;";
                    var cmd = new MySqlCommand(query, conexion);
                    cmd.Parameters.AddWithValue("@p_idRol", idRol);
                    int filasAfectadas = cmd.ExecuteNonQuery();
                    return filasAfectadas > 0; // Devuelve true si se actualizó al menos una fila
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores (puedes registrarlo si es necesario)
                return false; // Devuelve false si ocurre un error
            }
        }

        // agregar usuario 
        public async Task<string> AgregarUsuarioWeb(int idPermiso, int numEmpleado, string nombre, string aPaterno, string aMaterno, string contrasena)
        {
            try
            {
                TimeZoneInfo mexicoTimeZone = TZConvert.GetTimeZoneInfo("America/Mexico_City");
                DateTime mexicoNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, mexicoTimeZone);
                string query = "INSERT INTO UsuariosWebMayoreo (idPermiso,numEmpleado, nombre, aPaterno, aMaterno, contrasena, fechaCreacion) VALUES (@idPermiso,@numEmpleado, @nombre, @aPaterno, @aMaterno, @contrasena, @fechaCreacion)";
                // Crear una instancia de DynamicParameters
                var parametros = new DynamicParameters();
                parametros.Add("idPermiso", idPermiso, dbType: DbType.Int32); // Usar el nombre correcto del parámetro
                parametros.Add("numEmpleado", numEmpleado, dbType: DbType.Int32); // Usar el nombre correcto del parámetro
                parametros.Add("nombre", nombre, dbType: DbType.String); // Usar el nombre correcto del parámetro
                parametros.Add("aPaterno", aPaterno, dbType: DbType.String); // Usar el nombre correcto del parámetro
                parametros.Add("aMaterno", aMaterno, dbType: DbType.String); // Usar el nombre correcto del parámetro
                parametros.Add("contrasena", contrasena, dbType: DbType.String); // Usar el nombre correcto del parámetro
                parametros.Add("fechaCreacion", mexicoNow, dbType: DbType.DateTime); // Usar el nombre correcto del parámetro
                using (var con = new MySqlConnection(_connectionString))
                {
                    await con.OpenAsync(); // Abrir la conexión asíncronamente
                                           // Ejecutar la consulta y recuperar el primer objeto Empleado
                    await con.ExecuteAsync(query, parametros);
                    return "Usuario Guardado"; // Devolver el objeto Empleado o null si no existe
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores (puedes registrarlo si es necesario)
                return "Error al Guardar"; // Devuelve false si ocurre un error
            }

        }
        //public async Task<string> ActualizarUsuarioWeb(int idUsuario, int numEmpleado, string nombre, string aPaterno, string aMaterno, string contrasena, int idRol)
        //{
        //    try
        //    {
        //        TimeZoneInfo mexicoTimeZone = TZConvert.GetTimeZoneInfo("America/Mexico_City");
        //        DateTime mexicoNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, mexicoTimeZone);
        //        string query = "UPDATE UsuariosWebMayoreo SET numEmpleado = @numEmpleado, nombre = @nombre, aPaterno = @aPaterno, aMaterno = @aMaterno, contrasena = @contrasena, fechaCreacion = @fechaCreacion, idRol = @idRol WHERE idUsuario = @idUsuario";
        //        // Crear una instancia de DynamicParameters
        //        var parametros = new DynamicParameters();
        //        parametros.Add("numEmpleado", numEmpleado, dbType: DbType.Int32); // Usar el nombre correcto del parámetro
        //        parametros.Add("nombre", nombre, dbType: DbType.String); // Usar el nombre correcto del parámetro
        //        parametros.Add("aPaterno", aPaterno, dbType: DbType.String); // Usar el nombre correcto del parámetro
        //        parametros.Add("aMaterno", aMaterno, dbType: DbType.String); // Usar el nombre correcto del parámetro
        //        parametros.Add("contrasena", contrasena, dbType: DbType.String); // Usar el nombre correcto del parámetro
        //        parametros.Add("fechaCreacion", mexicoNow, dbType: DbType.DateTime); // Usar el nombre correcto del parámetro
        //        parametros.Add("idRol", idRol, dbType: DbType.Int32); // Usar el nombre correcto del parámetro
        //        using (var con = new MySqlConnection(_connectionString))
        //        {
        //            await con.OpenAsync(); // Abrir la conexión asíncronamente
        //                                   // Ejecutar la consulta y recuperar el primer objeto Empleado
        //            await con.ExecuteAsync(query, parametros);
        //            return "Usuario Actualizado"; // Devolver el objeto Empleado o null si no existe
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Manejo de errores (puedes registrarlo si es necesario)
        //        return "Error al Actualizar"; // Devuelve false si ocurre un error
        //    }

        //}
        public async Task<string> BajaUsuarioWeb(int idUsuario)
        {
            try
            {
                string query = "UPDATE UsuariosWebMayoreo SET status = 0 WHERE idUsuario = @idUsuario;";
                // Crear una instancia de DynamicParameters
                var parametros = new DynamicParameters();
                parametros.Add("idUsuario", idUsuario, dbType: DbType.Int32); // Usar el nombre correcto del parámetro
                using (var con = new MySqlConnection(_connectionString))
                {
                    await con.OpenAsync(); // Abrir la conexión asíncronamente
                                           // Ejecutar la consulta y recuperar el primer objeto Empleado
                    await con.ExecuteAsync(query, parametros);
                    return "Usuario dado de Baja"; // Devolver el objeto Empleado o null si no existe
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores (puedes registrarlo si es necesario)
                return "Error al dar de Baja"; // Devuelve false si ocurre un error
            }
        }

        public object ListarUsuarios()
        {
            List<object> usuarios = new List<object>();
            using (var conexion = new MySqlConnection(_connectionString))
            {
                conexion.Open();
                string query = "select idUsuario, idPermiso, numEmpleado, nombre, aPaterno, aMaterno, contrasena, fechaCreacion, status from UsuariosWebMayoreo";
                var cmd = new MySqlCommand(query, conexion);
                cmd.CommandType = CommandType.Text;
                using (var rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        usuarios.Add(new
                        {
                            idUsuario = Convert.ToInt32(rd["idUsuario"]),
                            idPermiso = Convert.ToInt32(rd["idPermiso"]),
                            numEmpleado = Convert.ToInt32(rd["numEmpleado"]),
                            nombre = rd["nombre"].ToString(),
                            aPaterno = rd["aPaterno"].ToString(),
                            aMaterno = rd["aMaterno"].ToString(),
                            contrasena = rd["contrasena"].ToString(),
                            fechaCreacion = rd["fechaCreacion"].ToString(),
                            status = rd["status"].ToString()
                        });
                    }
                }
            }
            return usuarios;
        }

        public object ListarUsuario(int idUsuario)
        {
            object usuario = new object();
            using (var conexion = new MySqlConnection(_connectionString))
            {
                conexion.Open();
                string query = "select idUsuario, idPermiso, numEmpleado, nombre, aPaterno, aMaterno, contrasena, fechaCreacion, status from UsuariosWebMayoreo where idUsuario = @idUsuario;";
                var cmd = new MySqlCommand(query, conexion);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("idUsuario", idUsuario);
                using (var rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        usuario = (new
                        {
                            idUsuario = Convert.ToInt32(rd["idUsuario"]),
                            idPermiso = Convert.ToInt32(rd["idPermiso"]),
                            numEmpleado = Convert.ToInt32(rd["numEmpleado"]),
                            nombre = rd["nombre"].ToString(),
                            aPaterno = rd["aPaterno"].ToString(),
                            aMaterno = rd["aMaterno"].ToString(),
                            contrasena = rd["contrasena"].ToString(),
                            fechaCreacion = rd["fechaCreacion"].ToString(),
                            status = rd["status"].ToString()
                        });
                    }
                }
            }
            return usuario;
        }
    }
}