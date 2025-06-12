
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using System.Xml;
using static sanimex.webapi.Dominio.Models.MSimuladorPieza;

namespace sanimex.WebApi.Sap.Services
{
    public class SimuladorPieza : ISimuladorPieza
    {
        public async Task<object> SimuladorPiezaAsync(string barcode, string noCliente, string centrosCorredor)
        {
            try
            {
                // Generar el XML de disponibilidad
                var xCad_XML = XmlSimuladorPieza(barcode, noCliente, centrosCorredor);

                if (string.IsNullOrEmpty(xCad_XML))
                    throw new Exception("No se generó el XML de disponibilidad.");

                // Realizar la solicitud y obtener la respuesta
                var respuesta = await RequestResponseAsync(xCad_XML, "simulador_Pedidos");

                // Procesar la respuesta y convertirla a un objeto JSON
                var resultado = LeerXmlSimuladorPieza(respuesta);

                // Guardar la respuesta XML en un archivo
                //string filePath = "respuesta.xml";
                //System.IO.File.WriteAllText(filePath, respuesta);
                //Console.WriteLine($"Respuesta XML guardada en: {filePath}");
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
                //var response = await client.PostAsync("http://10.10.10.28:8000/sap/bc/srt/rfc/sap/zws_simulate_so_prom/100/zws_simulate_so_prom/binding", content);
                var response = await client.PostAsync("http://10.10.10.16:8000/sap/bc/srt/rfc/sap/zws_simulate_so_prom/100/zws_simulate_prom/zbn_simulate_prom", content);

                // Asegúrate de manejar la respuesta
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                }

                // Leer el contenido de la respuesta de forma asíncrona
                return await response.Content.ReadAsStringAsync();
            }
        }
        private string XmlSimuladorPieza(string codebar, string noCliente,string centrosCorredor)
        {
            try
            {
                string empresa = "GSA";
                // Inicializa la variable XML como una cadena vacía.
                string XML = string.Empty;
                // canal de venta para gsa 1

                // Prepara el canal de venta, prefijando con '0'.
                string canal = "0" + 1;
                string compania = string.Empty;

                // Determina la compañía basada en el valor de Declaraciones.empresa.
                switch (empresa)
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

                // Inicializa cadenas para almacenar diferentes secciones del XML.
                string xmlItems = string.Empty;
                string xmlschdl = string.Empty;
                string xmltexts = string.Empty;

                // Define una cantidad total y un índice.
                double cantidad_total = 1;
                int i = 10;

                // Construye la sección de items del XML.
                xmlItems += "<item>";
                xmlItems += "<ItmNumber>" + i.ToString().PadLeft(6, '0') + "</ItmNumber>";
                xmlItems += "<Material>" + codebar + "</Material>";
                xmlItems += "<TargetQty>" + cantidad_total + "</TargetQty>";
                xmlItems += "<Plant>" + centrosCorredor + "</Plant>"; // Se supone que claveSap está definida en otro lugar.
                xmlItems += "<RechazoL></RechazoL>";
                xmlItems += "</item>";

                // Construye la sección de programación del XML.
                xmlschdl += "<item>";
                xmlschdl += "<ItmNumber>" + i.ToString().PadLeft(6, '0') + "</ItmNumber>";
                xmlschdl += "<ReqDate>" + DateTime.Now.ToString("yyyy-MM-dd") + "</ReqDate>";
                xmlschdl += "<ReqQty>" + cantidad_total + "</ReqQty>";
                xmlschdl += "</item>";

                // Construye la sección de textos del XML.
                xmltexts += "<item>";
                xmltexts += "<ItmNumber></ItmNumber>";
                xmltexts += "<TextLine>" + i.ToString().PadLeft(6, '0') + "</TextLine>";
                xmltexts += "</item>";

                // Comienza a construir el XML principal.
                XML = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:urn=\"urn:sap-com:document:sap:soap:functions:mc-style\">";
                XML += "  <soapenv:Header/>";
                XML += "   <soapenv:Body>";
                XML += "      <urn:ZFmSimulateSoProm>";

                // Sección de dirección.
                XML += "         <SoAddrwe>";
                XML += "            <Name></Name>";
                XML += "            <Name2></Name2>";
                XML += "            <Name3></Name3>";
                XML += "            <City></City>";
                XML += "            <District></District>";
                XML += "            <PostlCod1></PostlCod1>";
                XML += "            <Street></Street>";
                XML += "            <HouseNo></HouseNo>";
                XML += "            <StrSuppl1></StrSuppl1>";
                XML += "            <StrSuppl2></StrSuppl2>";
                XML += "            <Location></Location>";
                XML += "            <Country></Country>";
                XML += "            <Region></Region>";
                XML += "            <Tel1Numbr></Tel1Numbr>";
                XML += "            <FaxNumber></FaxNumber>";
                XML += "            <HouseNo2></HouseNo2>";
                XML += "            <EMail></EMail>";
                XML += "            <Name4></Name4>";
                XML += "         </SoAddrwe>";

                // Sección del encabezado de la orden.
                XML += "         <SoHeader>";
                XML += "            <SalesOrg>" + compania + "</SalesOrg>";
                XML += "            <DistrChan>" + canal + "</DistrChan>";
                XML += "            <ReqDateH>" + DateTime.Now.ToString("yyyy-MM-dd") + "</ReqDateH>";
                XML += "            <PurchDate>" + DateTime.Now.ToString("yyyy-MM-dd") + "</PurchDate>";
                XML += "            <PurchNoC>" + noCliente + "</PurchNoC>";
                XML += "            <PymtMeth></PymtMeth>";
                XML += "            <BillDate></BillDate>";
                XML += "            <MotivoH></MotivoH>";
                XML += "            <BloqueoFact></BloqueoFact>";
                XML += "            <BloqueoSo></BloqueoSo>";
                XML += "         </SoHeader>";

                // Sección de items.
                XML += "         <SoItems>";
                XML += "            <!--Zero or more repetitions:-->";
                XML += xmlItems; // Se añaden los items construidos anteriormente.
                XML += "         </SoItems>";

                // Sección de parámetros.
                XML += "         <SoParnr>";
                XML += "            <PartnAg>" + noCliente + "</PartnAg>";
                XML += "            <PartnWe>" + noCliente + "</PartnWe>";
                XML += "            <PartnRe>" + noCliente + "</PartnRe>";
                XML += "            <PartnRg>" + noCliente + "</PartnRg>";
                XML += "         </SoParnr>";

                // Sección de programación.
                XML += "         <SoSchdl>";
                XML += "            <!--Zero or more repetitions:-->";
                XML += xmlschdl; // Se añaden los elementos de programación.
                XML += "         </SoSchdl>";

                // Sección de textos.
                XML += "         <SoTexts>";
                XML += "            <!--Zero or more repetitions:-->";
                XML += xmltexts; // Se añaden los textos construidos.
                XML += "         </SoTexts>";

                // Cierra las etiquetas del XML.
                XML += "      </urn:ZFmSimulateSoProm>";
                XML += "   </soapenv:Body>";
                XML += "</soapenv:Envelope>";

                // Devuelve el XML completo.
                return XML;
            }
            catch (Exception ex)
            {
                // En caso de error, muestra un mensaje y devuelve una cadena vacía.
                Console.WriteLine("Error de armado XML WS simulador pedidos " + Environment.NewLine + ex.Message, "Error");
                return string.Empty;
            }
        }

