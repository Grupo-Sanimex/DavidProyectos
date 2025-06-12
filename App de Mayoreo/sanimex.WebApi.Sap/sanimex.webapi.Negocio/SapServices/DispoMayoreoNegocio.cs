
using System.Data;
using System.Net.Http.Headers;
using System.Text;
using System.Xml;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using sanimex.webapi.Datos.Servicio.WebServicesSap.Interfaces;
using sanimex.webapi.Dominio.Models;
using sanimex.webapi.Dominio.Models.WebServiceSap;
using sanimex.webapi.Negocio.Usuarios;
using sanimex.webapi.Negocio.WebServiceSap;

namespace sanimex.WebApi.Sap.Services
{
    public class DispoMayoreoNegocio : IDispoMayoreoNegocio
    {
        private readonly IWebServiceNegocio _webServiceNegocio;
        private readonly IWebServiceSap _webServiceSap;
        private readonly IConfiguration _configuration;
        public DispoMayoreoNegocio(IWebServiceNegocio webServiceNegocio, IWebServiceSap webServiceSap, IConfiguration configuration)
        {
            _webServiceNegocio = webServiceNegocio;
            _webServiceSap = webServiceSap;
            _configuration = configuration;
        }
        public async Task<List<MDisponibilidadMayoreo>> DisponibilidadxMayoreoAsync(string barcode, string centrosCorredor, bool soloExistencias = true)
        {
            try
            {
                // Generar el XML de disponibilidad
                string xCad_XML = await XmlDisponibilidad(barcode, centrosCorredor);

                if (string.IsNullOrEmpty(xCad_XML))
                    throw new Exception("No se generó el XML de disponibilidad.");

                // Realizar la solicitud y obtener la respuesta
                var respuesta = await RequestResponseAsync(xCad_XML, "disponibilidad_mayoreo");

                // Procesar la respuesta y convertirla a un objeto JSON
                var resultado = Leer_XML_Disponibilidad(respuesta, soloExistencias);

                return resultado.ToList();  // El servicio devuelve el objeto
            }
            catch (Exception ex)
            {
                // Aquí deberías manejar el error de forma más apropiada, como usando logging
                throw new Exception($"Error interno: {ex.Message}", ex);
            }
        }
        private async Task<string> RequestResponseAsync(string soapRequest, string action)
        {
            Webservice webservice = await _webServiceNegocio.AccesoWebservice("disponibilidad_mayoreo");

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
                    // Obtén el entorno actual

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

        private async Task<string> XmlDisponibilidad(string barcode, string centrosCorredor)
        {
            string xml = string.Empty;

            string sucHijo = await _webServiceSap.CorredorCentro(centrosCorredor);

            // Armado de los centros de surtido a consultar
            string[] listaSucursales = sucHijo.Split(',');
            string xmlCentros = string.Empty;

            for (int i = 0; i < listaSucursales.Length; i++)
            {
                xmlCentros += "<item>";
                xmlCentros += "<SIGN>I</SIGN>";
                xmlCentros += "<OPTION>EQ</OPTION>";
                xmlCentros += "<PLANT_LOW>" + listaSucursales[i] + "</PLANT_LOW>";
                xmlCentros += "<PLANT_HIGH></PLANT_HIGH>";
                xmlCentros += "</item>";
            }

            if (string.IsNullOrEmpty(xmlCentros))
            {
                xmlCentros += "<item>";
                xmlCentros += "<SIGN>I</SIGN>";
                xmlCentros += "<OPTION>EQ</OPTION>";
                xmlCentros += "<PLANT_LOW>" + centrosCorredor + "</PLANT_LOW>"; // Asegúrate de que claveSap esté definido en el contexto.
                xmlCentros += "<PLANT_HIGH></PLANT_HIGH>";
                xmlCentros += "</item>";
            }

            // Armado de la lista de productos
            string[] listaProductos = barcode.Split(',');
            string xmlProductos = string.Empty;

            for (int x = 0; x < listaProductos.Length; x++)
            {
                xmlProductos += "<item>";
                xmlProductos += "<SIGN>I</SIGN>";
                xmlProductos += "<OPTION>EQ</OPTION>";
                xmlProductos += "<LOW>" + listaProductos[x] + "</LOW>";
                xmlProductos += "<HIGH></HIGH>";
                xmlProductos += "</item>";
            }

            try
            {
                xml = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:urn=\"urn:sap-com:document:sap:rfc:functions\">";
                xml += "    <soapenv:Header/>";
                xml += "    <soapenv:Body><urn:Z_MM_DISPONIBILITY>";
                xml += "        <CENTROS>";
                xml += "            <!--Zero or more repetitions:-->";
                xml += xmlCentros;
                xml += "        </CENTROS>";
                xml += "        <DISPONIBLE>";
                xml += "            <item>";
                xml += "                <MATNR></MATNR>";
                xml += "                <WERKS></WERKS>";
                xml += "                <NAME1></NAME1>";
                xml += "                <STLIB></STLIB>";
                xml += "                <STCON></STCON>";
                xml += "                <STCOM></STCOM>";
                xml += "                <STPED></STPED>";
                xml += "                <STDIS></STDIS>";
                xml += "            </item>";
                xml += "        </DISPONIBLE>";
                xml += "        <MATERIALES>";
                xml += "            <!--Zero or more repetitions:-->";
                xml += xmlProductos;
                xml += "        </MATERIALES>";
                xml += "    </urn:Z_MM_DISPONIBILITY>";
                xml += "  </soapenv:Body>";
                xml += "</soapenv:Envelope>";
            }
            catch (Exception ex)
            {
               Console.WriteLine("Error al armar el XML de Disponibilidad : " + ex.Message + "\nError");
            }

            return xml;
        }
        private List<MDisponibilidadMayoreo> Leer_XML_Disponibilidad(string xmlRespuesta, bool soloExistencias)
        {
            var listaResultados = new List<MDisponibilidadMayoreo>(); // Lista para almacenar los resultados

            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlRespuesta);

                if (xmlDoc.ChildNodes.Count >= 1)
                {
                    foreach (XmlNode xNode in xmlDoc.GetElementsByTagName("DISPONIBLE")[0].ChildNodes)
                    {
                        int disponible;
                        if (xNode["MATNR"].InnerText.Contains("FLE-"))
                        {
                            disponible = 1;
                        }
                        else
                        {
                            disponible = int.Parse(xNode["STDIS"].InnerText.Replace(".0", ""));
                        }

                        if (soloExistencias)
                        {
                            if (disponible >= 1)
                            {
                                listaResultados.Add(new MDisponibilidadMayoreo
                                {
                                    Codigo = xNode["MATNR"].InnerText,
                                    ClaveSAP = xNode["WERKS"].InnerText,
                                    NombreSucursal = xNode["NAME1"].InnerText,
                                    StockLibre = int.Parse(xNode["STLIB"].InnerText.Replace(".0", "")),
                                    StockDispo = disponible
                                });
                            }
                        }
                        else
                        {
                            listaResultados.Add(new MDisponibilidadMayoreo
                            {
                                Codigo = xNode["MATNR"].InnerText,
                                ClaveSAP = xNode["WERKS"].InnerText,
                                NombreSucursal = xNode["NAME1"].InnerText,
                                StockLibre = int.Parse(xNode["STLIB"].InnerText.Replace(".0", "")),
                                StockDispo = disponible
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al leer el XML de respuesta: " + ex.Message);
            }

            return listaResultados; // Convertir la lista a JSON
        }

    }
}
