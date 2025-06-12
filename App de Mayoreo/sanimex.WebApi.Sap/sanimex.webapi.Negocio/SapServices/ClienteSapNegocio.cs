using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Configuration;
using sanimex.webapi.Dominio.Models;
using sanimex.webapi.Dominio.Models.WebServiceSap;
using sanimex.webapi.Negocio.WebServiceSap;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;

namespace sanimex.webapi.Negocio.SapServices
{
    public class ClienteSapNegocio : IClienteSapNegocio
    {
        private readonly IWebServiceNegocio _webServiceNegocio;
        private readonly IConfiguration _configuration;

        public ClienteSapNegocio(IWebServiceNegocio webServiceNegocio, IConfiguration configuration)
        {
            _webServiceNegocio = webServiceNegocio;
            _configuration = configuration;
        }
        public async Task<bool> ClientesMayoreo(string idCliente, string empresa, string rfcCte = "")
        {
            bool existe = false; // Variable para verificar si el cliente existe
            int canalVenta = 2; // Por defecto
            try
            {
                string BUKRS = "1300";
                string VTWEG = canalVenta == 2 ? "02" : "01";

                switch (empresa)
                {
                    case "GAM":
                        BUKRS = "1300";
                        break;
                    case "GSA":
                        BUKRS = "1200";
                        break;
                    case "AYT":
                        BUKRS = "1100";
                        break;
                    case "GAN":
                        BUKRS = "1400";
                        break;
                }

                string StrSql = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:urn=\"urn:sap-com:document:sap:rfc:functions\">";
                StrSql += "<soapenv:Header/>";
                StrSql += "<soapenv:Body>";
                StrSql += "<urn:Z_FM_CRED_POS_BON>";
                StrSql += "<BUKRS>" + BUKRS + "</BUKRS>";
                StrSql += "<FACTURAS>";
                StrSql += "<item><DOC_NO></DOC_NO><ALLOC_NMBR></ALLOC_NMBR><PSTNG_DATE></PSTNG_DATE><DOC_TYPE></DOC_TYPE><LC_AMOUNT></LC_AMOUNT><CP1_LC_AMOUNT></CP1_LC_AMOUNT><BLINE_DATE></BLINE_DATE><PMNTTRMS></PMNTTRMS><DSCT_DAYS1></DSCT_DAYS1><BLINE_DATE2></BLINE_DATE2></item>";
                StrSql += "</FACTURAS>";
                StrSql += "<I_DATOS><item><KUNNR></KUNNR><STCD1></STCD1><NAME1></NAME1><NAME2></NAME2><LAND1></LAND1><REGIO></REGIO><ORT01></ORT01><ORT02></ORT02><PSTLZ></PSTLZ><STRAS></STRAS><TELF1></TELF1><ADRNR></ADRNR><STKZN></STKZN><TXJCD></TXJCD></item></I_DATOS>";
                StrSql += "<I_FPAGO><item><ZLSCH></ZLSCH><TEXT1></TEXT1></item></I_FPAGO>";

                if (!string.IsNullOrEmpty(idCliente))
                {
                    StrSql += "<KUNNR>" + idCliente.PadLeft(10, '0') + "</KUNNR>";
                }
                else
                {
                    StrSql += "<KUNNR></KUNNR>";
                }

                if (!string.IsNullOrEmpty(rfcCte))
                {
                    StrSql += "<STCD1>" + rfcCte + "</STCD1>";
                }
                else
                {
                    StrSql += "<STCD1></STCD1>";
                }

                StrSql += "<VTWEG>" + VTWEG + "</VTWEG>";
                StrSql += "</urn:Z_FM_CRED_POS_BON>";
                StrSql += "</soapenv:Body>";
                StrSql += "</soapenv:Envelope>";

                // Aquí suponemos que tienes un método llamado RequestResponse que realiza la solicitud SOAP
                string Respuesta = await RequestResponseAsync(StrSql, "status_credito");

                // Llamar a la función y obtener las listas de clientes, direcciones y facturas
                var (clientes, direccionesClientes) = Leer_XML_Clientes_Mayoreo(Respuesta);

                // Serializar las listas a JSON
                if (clientes.Count == 0)
                {
                    existe = false; // No se encontró el cliente
                }
                else
                {
                    existe = true; // Se encontró el cliente
                    var resultado = new
                    {
                        status = "success",
                        message = "Cliente encontrado",
                        // Clientes = clientes,
                        // DireccionesClientes = direccionesClientes,
                    };
                }

                //string jsonResponse = System.Text.Json.JsonSerializer.Serialize(resultado, new JsonSerializerOptions { WriteIndented = true });

                return existe; // Retorna el JSON
            }
            catch (Exception ex)
            {
                // Lógica de manejo de errores
                throw new Exception("Error al procesar la solicitud: " + ex.Message);
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

        private static (List<Cliente>, List<DireccionCliente>) Leer_XML_Clientes_Mayoreo(string xCad_XML)
        {
            var clientes = new List<Cliente>();
            var direccionesClientes = new List<DireccionCliente>();
            var facturasClientes = new List<FacturaCliente>();

            try
            {
                XmlDocument xmlDoc = new XmlDocument();

                if (!string.IsNullOrEmpty(xCad_XML))
                {
                    xmlDoc.LoadXml(xCad_XML);

                    if (xmlDoc.ChildNodes.Count >= 1)
                    {
                        XmlNode datosNode = xmlDoc.GetElementsByTagName("I_DATOS")[0].ChildNodes[1];

                        if (datosNode != null)
                        {
                            var cliente = new Cliente
                            {
                                IdCliente = datosNode["KUNNR"].InnerText,
                                Rfc = datosNode["STCD1"].InnerText,
                                CreditoAutorizado = xmlDoc.GetElementsByTagName("CREDIT_LIMIT")[0].InnerText,
                                CreditoConsumido = xmlDoc.GetElementsByTagName("COMM_TOTAL_L")[0].InnerText,
                                CreditoDisponible = xmlDoc.GetElementsByTagName("CREDIT_CUSTOMER")[0].InnerText,
                                DiasDeCredito = xmlDoc.GetElementsByTagName("TERMS_PAYMENT")[0].InnerText,
                                Bloqueado = xmlDoc.GetElementsByTagName("XBLOCKED")[0].InnerText,
                                Estatus = "", // Por defecto
                                Bonificacion = xmlDoc.GetElementsByTagName("RAPPEL_CUSTOMER")[0].InnerText,
                                RazonSocial = datosNode["NAME1"].InnerText + " " + datosNode["NAME2"].InnerText,
                                IdTipoPersona = datosNode["STCD1"].InnerText.Length == 13 ? 1 : 2,
                                RegimenFiscal = datosNode["KATR7"].InnerText,
                                RazonBloqueo = xmlDoc.GetElementsByTagName("BLOCK_REASON_TXT")[0].InnerText
                            };
                            clientes.Add(cliente);

                            var direccionCliente = new DireccionCliente
                            {
                                Calle = datosNode["STRAS"].InnerText,
                                Colonia = datosNode["ORT01"].InnerText,
                                MunicipioDelg = datosNode["ORT02"].InnerText,
                                Estado = datosNode["REGIO"].InnerText,
                                CP = datosNode["PSTLZ"].InnerText,
                                Pais = datosNode["LAND1"].InnerText
                            };
                            direccionesClientes.Add(direccionCliente);
                        }

                        //// Procesar facturas de cliente
                        //foreach (XmlNode xNode in xmlDoc.GetElementsByTagName("FACTURAS")[0].ChildNodes)
                        //{
                        //    if (!string.IsNullOrEmpty(xNode["DOC_NO"].InnerText))
                        //    {
                        //        var facturaCliente = new FacturaCliente
                        //        {
                        //            Folio = xNode["DOC_NO"].InnerText,
                        //            FechaVenta = xNode["PSTNG_DATE"].InnerText,
                        //            MontoVenta = xNode["LC_AMOUNT"].InnerText,
                        //            FechaPago = xNode["BLINE_DATE"].InnerText,
                        //            Saldo = xNode["CP1_LC_AMOUNT"].InnerText,
                        //            DiasCredito = xNode["DSCT_DAYS1"].InnerText,
                        //            FolioVenta = xNode["ALLOC_NMBR"].InnerText
                        //        };
                        //        facturasClientes.Add(facturaCliente);
                        //    }
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al procesar XML: " + ex.Message);
            }

            return (clientes, direccionesClientes);
        }
        private static Hashtable formaPago_Cte(string text1)
        {
            // Simulación de función para procesar la forma de pago
            return new Hashtable
        {
            {"idMetodoPago", 1},
            {"descripcion", "Efectivo"},
            {"claveR", "EF"},
            {"icono", "efectivo.png"}
        };
        }

        private Hashtable FormaPago_Cte(string text1)
        {
            // Implementa la lógica del método formaPago_Cte
            // Este método debe devolver una Hashtable con los valores requeridos
            return new Hashtable();
        }


    }
}
