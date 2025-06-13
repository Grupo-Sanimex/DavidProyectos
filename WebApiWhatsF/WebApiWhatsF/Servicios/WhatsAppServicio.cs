using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using WebApiWhatsF.Models;

namespace WebApiWhatsF.Servicios
{
    public class WhatsAppServicio
    {
        private readonly HttpClient _httpClient;
        private readonly FacebookConfig _facebookSettings;

        public WhatsAppServicio(HttpClient httpClient, IOptions<FacebookConfig> facebookSettings)
        {
            _httpClient = httpClient;
            _facebookSettings = facebookSettings.Value;
        }
        public async Task<string> EnviarMensajeWhatsAppAsync(string numeroCliente, string numeroAcesor, string nombreCotizacion, string TotalCotizacion)
        {

            // Construir el objeto JSON dinámicamente
            var jsonObject = new Dictionary<string, object>
            {
                { "messaging_product", "whatsapp" },
                { "to", "52"+ numeroCliente },
                { "type", "template" },
                { "template", new Dictionary<string, object>
                    {
                        { "name", "gam" },
                        { "language", new Dictionary<string, string> { { "code", "es_MX" } } },
                        { "components", new List<object>
                            {
                                new Dictionary<string, object>
                                {
                                    { "type", "header" },
                                    { "parameters", new List<object>
                                        {
                                            new Dictionary<string, object>
                                            {
                                                { "type", "document" },
                                                { "document", new Dictionary<string, string>
                                                    {
                                                        { "link", "http://200.57.183.111:60803/api/PdfDocuments/pdf/"+ nombreCotizacion },
                                                        { "filename", nombreCotizacion }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                },
                                new Dictionary<string, object>
                                {
                                    { "type", "body" },
                                    { "parameters", new List<object>
                                        {
                                            new Dictionary<string, object>
                                            {
                                                { "type", "text" },
                                                { "text", numeroAcesor }
                                            },
                                            new Dictionary<string, object>
                                            {
                                                { "type", "text" },
                                                { "text", TotalCotizacion }
                                            },
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };
            var json = JsonSerializer.Serialize(jsonObject, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _facebookSettings.AccessToken);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(_facebookSettings.ApiUrl, content);

            return response.IsSuccessStatusCode
                ? "Mensaje enviado con éxito."
                : await response.Content.ReadAsStringAsync();
        }

        public async Task<string> EnviarMensajeWhatsAppAsyncD(string numeroCliente, string numeroAcesor, string nombreCotizacion, string TotalCotizacion, string telefonoSucursal)
        {

            // Construir el objeto JSON dinámicamente
            var jsonObject = new Dictionary<string, object>
            {
                { "messaging_product", "whatsapp" },
                { "recipient_type", "individual" },
                { "to", "52"+ numeroCliente },
                { "type", "template" },
                { "template", new Dictionary<string, object>
                    {
                        { "name", "empresa" },
                        { "language", new Dictionary<string, string> { { "code", "es_MX" } } },
                        { "components", new List<object>
                            {
                                new Dictionary<string, object>
                                {
                                    { "type", "header" },
                                    { "parameters", new List<object>
                                        {
                                            new Dictionary<string, object>
                                            {
                                                { "type", "document" },
                                                { "document", new Dictionary<string, string>
                                                    {
                                                        { "link", "http://200.57.183.111:60803/api/PdfDocuments/pdf/"+ nombreCotizacion },
                                                        { "filename", nombreCotizacion }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                },
                                new Dictionary<string, object>
                                {
                                    { "type", "body" },
                                    { "parameters", new List<object>
                                        {
                                            new Dictionary<string, object>
                                            {
                                                { "type", "text" },
                                                { "parameter_name", "telefono_sucursal"},
                                                { "text", telefonoSucursal  }
                                            },
                                            new Dictionary<string, object>
                                            {
                                                { "type", "text" },
                                                {"parameter_name", "telefono_asesora" },
                                                { "text", numeroAcesor }
                                            },
                                            new Dictionary<string, object>
                                            {
                                                { "type", "text" },
                                                { "parameter_name", "total_cotizacion"},
                                                { "text", TotalCotizacion }
                                            },
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };
            var json = JsonSerializer.Serialize(jsonObject, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _facebookSettings.AccessToken);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(_facebookSettings.ApiUrl, content);

            return response.IsSuccessStatusCode
                ? "Mensaje enviado con éxito."
                : await response.Content.ReadAsStringAsync();
        }
    }
}
