
using System.Collections;
using System.Data;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Xml;
using static sanimex.webapi.Dominio.Models.MSimuladorPedidos;

namespace sanimex.WebApi.Sap.Services
{
    public class SimuladorPedidos : ISimuladorPedidos
    {

        public async Task<object> SimuladarPedidos(string codigosProductos, string noCliente, string canalVenta, string empresa, string claveSap, bool validador = true)
        {
            try
            {
                // Generar el XML de disponibilidad
                var xCad_XML = XmlSimuladorPedidos(codigosProductos, noCliente, canalVenta, empresa, claveSap, validador);

                if (string.IsNullOrEmpty(xCad_XML))
                    throw new Exception("No se generó el XML de disponibilidad.");

                // Realizar la solicitud y obtener la respuesta
                var respuesta = await RequestResponseAsync(xCad_XML, "simulador_Pedidos");

                // Procesar la respuesta y convertirla a un objeto JSON
                var resultado = LeerXMLSimuladorPedidos(respuesta);

                // Guardar la respuesta XML en un archivo
                string filePath = "respuestaSimuladorPedidos.xml";
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
                var response = await client.PostAsync("http://vhsnxdev.sanimex.com.mx:8000/sap/bc/srt/rfc/sap/zws_simulate_so_prom/100/zws_simulate_prom/zbn_simulate_prom", content);
                
                // Asegúrate de manejar la respuesta
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                }

                // Leer el contenido de la respuesta de forma asíncrona
                return await response.Content.ReadAsStringAsync();
            }
        }
        private string XmlSimuladorPedidos(string codigosProductos, string noCliente, string canalVenta, string empresa, string claveSap, bool validador = true)
        {
            try
            {
                string XML = string.Empty;
                string canal = "0" + canalVenta;
                string compania = string.Empty;

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

                string xmlItems = string.Empty;
                string xmlschdl = string.Empty;
                string xmltexts = string.Empty;

                double cantidadTotal = 0;
                int i = 0;

                // Dividir la cadena de codigosProductos en un array de strings
                var listaCodigos = codigosProductos.Split(',');

                // Asumiendo que la cantidad solicitada se puede manejar de otra manera (puedes modificar esta parte según tu lógica).
                foreach (var codigoProducto in listaCodigos)
                {
                    i += 10;

                    // Aquí debes definir cómo vas a obtener la cantidad (ya que ya no tienes el DataTable).
                    // Puedes tener una lista separada para las cantidades o algún otro mecanismo.
                    // Por ahora, asumiré que tienes un valor predeterminado.
                    double cantidadSolicitada = 1; // Reemplaza con tu lógica para obtener la cantidad
                    double cantidad = 1; // Reemplaza con tu lógica para obtener la cantidad en stock

                    if (validador)
                    {
                        if (cantidadSolicitada > cantidad)
                        {
                            cantidadTotal = cantidad;
                        }
                        else
                        {
                            cantidadTotal = cantidadSolicitada;
                        }
                    }
                    else
                    {
                        cantidadTotal = cantidadSolicitada;
                    }

                    xmlItems += "<item>";
                    xmlItems += $"<ItmNumber>{i.ToString().PadLeft(6, '0')}</ItmNumber>";
                    xmlItems += $"<Material>{codigoProducto.Trim()}</Material>"; // Asegúrate de eliminar espacios innecesarios
                    xmlItems += $"<TargetQty>{cantidadTotal}</TargetQty>";
                    xmlItems += $"<Plant>{claveSap}</Plant>";
                    xmlItems += "<RechazoL></RechazoL>";
                    xmlItems += "</item>";

                    xmlschdl += "<item>";
                    xmlschdl += $"<ItmNumber>{i.ToString().PadLeft(6, '0')}</ItmNumber>";
                    xmlschdl += $"<ReqDate>{DateTime.Now:yyyy-MM-dd}</ReqDate>";
                    xmlschdl += $"<ReqQty>{cantidadTotal}</ReqQty>";
                    xmlschdl += "</item>";

                    xmltexts += "<item>";
                    xmltexts += "<ItmNumber></ItmNumber>";
                    xmltexts += $"<TextLine>{i.ToString().PadLeft(6, '0')}</TextLine>";
                    xmltexts += "</item>";
                }

                XML = $"<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:urn=\"urn:sap-com:document:sap:soap:functions:mc-style\">";
                XML += "  <soapenv:Header/>";
                XML += "   <soapenv:Body>";
                XML += "      <urn:ZFmSimulateSoProm>";
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
                XML += "         <SoHeader>";
                XML += $"            <SalesOrg>{compania}</SalesOrg>";
                XML += $"            <DistrChan>{canal}</DistrChan>";
                XML += $"            <ReqDateH>{DateTime.Now:yyyy-MM-dd}</ReqDateH>";
                XML += $"            <PurchDate>{DateTime.Now:yyyy-MM-dd}</PurchDate>";
                XML += $"            <PurchNoC>{noCliente}</PurchNoC>";
                XML += "            <PymtMeth></PymtMeth>";
                XML += "            <BillDate></BillDate>";
                XML += "            <MotivoH></MotivoH>";
                XML += "            <BloqueoFact></BloqueoFact>";
                XML += "            <BloqueoSo></BloqueoSo>";
                XML += "         </SoHeader>";
                XML += "         <SoItems>";
                XML += xmlItems;
                XML += "         </SoItems>";
                XML += "         <SoParnr>";
                XML += $"            <PartnAg>{noCliente}</PartnAg>";
                XML += $"            <PartnWe>{noCliente}</PartnWe>";
                XML += $"            <PartnRe>{noCliente}</PartnRe>";
                XML += $"            <PartnRg>{noCliente}</PartnRg>";
                XML += "         </SoParnr>";
                XML += "         <SoSchdl>";
                XML += xmlschdl;
                XML += "         </SoSchdl>";
                XML += "         <SoTexts>";
                XML += xmltexts;
                XML += "         </SoTexts>";
                XML += "      </urn:ZFmSimulateSoProm>";
                XML += "   </soapenv:Body>";
                XML += "</soapenv:Envelope>";

                return XML;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error de armado XML WS simulador pedidos: " + ex.Message);
                return string.Empty;
            }
        }