        public string LeerXmlSimuladorPieza(string xmlRespuesta)
        {
            try
            {
                var promociones = new List<Promocion>();
                var items = new List<Item>();
                var errores = new List<Error>();

                // Cargar el XML
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlRespuesta);

                XmlNodeList materialesDisponible = xmlDoc.GetElementsByTagName("n0:ZFmSimulateSoPromResponse");

                foreach (XmlNode nodeActual in materialesDisponible[0].ChildNodes)
                {
                    if (nodeActual.Name == "SdProm")
                    {
                        foreach (XmlNode item in nodeActual.ChildNodes)
                        {
                            // Obtener valores y agregar a promociones
                            var material = item["Matnr"]?.InnerText;
                            var claseCondicion = item["Kschl"]?.InnerText;
                            var porcentajeDesc = item["Kbetr"]?.InnerText;
                            var montoDescuento = item["Kwert"]?.InnerText;
                            var posicion = item["Posnr"]?.InnerText;
                            var articuloRegalo = item["Matpr"]?.InnerText;
                            var cantidadPadre = item["Meng1"]?.InnerText;
                            var cantidadHijo = item["Meng2"]?.InnerText;

                            if (!string.IsNullOrEmpty(porcentajeDesc) && Math.Abs(Convert.ToDouble(porcentajeDesc)) >= 0)
                            {
                                promociones.Add(new Promocion
                                {
                                    Status = 0,
                                    Montal = 0,
                                    Posicion = posicion,
                                    Material = material,
                                    Cantidad = 0,
                                    ClaseCondicion = claseCondicion,
                                    PorcentajeDesc = Convert.ToDecimal(porcentajeDesc),
                                    SinDescripcion = string.Empty,
                                    MontoDescuento = Convert.ToDecimal(montoDescuento),
                                    ClaveDocumento = string.Empty,
                                    PorcentajePosi = string.Empty,
                                    PrecioFinal = 0,
                                    Descripcion = string.Empty,
                                    ArticuloRegalo = articuloRegalo,
                                    CantidadPadre = Convert.ToDecimal(cantidadPadre),
                                    CantidadHijo = Convert.ToDecimal(cantidadHijo),
                                    PrecioNuevo = 0,
                                    PromoAceptada = false
                                });
                            }
                        }
                    }
                    else if (nodeActual.Name == "ItemsOut")
                    {
                        foreach (XmlNode item in nodeActual.ChildNodes)
                        {
                            // Obtener valores y agregar a items
                            var itmNumber = item["ItmNumber"]?.InnerText;
                            var material = item["Material"]?.InnerText;
                            var shortText = item["ShortText"]?.InnerText;
                            var netValue = item["NetValue"]?.InnerText;
                            var currency = item["Currency"]?.InnerText;
                            var reqQty = item["ReqQty"]?.InnerText;
                            var plant = item["Plant"]?.InnerText;
                            var targetQty = item["TargetQty"]?.InnerText;
                            var stgeLoc = item["StgeLoc"]?.InnerText;
                            var subtotal1 = item["SUBTOTAL1"]?.InnerText;
                            var subtotal2 = item["SUBTOTAL2"]?.InnerText;
                            var subtotal3 = item["SUBTOTAL3"]?.InnerText;
                            var subtotal4 = item["SUBTOTAL4"]?.InnerText;
                            var subtotal5 = item["SUBTOTAL5"]?.InnerText;
                            var subtotal6 = item["SUBTOTAL6"]?.InnerText;

                            items.Add(new Item
                            {
                                ItmNumber = itmNumber,
                                Material = material,
                                ShortText = shortText,
                                NetValue = Convert.ToDecimal(netValue),
                                Currency = currency,
                                ReqQty = Convert.ToDecimal(reqQty),
                                Plant = plant,
                                TargetQty = Convert.ToDecimal(targetQty),
                                StgeLoc = stgeLoc,
                                Subtotal1 = Convert.ToDecimal(subtotal1),
                                Subtotal2 = Convert.ToDecimal(subtotal2),
                                Subtotal3 = Convert.ToDecimal(subtotal3),
                                Subtotal4 = Convert.ToDecimal(subtotal4),
                                Subtotal5 = Convert.ToDecimal(subtotal5),
                                Subtotal6 = Convert.ToDecimal(subtotal6),
                                Points = 0
                            });
                        }
                    }
                    else if (nodeActual.Name == "Errores")
                    {
                        foreach (XmlNode item in nodeActual.ChildNodes)
                        {
                            var number = item["Number"]?.InnerText;
                            var message = item["Message"]?.InnerText;
                            var row = item["Row"]?.InnerText;

                            errores.Add(new Error
                            {
                                Number = number,
                                Message = message,
                                Row = row
                            });
                        }
                    }
                }

                // Crear un objeto de respuesta final
                var response = new
                {
                    Promociones = promociones,
                    Items = items,
                    Errores = errores
                };

                // Convertir a JSON
                return JsonConvert.SerializeObject(response, Newtonsoft.Json.Formatting.Indented);
            }
            catch (Exception ex)
            {
                var errorResponse = new
                {
                    Success = false,
                    Message = "Error al armar tabla de respuesta del simulador",
                    Detail = ex.Message
                };
                return JsonConvert.SerializeObject(errorResponse);
            }
        }
    }
}
