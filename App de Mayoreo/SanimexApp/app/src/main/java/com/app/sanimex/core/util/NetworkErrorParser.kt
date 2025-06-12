

import retrofit2.HttpException
import com.app.sanimex.domain.model.Error


/**
 * Extensión de función para la clase [HttpException] que intenta analizar la excepción HTTP
 * y convertirla a un modelo de error específico de la aplicación ([Error]).
 *
 * Actualmente, la implementación simplemente crea y devuelve una nueva instancia de [Error]
 * con una cadena vacía como mensaje. En una implementación real, esta función debería
 * intentar extraer información relevante del cuerpo de la respuesta de la excepción HTTP
 * para construir un modelo de error más informativo.
 *
 * @return Una instancia de la clase [Error] que representa el error ocurrido durante la petición HTTP.
 * Actualmente, siempre devuelve un [Error] con un mensaje vacío.
 * @author David Duarte
 * @version 1.0
 */
fun HttpException.parseToErrorModel(): Error {
    // TODO: Implementar la lógica para analizar el cuerpo de la respuesta de la excepción
    // y extraer información para crear un ErrorModel más detallado.
    return Error("")
}