package com.app.sanimex.core.util


/**
 * Clase sellada genérica que representa el resultado de una operación asíncrona, como una llamada a la red o una lectura de base de datos.
 *
 * Esta clase tiene tres subtipos principales para indicar los diferentes estados de la operación:
 * - [Loading]: Indica que la operación está en curso. Puede contener datos previos opcionales.
 * - [Success]: Indica que la operación se completó con éxito y contiene los datos resultantes.
 * - [Failure]: Indica que la operación falló y contiene un mensaje de error opcional y datos previos opcionales.
 *
 * @param T El tipo de los datos que se transportan en el [Resource].
 * @property data Los datos asociados con el estado del recurso. Puede ser nulo.
 * @property message Un mensaje de error opcional asociado con el estado [Failure]. Nulo para [Loading] y [Success].
 * @author David Duarte
 * @version 1.0
 */
sealed class Resource<T>(val data: T? = null, val message: String? = null) {
    /**
     * Subclase que representa el estado de carga de una operación.
     *
     * Puede contener datos previos si están disponibles mientras se carga la nueva información.
     *
     * @param T El tipo de los datos que se están cargando.
     * @param data Los datos previos opcionales que pueden estar disponibles durante la carga.
     */
    class Loading<T>(data: T? = null) : Resource<T>(data = data)
    /**
     * Subclase que representa el estado de éxito de una operación.
     *
     * Contiene los datos resultantes de la operación exitosa.
     *
     * @param T El tipo de los datos obtenidos con éxito.
     * @param data Los datos resultantes de la operación exitosa. No puede ser nulo en este estado.
     */
    class Success<T>(data: T) : Resource<T>(data = data)
    /**
     * Subclase que representa el estado de fallo de una operación.
     *
     * Contiene un mensaje de error que describe la razón del fallo y puede contener datos previos opcionales.
     *
     * @param T El tipo de los datos que se intentaron obtener.
     * @param data Los datos previos opcionales que pueden estar disponibles a pesar del fallo.
     * @param message El mensaje de error que describe la razón del fallo. No puede ser nulo en este estado.
     */
    class Failure<T>(data: T? = null, message: String) : Resource<T>(data = data, message = message)
}