        public string LeerXMLSimuladorPedidos(string XmlRespuesta)
        {
            try
            {
                // Crear las listas de objetos para almacenar la información del XML
                var promociones = new List<Promo>();
                var itemsOut = new List<Item>();
                var errores = new List<ErrorResponse>();
                int puntos = 0;
                var paramsPuntos = new Hashtable();

                // Cargar el XML
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(XmlRespuesta);

                // Validar que existan nodos
                if (xmlDoc.ChildNodes.Count >= 1)
                {
                    // Leer "EtPuntos" para cargar los puntos
                    foreach (XmlNode xNode in xmlDoc.GetElementsByTagName("EtPuntos")[0].ChildNodes)
                    {
                        paramsPuntos.Add(xNode["Material"].InnerText, xNode["Caract"].InnerText);
                    }

                    // Leer "SdProm" para cargar las promociones
                    foreach (XmlNode xNode in xmlDoc.GetElementsByTagName("SdProm")[0].ChildNodes)
                    {
                        var promo = new Promo
                        {
                            Status = "0",
                            Montoral = "0",
                            Posicion = xNode["Posnr"].InnerText,
                            Material = xNode["Matnr"].InnerText,
                            Cantidad = "0",
                            ClaseCondicion = xNode["Kschl"].InnerText,
                            PorcentajeDesc = xNode["Kbetr"].InnerText,
                            SinDescripcion = xNode["Koein"].InnerText,
                            MontoDescuento = xNode["Kwert"].InnerText,
                            ClaveDocumento = xNode["Aktnr"].InnerText,
                            PorcentajePosi = Math.Abs(double.Parse(xNode["Kbetr"].InnerText)).ToString(),
                            PrecioFinal = "0",
                            Descripcion = "",
                            ArticuloRegalo = xNode["Matpr"].InnerText,
                            CantidadPadre = xNode["Meng1"].InnerText,
                            CantidadHijo = xNode["Meng2"].InnerText,
                            PrecioNuevo = "0",
                            PromoAceptada = "0"
                        };
                        promociones.Add(promo);
                    }

                    // Leer "ItemsOut" para cargar los items
                    foreach (XmlNode xNode in xmlDoc.GetElementsByTagName("ItemsOut")[0].ChildNodes)
                    {
                        if (paramsPuntos.ContainsKey(xNode["Material"].InnerText))
                        {
                            puntos = Convert.ToInt32(paramsPuntos[xNode["Material"].InnerText]);
                        }

                        // Obtener información del producto (simulación en este caso)
                        var infoProducto = new Hashtable();
                        // Simulación de llamada a Datos_WS, sustitúyelo por tu propia implementación
                        infoProducto["presentacion"] = "Presentación Demo";
                        infoProducto["peso"] = "1kg";
                        infoProducto["ClaveProdServ"] = "01010101";
                        infoProducto["ClaveUnidad"] = "C62";
                        infoProducto["resurtible"] = "Sí";
                        infoProducto["importado"] = "No";
                        infoProducto["Proveedor"] = "Proveedor Demo";
                        infoProducto["Contenido"] = "Contenido Demo";
                        infoProducto["PromoCrest"] = "PromoCrest Demo";
                        infoProducto["Clasificacion"] = "A";

                        var item = new Item
                        {
                            ItmNumber = xNode["ItmNumber"].InnerText,
                            Material = xNode["Material"].InnerText,
                            ShortText = xNode["ShortText"].InnerText,
                            NetValue = xNode["NetValue"].InnerText,
                            Currency = xNode["Currency"].InnerText,
                            ReqQty = xNode["ReqQty"].InnerText,
                            Plant = xNode["Plant"].InnerText,
                            TargetQty = xNode["TargetQty"].InnerText,
                            StgeLoc = xNode["StgeLoc"].InnerText,
                            Subtotal1 = xNode["SUBTOTAL1"].InnerText,
                            Subtotal2 = xNode["SUBTOTAL2"].InnerText,
                            Subtotal3 = xNode["SUBTOTAL3"].InnerText,
                            Subtotal4 = xNode["SUBTOTAL4"].InnerText,
                            Subtotal5 = xNode["SUBTOTAL5"].InnerText,
                            Subtotal6 = xNode["SUBTOTAL6"].InnerText,
                            Points = puntos,
                            Presentacion = infoProducto["presentacion"].ToString(),
                            Peso = infoProducto["peso"].ToString(),
                            ClaveProdServ = infoProducto["ClaveProdServ"].ToString(),
                            ClaveUnidad = infoProducto["ClaveUnidad"].ToString(),
                            Resurtible = infoProducto["resurtible"].ToString(),
                            Importado = infoProducto["importado"].ToString(),
                            Proveedor = infoProducto["Proveedor"].ToString(),
                            Contenido = infoProducto["Contenido"].ToString(),
                            PromoCrest = infoProducto["PromoCrest"].ToString(),
                            PrecioCombo = 0,
                            Clasificacion = infoProducto["Clasificacion"].ToString()
                        };
                        itemsOut.Add(item);
                    }

                    // Leer "Errores" para cargar los errores
                    foreach (XmlNode xNode in xmlDoc.GetElementsByTagName("Errores")[0].ChildNodes)
                    {
                        var errorResponse = new ErrorResponse
                        {
                            Number = xNode["Number"].InnerText,
                            Message = xNode["Message"].InnerText,
                            Row = xNode["Row"].InnerText
                        };
                        errores.Add(errorResponse);
                    }
                }

                // Crear el objeto de respuesta final
                var simuladorResponse = new
                {
                    Promociones = promociones,
                    Items = itemsOut,
                    Errores = errores
                };

                // Serializar a JSON
                string jsonResponse = JsonSerializer.Serialize(simuladorResponse, new JsonSerializerOptions { WriteIndented = true });
                return jsonResponse;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al armar tabla de respuesta del simulador: " + ex.Message);
                return null;
            }
        }
    }
}
