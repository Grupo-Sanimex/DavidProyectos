using Dapper;
using MySql.Data.MySqlClient;
using sanimex.webapi.Datos.Comun;
using sanimex.webapi.Dominio.Models;
using sanimex.webapi.Dominio.Models.Producto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sanimex.webapi.Datos.Servicio.ProductoServicio.Implementacion
{
    public class ProductoService: IProductoService
    {
        private readonly string _connectionString;

        public ProductoService(IDatos databaseConfig)
        {
            _connectionString = databaseConfig.GetConnectionString();
        }
        public async Task<List<BusquedaProducto>> ObtenerProducto(string busqueda, int limite) // Método asíncrono que devuelve un objeto Empleado
        {
            string query = "ElasticSearch"; // Nombre del procedimiento almacenado
            var parametros = new DynamicParameters(); // Crear una instancia de DynamicParameters
            parametros.Add("TextoBuscar", busqueda, dbType: DbType.String); // Asegúrate de usar el nombre correcto del parámetro
            parametros.Add("limite", limite, dbType: DbType.Int32); // Asegúrate de usar el nombre correcto del parámetro

            using (var con = new MySqlConnection(_connectionString))
            {
                await con.OpenAsync(); // Abrir la conexión asíncronamente
                // Ejecutar el procedimiento almacenado y recuperar el primer objeto Empleado
                var lista = await con.QueryAsync<BusquedaProducto>(query, parametros, commandType: CommandType.StoredProcedure);

                return lista.ToList(); // Devolver el objeto Empleado o null si no existe
            }
        }
        public async Task<MProducto> Producto(string CodigoBarra) // Método asíncrono que devuelve un objeto Empleado
        {
            // Consulta SQL directa para obtener el empleado
            string query = "SELECT `Sanimex.Product.Code` AS CodigoBarra, `Sanimex.Product.Description` AS Descripcion, `Sanimex.ClassificationProduct.Provider` AS Proveedor, `Sanimex.Sales.Prices.RetailPrice` AS Precio FROM productos WHERE `Sanimex.Product.Code` = @CodigoBarra"; // Asegúrate de que el nombre de la tabla y los campos sean correctos

            // Crear una instancia de DynamicParameters
            var parametros = new DynamicParameters();
            parametros.Add("CodigoBarra", CodigoBarra, dbType: DbType.String); // Usar el nombre correcto del parámetro

            using (var con = new MySqlConnection(_connectionString))
            {
                await con.OpenAsync(); // Abrir la conexión asíncronamente

                // Ejecutar la consulta y recuperar el primer objeto Empleado
                var usuario = await con.QueryFirstOrDefaultAsync<MProducto>(query, parametros);

                return usuario; // Devolver el objeto Empleado o null si no existe
            }
        }

        public async Task<DataSet> GetResultadosElasticSearchAsync(string textoBuscar, string proveedor = "todos", string color = "todos", string formato = "todos")
        {
            int size = 8;
            var response = new DataSet("Respuesta");

            // Parámetros para ambos procedimientos almacenados
            var parametros = new DynamicParameters();
            parametros.Add("textoBuscar", textoBuscar, DbType.String);
            parametros.Add("limite", size, DbType.Int32);
            parametros.Add("proveedor", proveedor, DbType.String);
            parametros.Add("color", color, DbType.String);
            parametros.Add("formato", formato, DbType.String);

            try
            {
                using (var con = new MySqlConnection(_connectionString))
                {
                    await con.OpenAsync();

                    // Ejecutar el primer procedimiento almacenado y cargar los datos manualmente en DataTable
                    var resdData = await con.QueryAsync("getResultadosElasticSearch", parametros, commandType: CommandType.StoredProcedure);
                    var resdTable = ConvertToDataTable(resdData);
                    resdTable.TableName = "resd"; // Asignar el nombre de la tabla
                    response.Tables.Add(resdTable);

                    // Ejecutar el segundo procedimiento almacenado y cargar los datos manualmente en DataTable
                    var tbFiltrosData = await con.QueryAsync("Filtros", parametros, commandType: CommandType.StoredProcedure);
                    var tbFiltrosTable = ConvertToDataTable(tbFiltrosData);
                    tbFiltrosTable.TableName = "filtros"; // Asignar el nombre de la tabla
                    response.Tables.Add(tbFiltrosTable);
                }

                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al armar dataset: {ex.Message}");
                return null;
            }
        }


        // Método auxiliar para convertir IEnumerable<dynamic> a DataTable
        private DataTable ConvertToDataTable(IEnumerable<dynamic> data)
        {
            var dataTable = new DataTable();

            // Obtener las columnas de la primera fila
            if (data.Any())
            {
                var firstRow = (IDictionary<string, object>)data.First();
                foreach (var column in firstRow.Keys)
                {
                    dataTable.Columns.Add(column);
                }

                // Rellenar las filas
                foreach (var row in data)
                {
                    var dataRow = dataTable.NewRow();
                    foreach (var column in firstRow.Keys)
                    {
                        dataRow[column] = ((IDictionary<string, object>)row)[column] ?? DBNull.Value;
                    }
                    dataTable.Rows.Add(dataRow);
                }
            }
            return dataTable;
        }


        public async Task<string> Nombre_Cliente_Cotiza_May(string idCliente)
        {
            string query = "SELECT razonSocial FROM GAM_clientes WHERE idclienteSAP = @idCliente AND estatus = 1";
            var parametros = new DynamicParameters();
            parametros.Add("idCliente", idCliente, DbType.String);

            try
            {
                using (var con = new MySqlConnection(_connectionString))
                {
                    await con.OpenAsync();
                    var razonSocial = await con.QueryFirstOrDefaultAsync<string>(query, parametros);

                    return razonSocial;
                }
            }
            catch (Exception ex)
            {
                return $"Error al obtener razonSocial: {ex.Message}";
            }
        }
        public async Task<string> Tipo_Cliente_Clas_May(string idCliente)
        {
            string query = "SELECT Tipo FROM GAM_clientes_Clasificacion where idclienteSAP = @idCliente AND Status='A'";
            var parametros = new DynamicParameters();
            parametros.Add("idCliente", idCliente, DbType.String);

            try
            {
                using (var con = new MySqlConnection(_connectionString))
                {
                    await con.OpenAsync();
                    var razonSocial = await con.QueryFirstOrDefaultAsync<string>(query, parametros);

                    return razonSocial;
                }
            }
            catch (Exception ex)
            {
                return $"Error al obtener Clasificacion: {ex.Message}";
            }
        }
        public async Task<int> Productos_Bloqueados_NC(string Codebar, string claveSap)
        {
            string query = "Select Count(*) FROM productos_NC WHERE codigoBarras= @Codebar AND Status='A' AND CASE WHEN sucursalesAplica <> 'Todas' THEN FIND_IN_SET(@claveSap,sucursalesAplica)>0 ELSE 1=1 END";
            var parametros = new DynamicParameters();
            parametros.Add("Codebar", Codebar, DbType.String);
            parametros.Add("claveSap", claveSap, DbType.String);

            try
            {
                using (var con = new MySqlConnection(_connectionString))
                {
                    await con.OpenAsync();
                    var Productos_Bloqueados_NC = await con.QueryFirstOrDefaultAsync<string>(query, parametros);

                    return Convert.ToInt32(Productos_Bloqueados_NC);
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<decimal> Descuentos_Especiales_Mayoreo(string Tipo_Cliente_Clasificado, string Codebar, string Tipo_Entrega, string ClaveSap)
        {
            string query = "";
            if (Tipo_Entrega == "Recoge")
            {
                Tipo_Cliente_Clasificado = "Recoge_" + Tipo_Cliente_Clasificado;
                query = $@"SELECT CASE WHEN FIND_IN_SET('{ClaveSap}', sucursalesAplica) > 0 THEN 0.04 ELSE {Tipo_Cliente_Clasificado} END as Recoge FROM Descuentos_Especiales_May WHERE codigoBarras = '{Codebar}';";

            }
            else if (Tipo_Entrega == "Contado")
            {
                Tipo_Cliente_Clasificado = "Contado_" + Tipo_Cliente_Clasificado;
                query = $@"
    SELECT CASE 
        WHEN FIND_IN_SET('{ClaveSap}', sucursalesAplica) > 0 THEN 0.02 
        ELSE `{Tipo_Cliente_Clasificado}` 
    END as Descuento
    FROM Descuentos_Especiales_May 
    WHERE codigoBarras = '{Codebar}';
";

            }

            var parametros = new DynamicParameters();
            parametros.Add("Codebar", Codebar, DbType.String);

            try
            {
                using (var con = new MySqlConnection(_connectionString))
                {
                    await con.OpenAsync();
                    var resultado = await con.QueryFirstOrDefaultAsync<string>(query, parametros);

                    // Verificar el resultado obtenido
                    Console.WriteLine($"Resultado de la consulta: {resultado}");

                    // Intentar convertir el resultado a decimal
                    if (decimal.TryParse(resultado?.Trim(), out decimal descuento))
                    {
                        return descuento;
                    }
                    return 0; // Valor por defecto si no se puede convertir
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores
                Console.WriteLine($"Error: {ex.Message}");
                return 0; // Devuelve 0 en caso de error
            }
        }

        public async Task<float> MetroXCaja(string Codebar)
        {
            string query = "select `Sanimex.FeatureProduct.SquareMeter` AS metroXCaja from productos where `Sanimex.Product.Code` = @Codebar;";
            var parametros = new DynamicParameters();
            parametros.Add("Codebar", Codebar, DbType.String);

            try
            {
                using (var con = new MySqlConnection(_connectionString))
                {
                    await con.OpenAsync();
                    var metroxCaja = await con.QueryFirstOrDefaultAsync<string>(query, parametros);
                    return Convert.ToSingle(metroxCaja);
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

    }
}
