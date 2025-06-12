using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Net.Http.Headers;
using System.Text;
using System.Xml;

using Newtonsoft.Json;
using static sanimex.webapi.Dominio.Models.Disponibilidad; // Asegúrate de tener instalada esta librería (Newtonsoft.Json)

namespace sanimex.WebApi.Sap.Services
{
    public class DisponibilidadService : IDisponibilidadService
    {
        public async Task<object> DisponibilidadxCentrosAsync(string barcode, string descripcion="", string centrosCorredor="", string sucHijoSap="")
        {
            try
            {
                // Generar el XML de disponibilidad
                var xCad_XML = XmlDisponibilidadCorredor(barcode, descripcion, centrosCorredor, sucHijoSap);

                if (string.IsNullOrEmpty(xCad_XML))
                    throw new Exception("No se generó el XML de disponibilidad.");

                // Realizar la solicitud y obtener la respuesta
                var respuesta = await RequestResponseAsync(xCad_XML, "disponibilidad_centros");

                // Procesar la respuesta y convertirla a un objeto JSON
                var resultado = LeerXmlDisponibilidadCentros(respuesta);

                // Guardar la respuesta XML en un archivo
                string filePath = "respuesta.xml";
                System.IO.File.WriteAllText(filePath, respuesta);
                Console.WriteLine($"Respuesta XML guardada en: {filePath}");
                return resultado;  // El servicio devuelve el objeto
            }
            catch (Exception ex)
            {
                // Aquí deberías manejar el error de forma más apropiada, como usando logging
                throw new Exception($"Error interno: {ex.Message}", ex);
            }
        }

        private async Task<string> RequestResponseAsync(string soapRequest, string action)
        {
            using (var client = new HttpClient())
            {
                string username = "WEBSERV_USR";  // Cambia esto por tu usuario
                string password = "U9A/aGTJZea"; // Cambia esto por tu contraseña
                                                 // Agregar encabezado personalizado si es necesario
                client.DefaultRequestHeaders.Add("sap-usercontext", "sap-client=100; path=/");
                // Agrega el contenido SOAP a la solicitud
                var content = new StringContent(soapRequest, Encoding.UTF8, "text/xml");

                // Autenticación básica
                var byteArray = Encoding.ASCII.GetBytes($"{username}:{password}");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));



                // Envía la solicitud POST de forma asíncrona
                var response = await client.PostAsync("http://192.168.99.25:8000/sap/bc/srt/rfc/sap/zws_consulta_disp_mat/100/zws_consulta_disp_mat/zbn_consulta_disp_mat", content);

                // Asegúrate de manejar la respuesta
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                }

