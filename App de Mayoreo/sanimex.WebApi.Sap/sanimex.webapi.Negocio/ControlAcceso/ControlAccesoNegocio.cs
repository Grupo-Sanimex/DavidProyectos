using sanimex.webapi.Dominio.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeZoneConverter;
namespace sanimex.webapi.Negocio.ControlAcceso
{
    public class ControlAccesoNegocio : IControlAccesoNegocio
    {
        public Task<bool> validarHora()
        {
            bool siAcceso = true;

            // Obtener la zona horaria de Ciudad de México
            TimeZoneInfo mexicoTimeZone = TZConvert.GetTimeZoneInfo("America/Mexico_City");

            // Obtener la fecha y hora actual en UTC y convertirla a la zona horaria de México
            DateTime mexicoNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, mexicoTimeZone);

            // Obtener las variables que necesitas
            var diaActual = mexicoNow.DayOfWeek;  // Día de la semana (Sunday, Monday, etc.)
            var horaActual = mexicoNow.Hour;      // Hora actual (0-23)
            switch (diaActual)
            {
                case DayOfWeek.Saturday:
                    if (horaActual <= 7 || horaActual >= 14)
                    {
                        siAcceso = false;
                    }
                    break;
                case DayOfWeek.Sunday:
                    siAcceso = false;
                    break;
                default:
                    if (horaActual <= 7 || horaActual >= 21)
                    {
                        siAcceso = false;
                    }
                    break;
            }
            return Task.FromResult(siAcceso);
        }

        public Task<bool> validarUltimaHora()
        {
            bool ultimaHora = true;
            var horaActual = DateTime.Now.Hour;
            var minutosActual = DateTime.Now.Minute;
            if (horaActual == 16 || minutosActual >= 30)
            {
                ultimaHora = false;
            }
            return Task.FromResult(ultimaHora);
        }
    }
}
