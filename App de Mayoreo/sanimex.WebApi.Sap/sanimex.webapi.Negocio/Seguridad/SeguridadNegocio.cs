using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace sanimex.webapi.Negocio.Seguridad
{
    public class SeguridadNegocio
    {
        public string ConvertirContraseña(string contrasena)
        {
            string cadenaASCII;

            try
            {
                using (MD5 md5Hasher = MD5.Create())
                {
                    byte[] data;
                    StringBuilder sBuilder = new StringBuilder();

                    // Convertir la contraseña a bytes usando ASCII y calcular el hash MD5
                    data = md5Hasher.ComputeHash(Encoding.ASCII.GetBytes(contrasena));

                    // Recorrer cada byte y convertirlo a cadena hexadecimal
                    foreach (byte b in data)
                    {
                        sBuilder.AppendFormat("{0:x2}", b);
                    }
                    cadenaASCII = sBuilder.ToString();

                    return cadenaASCII;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); // Puedes manejar el error de otra forma si lo prefieres
                return string.Empty;
            }
        }
    }
}