                // Leer el contenido de la respuesta de forma asíncrona
                return await response.Content.ReadAsStringAsync();
            }
        }
        public string XmlDisponibilidadCorredor(string barcode, string descripcion, string centroCorredor, string sucHijoSap)
        {
            string WerksNode = "";
            string IvidsNode = "";
            string IvMaterialNode = "";
            string xmlReturn = "";

            try
            {
                // Crear una tabla para centros
                DataTable tablaCentros = new DataTable();
                tablaCentros.Columns.Add("sucHijoSap", typeof(string));

                // Crear una fila de datos y agregar el valor de sucHijoSap
                DataRow datoCentros = tablaCentros.NewRow();
                datoCentros["sucHijoSap"] = sucHijoSap;
                tablaCentros.Rows.Add(datoCentros); // Agregamos la fila a la tabla

                // Verificar que no sea nulo
                if (datoCentros != null)
                {
                    // Dividir el valor de sucHijoSap en varios centros
                    string cadenaCentros = datoCentros["sucHijoSap"].ToString();
                    string[] arrCentros = cadenaCentros.Split(',');

                    // Recorrer los centros y crear los nodos XML para Werks
                    for (int i = 0; i < arrCentros.Length; i++)
                    {
                        WerksNode += "<item>";
                        WerksNode += "<Werks>" + arrCentros[i] + "</Werks>";
                        WerksNode += "</item>";
                    }

                    // Si barcode no está vacío, agregamos el nodo IvId
                    if (!string.IsNullOrEmpty(barcode))
                    {
                        IvidsNode = "<IvId>" + barcode + "</IvId>";
                    }

                    // Si descripcion no está vacía
                    if (!string.IsNullOrEmpty(descripcion))
                    {
                        string cadRetorno = "";

                        if (!string.IsNullOrEmpty(cadRetorno))
                        {
                            string[] arrCodes = cadRetorno.Split(',');

                            for (int i = 0; i < arrCodes.Length; i++)
                            {
                                IvidsNode += "<IvId>" + arrCodes[i] + "</IvId>";
                            }
                        }
                    }

                    // Generamos el XML final
                    xmlReturn = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:urn=\"urn:sap-com:document:sap:soap:functions:mc-style\">";
                    xmlReturn += "<soapenv:Header/>";
                    xmlReturn += "<soapenv:Body>";
                    xmlReturn += "<urn:ZfmConsultaDispMat>";
                    xmlReturn += "<ItWerks>";
                    xmlReturn += WerksNode;
                    xmlReturn += "</ItWerks>";

                    if (!string.IsNullOrEmpty(IvidsNode))
                    {
                        xmlReturn += IvidsNode;
                    }
                    else
                    {
                        xmlReturn += "<IvId></IvId>";
                    }

                    if (!string.IsNullOrEmpty(IvMaterialNode))
                    {
                        xmlReturn += IvMaterialNode;
                    }
                    else
                    {
                        xmlReturn += "<IvMaterial></IvMaterial>";
                    }

                    xmlReturn += "</urn:ZfmConsultaDispMat>";
                    xmlReturn += "</soapenv:Body>";
                    xmlReturn += "</soapenv:Envelope>";
                }
                else
                {
                    return string.Empty;
                }

                return xmlReturn;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al generar XML plano para consulta por centros, reporte este error a sistemas: " + ex.Message);
                return string.Empty;
            }
        }

        // Función que lee el XML y devuelve el resultado en formato JSON
        public string LeerXmlDisponibilidadCentros(string xmlRespuesta)
        {
            // Crear una lista de SucHijoSap
            List<SucursalHijoSap> sucHijosList = new List<SucursalHijoSap>();

            // Crear un XmlDocument para procesar el XML
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlRespuesta);

            try
            {
                // Obtener la lista de nodos con la etiqueta "ZfmConsultaDispMatResponse"
                XmlNodeList sucursalesDisponibles = xmlDoc.GetElementsByTagName("n0:ZfmConsultaDispMatResponse");

                // Recorrer los nodos hijos de "ZfmConsultaDispMatResponse"
                for (int x = 0; x < sucursalesDisponibles[0].ChildNodes.Count; x++)
                {
                    XmlNode nodeActual = sucursalesDisponibles[0].ChildNodes[x];

                    // Obtener el sucHijoSap del nodo WerksNode
                    string sucHijoSap = nodeActual["Werks"]?.InnerText; // Asegúrate que "WerksNode" es el nombre correcto

                    // Verificar si ya existe un SucHijoSap en la lista
                    var sucHijoExistente = sucHijosList.FirstOrDefault(s => s.SucHijoSap == sucHijoSap);
                    if (sucHijoExistente == null)
                    {
                        // Si no existe, crear uno nuevo
                        sucHijoExistente = new SucursalHijoSap
                        {
                            SucHijoSap = sucHijoSap,
                            Productos = new List<Producto>()
                        };
                        sucHijosList.Add(sucHijoExistente);
                    }

                    // Recorrer los nodos de productos dentro del nodo actual
                    for (int i = 0; i < nodeActual.ChildNodes.Count; i++)
                    {
                        XmlNode nodeProductos = nodeActual.ChildNodes[i];

                        // Obtener los detalles del producto
                        string barcode = string.Empty;
                        string descripcion = string.Empty;
                        string stockLibre = string.Empty;

                        // Recorrer los atributos del producto (Matnr, Maktx, Labst)
                        for (int xy = 0; xy < nodeProductos.ChildNodes.Count; xy++)
                        {
                            switch (nodeProductos.ChildNodes[xy].Name)
                            {
                                case "Matnr":  // Código del producto
                                    barcode = nodeProductos.ChildNodes[xy].InnerText;
                                    break;

                                case "Maktx":  // Descripción del producto
                                    descripcion = nodeProductos.ChildNodes[xy].InnerText;
                                    break;

                                case "Labst":  // Stock libre
                                    stockLibre = barcode.Contains("FLE-") ? "1" : nodeProductos.ChildNodes[xy].InnerText;
                                    break;
                            }
                        }

                        // Agregar el producto a la lista de productos del SucHijo existente
                        sucHijoExistente.Productos.Add(new Producto
                        {
                            Codigo = barcode,
                            Descripcion = descripcion,
                            StockLibre = stockLibre
                        });
                    }
                }

                // Convertir la lista de SucHijoSap a formato JSON
                string jsonResult = JsonConvert.SerializeObject(sucHijosList, Newtonsoft.Json.Formatting.Indented);

                return jsonResult;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al leer el XML de respuesta: " + ex.Message);
            }
        }
    }
}
