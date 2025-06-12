package com.app.sanimex.core.util

import kotlinx.coroutines.flow.Flow
import kotlinx.coroutines.flow.collectLatest


/**
 * Función de extensión suspendida para [Flow] de [Resource]<[T]> que simplifica el manejo de diferentes estados de la respuesta.
 *
 * Esta función recolecta los últimos valores emitidos por el [Flow] y ejecuta las funciones lambda
 * proporcionadas según el estado del [Resource] emitido:
 * - [onSuccess]: Se invoca si el [Resource] es de tipo [Resource.Success], proporcionando los datos (que pueden ser nulos).
 * - [onFailure]: Se invoca si el [Resource] es de tipo [Resource.Failure], proporcionando el mensaje de error (que puede ser nulo).
 * - [onLoading]: Se invoca si el [Resource] es de tipo [Resource.Loading].
 *
 * @param onSuccess Función lambda que se ejecuta cuando la respuesta es exitosa. Recibe los datos de tipo [T] (puede ser nulo).
 * @param onFailure Función lambda que se ejecuta cuando la respuesta falla. Recibe el mensaje de error (puede ser nulo).
 * @param onLoading Función lambda que se ejecuta cuando el [Flow] está emitiendo un estado de carga.
 * @author David Duarte
 * @version 1.0
 */
suspend fun <T> Flow<Resource<T>>.onResponse(
    onSuccess: (T?) -> Unit,
    onFailure: (String?) -> Unit,
    onLoading: () -> Unit
) {
    this.collectLatest { response ->
        when (response) {
            is Resource.Failure -> onFailure(response.message)
            is Resource.Loading -> onLoading()
            is Resource.Success -> onSuccess(response.data)
        }
    }
}