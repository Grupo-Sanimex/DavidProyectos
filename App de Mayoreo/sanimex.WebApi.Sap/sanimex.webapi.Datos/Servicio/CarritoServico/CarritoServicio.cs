using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Utilities;
using sanimex.webapi.Datos.Comun;
using sanimex.webapi.Datos.Servicio.UsuarioServicio;
using sanimex.webapi.Dominio.Models;
using sanimex.webapi.Dominio.Models.Carrito;
using sanimex.webapi.Dominio.Models.Logs;
using sanimex.webapi.Dominio.Models.Usuarios;
using TimeZoneConverter;

namespace sanimex.webapi.Datos.Servicio.CarritoServico
{
    public class CarritoServicio : ICarritoServicio
    {
        private readonly string _connectionString;
        private readonly IUsuarioServicio _usuarioServicio;

        public CarritoServicio(IDatos databaseConfig, IUsuarioServicio usuarioServicio)
        {
            _connectionString = databaseConfig.GetConnectionString();
            _usuarioServicio = usuarioServicio;
        }
        public async Task<int> GuardarCarro(string idUsuario, List<Carro> carros)
        {
            // Obtener la zona horaria de Ciudad de México
            TimeZoneInfo mexicoTimeZone = TZConvert.GetTimeZoneInfo("America/Mexico_City");

            // Obtener la fecha y hora actual en UTC y convertirla a la zona horaria de México
            DateTime mexicoNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, mexicoTimeZone);

            // Calcular el total de precioFinal
            float totalPrecioFinal = carros.Sum(c => c.precioFinal * c.cantidad);
            string sucursal = carros.FirstOrDefault()?.sucursal!;
            string idClienteSAP = carros.FirstOrDefault()?.ClaveCliente!;
            string tipo_consulta = carros.FirstOrDefault()?.tipo_consulta!;

            int idSucursal = await ObtenerIdSucursal(sucursal);

