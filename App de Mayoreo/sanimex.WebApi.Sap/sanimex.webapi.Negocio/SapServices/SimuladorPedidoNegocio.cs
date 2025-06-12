using Microsoft.Extensions.Configuration;
using sanimex.webapi.Datos.Servicio.SucursalService;
using sanimex.webapi.Dominio.Models.Sucursales;
using sanimex.webapi.Dominio.Models.WebServiceSap;
using sanimex.webapi.Negocio.WebServiceSap;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace sanimex.webapi.Negocio.SapServices
{
    public class SimuladorPedidoNegocio: ISimuladorPedidoNegocio
    {
        private readonly IWebServiceNegocio _webServiceNegocio;
        private readonly ISucursalService _sucursalService;
        private readonly IConfiguration _configuration;

        public SimuladorPedidoNegocio(IWebServiceNegocio webServiceNegocio, ISucursalService sucursalService, IConfiguration configuration)
        {
            _webServiceNegocio = webServiceNegocio;
            _sucursalService = sucursalService;
            _configuration = configuration;
        }

        public async Task<DataSet> SimuladorPiezaAsync(string codebar, string noCliente, string ClaveSap)
        {
            DataSet tablaRespuesta = new DataSet();
            string xCadXml;
            string respuesta;

            try
            {
                xCadXml = await XmlSimuladorPieza(codebar, noCliente, ClaveSap);

                if (string.IsNullOrEmpty(xCadXml))
                {
                    return null;
                }

                respuesta = await RequestResponseAsync(xCadXml, "simulador_Pedidos");

                tablaRespuesta = LeerXMLSimuladorPieza(respuesta);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error de WS simulador pedidos: {ex.Message}");
            }

            return tablaRespuesta;
        }
        private async Task<string> XmlSimuladorPieza(string codebar, string noCliente, string ClaveSap)
        {
            TipoVenta tipoVenta = await _sucursalService.ObtenerTipoVenta(ClaveSap);

            int CanalVenta = tipoVenta.CanalVenta;
            string Empresa = tipoVenta.Empresa;

            try
            {
                // Inicia la cadena XML vacía
                string xml = string.Empty;

                // Obtiene el valor de 'canal' añadiendo un prefijo "0" al canal de ventas declarado en otra clase
                string canal = "0" + CanalVenta;

                // Inicializa la variable 'compania' vacía, se asignará un valor basado en la empresa seleccionada
                string compania = string.Empty;

                // Asigna el valor de 'compania' en base a la empresa actual
                switch (Empresa)
                {
                    case "GSA":
                        compania = "1200";
                        break;
                    case "GAM":
                        compania = "1300";
                        break;
                    case "SA":
                        compania = "1100";
                        break;
                    case "GAN":
                        compania = "1400";
                        break;
                }

                // Inicializa variables para crear diferentes partes del XML
                string xmlItems = string.Empty;
                string xmlSchdl = string.Empty;
                string xmlTexts = string.Empty;

                // Establece la cantidad total (1 en este caso) y el índice inicial (10)
                double cantidadTotal = 1;
                int i = 10;

                // Construye el bloque XML de 'Items'
                xmlItems += "<item>";
                xmlItems += $"<ItmNumber>{i.ToString().PadLeft(6, '0')}</ItmNumber>";
                xmlItems += $"<Material>{codebar}</Material>";
                xmlItems += $"<TargetQty>{cantidadTotal}</TargetQty>";
                xmlItems += $"<Plant>{ClaveSap}</Plant>"; // Se usa la clave de planta declarada
                xmlItems += "<RechazoL></RechazoL>";
                xmlItems += "</item>";

                // Construye el bloque XML de 'Schedule'
                xmlSchdl += "<item>";
                xmlSchdl += $"<ItmNumber>{i.ToString().PadLeft(6, '0')}</ItmNumber>";
                xmlSchdl += $"<ReqDate>{DateTime.UtcNow:yyyy-MM-dd}</ReqDate>";
                xmlSchdl += $"<ReqQty>{cantidadTotal}</ReqQty>";
                xmlSchdl += "</item>";

                // Construye el bloque XML de 'Texts'
                xmlTexts += "<item>";
                xmlTexts += "<ItmNumber></ItmNumber>";
                xmlTexts += $"<TextLine>{i.ToString().PadLeft(6, '0')}</TextLine>";
                xmlTexts += "</item>";

                // Construye el XML completo usando una estructura de SOAP para enviar los datos
                xml = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:urn=\"urn:sap-com:document:sap:soap:functions:mc-style\">";
                xml += "<soapenv:Header/>";
                xml += "<soapenv:Body>";
                xml += "<urn:ZFmSimulateSoProm>";
                xml += "<SoAddrwe>";
                xml += "<Name></Name><Name2></Name2><Name3></Name3><City></City><District></District><PostlCod1></PostlCod1>";
                xml += "<Street></Street><HouseNo></HouseNo><StrSuppl1></StrSuppl1><StrSuppl2></StrSuppl2><Location></Location>";
                xml += "<Country></Country><Region></Region><Tel1Numbr></Tel1Numbr><FaxNumber></FaxNumber><HouseNo2></HouseNo2>";
                xml += "<EMail></EMail><Name4></Name4>";
                xml += "</SoAddrwe>";
                xml += "<SoHeader>";
                xml += $"<SalesOrg>{compania}</SalesOrg>";
                xml += $"<DistrChan>{canal}</DistrChan>";
                xml += $"<ReqDateH>{DateTime.UtcNow:yyyy-MM-dd}</ReqDateH>";
                xml += $"<PurchDate>{DateTime.UtcNow:yyyy-MM-dd}</PurchDate>";
                xml += $"<PurchNoC>{noCliente}</PurchNoC>";
                xml += "<PymtMeth></PymtMeth><BillDate></BillDate><MotivoH></MotivoH><BloqueoFact></BloqueoFact><BloqueoSo></BloqueoSo>";
                xml += "</SoHeader>";
                xml += "<SoItems>";
                xml += xmlItems; // Inserta los items creados en el bloque
                xml += "</SoItems>";
                xml += "<SoParnr>";
                xml += $"<PartnAg>{noCliente}</PartnAg><PartnWe>{noCliente}</PartnWe><PartnRe>{noCliente}</PartnRe><PartnRg>{noCliente}</PartnRg>";
                xml += "</SoParnr>";
                xml += "<SoSchdl>";
                xml += xmlSchdl; // Inserta el bloque de programación
                xml += "</SoSchdl>";
                xml += "<SoTexts>";
                xml += xmlTexts; // Inserta el bloque de textos
                xml += "</SoTexts>";
                xml += "</urn:ZFmSimulateSoProm>";
                xml += "</soapenv:Body>";
                xml += "</soapenv:Envelope>";

                // Retorna el XML final
                return xml;
            }
            catch (Exception ex)
            {
                // Muestra un mensaje de error en la consola
                Console.WriteLine($"Error de armado XML WS simulador pedidos: {ex.Message}");
                return string.Empty; // Devuelve una cadena vacía en caso de error
            }
        }

        private async Task<string> RequestResponseAsync(string soapRequest, string action)
        {
            Webservice webservice = await _webServiceNegocio.AccesoWebservice(action);

            // Configuración del HttpClientHandler
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator; // Si es necesario ignorar el certificado SSL.
            handler.Proxy = null; // Si estás detrás de un proxy, podrías necesitar esta configuración.

            using (var client = new HttpClient(handler))
            {
                string username = webservice.usuario!;
                string password = webservice.pwd!;

                client.DefaultRequestHeaders.Add("sap-usercontext", "sap-client=100; path=/");

                var content = new StringContent(soapRequest, Encoding.UTF8, "text/xml");

                var byteArray = Encoding.ASCII.GetBytes($"{username}:{password}");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                client.Timeout = TimeSpan.FromSeconds(60); // Aumenta el timeout a 60 segundos.

                HttpResponseMessage response = null;
                try
                {
                    // ====================== Servidor y
                    // ====================== Local
                    // Obtén el entorno actual
                    var environment = _configuration["Environment"];

                    // Devuelve la cadena de conexión adecuada
                    if (environment == "2")
                    {
                        response = await client.PostAsync(webservice.rutaNet, content);
                    }
                    else
                    {
                        response = await client.PostAsync(webservice.rutaNet, content);
                    }

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new HttpRequestException($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                    }

                    return await response.Content.ReadAsStringAsync();
                }
                catch (HttpRequestException ex)
                {
                    // Maneja el error de conexión
                    throw new Exception($"Error de conexión: {ex.Message}");
                }
                finally
                {
                    response?.Dispose();
                }
            }
        }
            public DataSet LeerXMLSimuladorPieza(string xmlRespuesta)
            {
                try
                {
                    // Configuración de la tabla para promociones
                    DataTable tablaPromociones = new DataTable("promos");
                    tablaPromociones.Columns.Add("status");
                    tablaPromociones.Columns.Add("montoral");
                    tablaPromociones.Columns.Add("posicion");
                    tablaPromociones.Columns.Add("material");
                    tablaPromociones.Columns.Add("cantidad");
                    tablaPromociones.Columns.Add("clasecondicion");
                    tablaPromociones.Columns.Add("porcentajeDesc");
                    tablaPromociones.Columns.Add("sinDescripcion");
                    tablaPromociones.Columns.Add("montoDescuento");
                    tablaPromociones.Columns.Add("claveDocuemnto");
                    tablaPromociones.Columns.Add("porcentajeposi");
                    tablaPromociones.Columns.Add("precioFinal");
                    tablaPromociones.Columns.Add("descripcion");
                    tablaPromociones.Columns.Add("articuloRegalo");
                    tablaPromociones.Columns.Add("cantidadPadre");
                    tablaPromociones.Columns.Add("cantidadHijo");
                    tablaPromociones.Columns.Add("precioNuevo");
                    tablaPromociones.Columns.Add("promoAceptada");

                    // Configuración de la tabla para productos
                    DataTable tablaItems = new DataTable("ItemsOut");
                    tablaItems.Columns.Add("ITM_NUMBER");
                    tablaItems.Columns.Add("MATERIAL");
                    tablaItems.Columns.Add("SHORT_TEXT");
                    tablaItems.Columns.Add("NET_VALUE");
                    tablaItems.Columns.Add("CURRENCY");
                    tablaItems.Columns.Add("REQ_QTY");
                    tablaItems.Columns.Add("PLANT");
                    tablaItems.Columns.Add("TARGET_QTY");
                    tablaItems.Columns.Add("STGE_LOC");
                    tablaItems.Columns.Add("SUBTOTAL1");
                    tablaItems.Columns.Add("SUBTOTAL2");
                    tablaItems.Columns.Add("SUBTOTAL3");
                    tablaItems.Columns.Add("SUBTOTAL4");
                    tablaItems.Columns.Add("SUBTOTAL5");
                    tablaItems.Columns.Add("SUBTOTAL6");
                    tablaItems.Columns.Add("POINTS");
                    tablaItems.PrimaryKey = new DataColumn[] { tablaItems.Columns["MATERIAL"] };

                    // Configuración de la tabla para errores
                    DataTable tablaErrores = new DataTable("Errores");
                    tablaErrores.Columns.Add("Number");
                    tablaErrores.Columns.Add("Message");
                    tablaErrores.Columns.Add("Row");

                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(xmlRespuesta);

                    XmlNodeList materialesDisponibles = xmlDoc.GetElementsByTagName("n0:ZFmSimulateSoPromResponse");

                    // Procesa cada nodo dentro del XML
                    foreach (XmlNode nodeActual in materialesDisponibles[0].ChildNodes)
                    {
                        if (nodeActual.Name == "SdProm")
                        {
                            foreach (XmlNode promoNode in nodeActual.ChildNodes)
                            {
                                var material = promoNode["Matnr"].InnerText;
                                var claseCondicion = promoNode["Kschl"].InnerText;
                                var porcentajeDesc = promoNode["Kbetr"].InnerText;
                                var porcentajePosi = ValidaLongitud(Math.Abs(double.Parse(promoNode["Kbetr"].InnerText)));
                                var montoDescuento = promoNode["Kwert"].InnerText;
                                var sinDescripcion = promoNode["Koein"].InnerText;
                                var claveDocumento = promoNode["Aktnr"].InnerText;
                                var posicion = promoNode["Posnr"].InnerText;
                                var articuloRegalo = promoNode["Matpr"].InnerText;
                                var cantidadPadre = promoNode["Meng1"].InnerText;
                                var cantidadHijo = promoNode["Meng2"].InnerText;

                                tablaPromociones.Rows.Add(0, 0, posicion, material, 0, claseCondicion, porcentajeDesc, sinDescripcion,
                                    montoDescuento, claveDocumento, porcentajePosi, 0, "", articuloRegalo, cantidadPadre, cantidadHijo, 0, 0);
                            }
                        }
                        else if (nodeActual.Name == "ItemsOut")
                        {
                            foreach (XmlNode itemNode in nodeActual.ChildNodes)
                            {
                                var itmNumber = itemNode["ItmNumber"].InnerText;
                                var material = itemNode["Material"].InnerText;
                                var shortText = itemNode["ShortText"].InnerText;
                                var netValue = itemNode["NetValue"].InnerText;
                                var currency = itemNode["Currency"].InnerText;
                                var reqQty = itemNode["ReqQty"].InnerText;
                                var plant = itemNode["Plant"].InnerText;
                                var targetQty = itemNode["TargetQty"].InnerText;
                                var stgeLoc = itemNode["StgeLoc"].InnerText;
                                var subtotal1 = itemNode["SUBTOTAL1"].InnerText;
                                var subtotal2 = itemNode["SUBTOTAL2"].InnerText;
                                var subtotal3 = itemNode["SUBTOTAL3"].InnerText;
                                var subtotal4 = itemNode["SUBTOTAL4"].InnerText;
                                var subtotal5 = itemNode["SUBTOTAL5"].InnerText;
                                var subtotal6 = itemNode["SUBTOTAL6"].InnerText;

                                tablaItems.Rows.Add(itmNumber, material, shortText, netValue, currency, reqQty, plant, targetQty, stgeLoc, subtotal1, subtotal2, subtotal3, subtotal4, subtotal5, subtotal6, 0);
                            }
                        }
                        else if (nodeActual.Name == "Errores")
                        {
                            foreach (XmlNode errorNode in nodeActual.ChildNodes)
                            {
                                var number = errorNode["Number"].InnerText;
                                var message = errorNode["Message"].InnerText;
                                var row = errorNode["Row"].InnerText;
                                tablaErrores.Rows.Add(number, message, row);
                            }
                        }
                    }

                    // Crear y retornar el DataSet final
                    DataSet resultado = new DataSet();
                    resultado.Tables.Add(tablaPromociones);
                    resultado.Tables.Add(tablaItems);
                    resultado.Tables.Add(tablaErrores);
                    return resultado;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al armar tabla de respuesta del simulador: {ex.Message}");
                    return null;
                }
            }

            // Método para validar longitud
            private static string ValidaLongitud(double valor)
            {
                return valor.ToString("N2"); // Ajusta el formato de acuerdo a tus necesidades
            }

    }
}