            if (totalPrecioFinal != 0 && idSucursal != 0 && idClienteSAP != null)
            {
                //int numEmpleado = await ObtenerNumEmpleado(Convert.ToInt32(idUsuario));
                // Consulta SQL para realizar el INSERT y obtener el ID generado
                string query = @"
            INSERT INTO CotizacionMaster (idDispositivo, idSucursal, Status, fechaAlta, totalCotizacion, idClienteSAP, tipo_consulta)
            VALUES (@idDispositivo, @idSucursal, @Status, @fechaAlta, @totalCotizacion, @idClienteSAP, @tipo_consulta);
            SELECT LAST_INSERT_ID();";

                // Crear una instancia de DynamicParameters
                var parametros = new DynamicParameters();
                parametros.Add("idDispositivo", idUsuario, DbType.String);
                parametros.Add("idSucursal", idSucursal, DbType.Int32);
                parametros.Add("Status", "A", DbType.String);
                parametros.Add("fechaAlta", mexicoNow, DbType.DateTime);
                parametros.Add("totalCotizacion", totalPrecioFinal, DbType.Decimal);
                parametros.Add("idClienteSAP", idClienteSAP, DbType.Decimal);
                parametros.Add("tipo_consulta", Convert.ToInt16(tipo_consulta), DbType.Int16);

                using (var con = new MySqlConnection(_connectionString))
                {
                    await con.OpenAsync(); // Abrir la conexión asíncronamente

                    // Ejecutar el INSERT y obtener el ID generado
                    var idGenerado = await con.ExecuteScalarAsync<int>(query, parametros);
                    // Devolver el ID generado
                    return idGenerado;
                }
            }
            else
            {
                return 0;
            }

        }
        // Método para insertar una lista de carros y calcular el total
        public async Task<bool> GuardarCodigoAsync(int idGenerado, List<Carro> carros)
        {
            if (carros == null || !carros.Any())
            {
                return false;
            }

            using (IDbConnection con = new MySqlConnection(_connectionString))
            {
                con.Open();
                if (con.State != ConnectionState.Open)
                {
                    return false;
                }

                using (var transaction = con.BeginTransaction())
                {
                    try
                    {
                        foreach (var carro in carros)
                        {
                            var query = @"
                        INSERT INTO cotizacionDetalle (idCotizacion, codebar, cantidad, status, precioUnitario)
                        VALUES (@idGenerado, @codigo, @cantidad, @status, @precioUnitario)";

                            int rowsAffected = await con.ExecuteAsync(query, new
                            {
                                idGenerado = idGenerado,
                                codigo = carro.codigo,
                                cantidad = carro.cantidad,
                                status = "A",
                                precioUnitario = carro.precioFinal
                            }, transaction);

                            if (rowsAffected != 1)
                            {
                                transaction.Rollback();
                                return false;
                            }
                        }
                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }
        //public async Task<bool> GuardarCodigoAsync(int idGenerado, List<Carro> carros)
        //{
        //    using (IDbConnection con = new MySqlConnection(_connectionString))
        //    {
        //        con.Open();
        //        try
        //        {
        //            foreach (var carro in carros)
        //            {
        //                var query = @"
        //                INSERT INTO cotizacionDetalle (idCotizacion, codebar, cantidad, status, precioUnitario)
        //                VALUES (@idGenerado, @codigo, @cantidad, @status, @precioUnitario)";

        //                int rowsAffected = await con.ExecuteAsync(query, new
        //                {
        //                    idGenerado = idGenerado,
        //                    codigo = carro.codigo,
        //                    cantidad = carro.cantidad,
        //                    status = "A",
        //                    precioUnitario = carro.precioFinal
        //                });
        //            }
        //            return true;

        //        }
        //        catch (Exception ex)
        //        {
        //            return false;
        //        }
        //     }
        //}
        


        public async Task<int> ObtenerIdSucursal(string claveSap) // Método asíncrono que devuelve un objeto Empleado
        {
            // Consulta SQL directa para obtener el empleado
            string query = "SELECT idSucursal FROM Sucursales WHERE idSAP = @claveSap"; // Asegúrate de que el nombre de la tabla y los campos sean correctos

            // Crear una instancia de DynamicParameters
            var parametros = new DynamicParameters();
            parametros.Add("claveSap", claveSap, dbType: DbType.String); // Usar el nombre correcto del parámetro

            using (var con = new MySqlConnection(_connectionString))
            {
                await con.OpenAsync(); // Abrir la conexión asíncronamente

                // Ejecutar la consulta y recuperar el primer objeto Empleado
                var idSucursal = await con.QueryFirstOrDefaultAsync<int>(query, parametros);

                return idSucursal!; // Devolver el objeto Empleado o null si no existe
            }
        }
        public async Task<int> ObtenerNumEmpleado(int idUsuario) // Método asíncrono que devuelve un objeto Empleado
        {
            // Consulta SQL directa para obtener el empleado
            string query = "SELECT numEmpleado FROM Usuarios WHERE idUsuario = @idUsuario"; // Asegúrate de que el nombre de la tabla y los campos sean correctos

            // Crear una instancia de DynamicParameters
            var parametros = new DynamicParameters();
            parametros.Add("idUsuario", idUsuario, dbType: DbType.Int32); // Usar el nombre correcto del parámetro

            using (var con = new MySqlConnection(_connectionString))
            {
                await con.OpenAsync(); // Abrir la conexión asíncronamente

                // Ejecutar la consulta y recuperar el primer objeto Empleado
                var idSucursal = await con.QueryFirstOrDefaultAsync<int>(query, parametros);

                return idSucursal!; // Devolver el objeto Empleado o null si no existe
            }
        }

        public async Task<List<HisCotizaGerenteSucursal>> HisCotizaGerenteSucursal(string idUsuario, string fechaConsulta)
        {
            string query = "SELECT DISTINCT s.idSAP AS claveSucursal from SupervisorSucursales ss " +
                "INNER JOIN Sucursales s ON ss.idSucursal = s.idSucursal " +
                "INNER JOIN CotizacionMaster cm ON s.idSucursal = cm.idSucursal " +
                "WHERE ss.idUsuario = @idUsuario and DATE(cm.fechaAlta) = @fechaConsulta and ss.status = 1;";
            using (var con = new MySqlConnection(_connectionString))
            {
                await con.OpenAsync(); // Abrir la conexión asíncronamente
                var parametros = new DynamicParameters();
                parametros.Add("idUsuario", idUsuario, DbType.String);
                parametros.Add("fechaConsulta", fechaConsulta, DbType.String);
                var historialCarrito = await con.QueryAsync<HisCotizaGerenteSucursal>(query, parametros);
                return historialCarrito.ToList(); // Devolver la lista de historial de carrito
            }
        }
        public async Task<List<HisCtoMasterGerente>> ListarHistorialCMasterGerente(string idUsuario, string fechaConsulta)
        {
            string query = "SELECT DISTINCT cm.idDispositivo , u.nombre, u.aPaterno, u.aMaterno, u.numEmpleado FROM CotizacionMaster cm INNER JOIN Sucursales s ON cm.idSucursal = s.idSucursal " +
                "INNER JOIN Usuarios u ON cm.idDispositivo = u.idUsuario " +
                "INNER JOIN SupervisorSucursales sp ON cm.idSucursal = sp.idSucursal " +
                "WHERE sp.idUsuario = @idUsuario AND DATE(cm.fechaAlta) = @fechaConsulta;";

            using (var con = new MySqlConnection(_connectionString))
            {
                await con.OpenAsync(); // Abrir la conexión asíncronamente

                var parametros = new DynamicParameters();
                parametros.Add("idUsuario", idUsuario, DbType.String);
                parametros.Add("fechaConsulta", fechaConsulta, DbType.String);

                var historialCarrito = await con.QueryAsync<HisCtoMasterGerente>(query, parametros);

                return historialCarrito.ToList();
            }
        }

        public async Task<List<HisCtoMaster>> ListarHistorialCMaster(string idUsuario, string fechaConsulta)
        {
            string query = "SELECT idCotizacion, idClienteSAP, totalCotizacion, Status, DATE_FORMAT(fechaAlta, '%Y-%m-%d') AS fecha," +
                " DATE_FORMAT(fechaAlta, '%H:%i:%s') AS hora, idventa FROM CotizacionMaster WHERE DATE(fechaAlta) = @fechaConsulta AND idDispositivo = @idUsuario";
            using (var con = new MySqlConnection(_connectionString))
            {
                await con.OpenAsync();

                var parametros = new DynamicParameters();
                parametros.Add("idUsuario", idUsuario, DbType.String);
                parametros.Add("fechaConsulta", fechaConsulta, DbType.String);

                var historialCarrito = await con.QueryAsync<HisCtoMaster>(query, parametros);

                return historialCarrito.ToList();
            }
        }


        public async Task<List<CotizacionDetalle>> CotizacionDetalle(string idUsuario, string idCotizacion)
        {
            RolUsuario rolUsuario = await _usuarioServicio.Roles(Convert.ToInt32(idUsuario));
            if (rolUsuario.idRol == 8 || rolUsuario.idRol == 10 || rolUsuario.idRol == 1)
            {
                string query = "SELECT cd.codebar, p.`Sanimex.Product.Description` AS description, cd.cantidad, cd.precioUnitario ,cm.Status FROM cotizacionDetalle cd " +
                   "INNER JOIN productos p ON cd.codebar = p.`Sanimex.Product.Code` " +
                   "INNER JOIN CotizacionMaster cm ON cd.idCotizacion = cm.idCotizacion " +
                   "WHERE cd.idCotizacion = @idCotizacion;";
                using (var con = new MySqlConnection(_connectionString))
                {
                    await con.OpenAsync();

                    var parametros = new DynamicParameters();
                    parametros.Add("idCotizacion", idCotizacion, DbType.String);

                    var historialCarrito = await con.QueryAsync<CotizacionDetalle>(query, parametros);

                    return historialCarrito.ToList();
                }
            }
            else
            {
                string query = "SELECT cd.codebar, p.`Sanimex.Product.Description` AS description, cd.cantidad, cd.precioUnitario ,cm.Status FROM cotizacionDetalle cd " +
                    "INNER JOIN productos p ON cd.codebar = p.`Sanimex.Product.Code` " +
                    "INNER JOIN CotizacionMaster cm ON cd.idCotizacion = cm.idCotizacion " +
                    "WHERE cd.idCotizacion = @idCotizacion;";
                using (var con = new MySqlConnection(_connectionString))
                {
                    await con.OpenAsync();

                    var parametros = new DynamicParameters();
                    parametros.Add("idCotizacion", idCotizacion, DbType.String);

                    var historialCarrito = await con.QueryAsync<CotizacionDetalle>(query, parametros);

                    return historialCarrito.ToList();
                }
            }
        }

        public async Task<ConsultaCliente?> ConsultaCliente(string idCotizacion)
        {
            string query = "SELECT cd.codebar, cm.idClienteSAP AS claveCliente, s.idSAP AS centrosCorredor FROM CotizacionMaster cm " +
                "INNER JOIN Sucursales s ON cm.idSucursal = s.idSucursal " +
                "INNER JOIN cotizacionDetalle cd ON cm.idCotizacion = cd.idCotizacion " +
                "where cm.idCotizacion = @idCotizacion;";
            using (var con = new MySqlConnection(_connectionString))
            {
                await con.OpenAsync();
                var parametros = new DynamicParameters();
                parametros.Add("idCotizacion", idCotizacion, DbType.String);
                var historialCarrito = await con.QueryFirstOrDefaultAsync<ConsultaCliente>(query, parametros);
                return historialCarrito;
            }
        }

    }
}
