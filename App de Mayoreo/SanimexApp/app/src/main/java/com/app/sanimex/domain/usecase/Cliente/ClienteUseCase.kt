package com.app.sanimex.domain.usecase.Cliente

import com.app.sanimex.core.util.Resource
import com.app.sanimex.domain.repository.NetworkRepository
import kotlinx.coroutines.flow.Flow
import javax.inject.Inject

/**
 * Caso de uso para obtener la información de un cliente.
 *
 * Este caso de uso encapsula la lógica para llamar al repositorio de red y obtener
 * la información de un cliente específico utilizando su clave y el código del corredor.
 *
 * @property networkRepository El repositorio de red inyectado que proporciona el acceso a la API.
 * @author David Duarte
 * @version 1.0
 */
class ClienteUseCase @Inject constructor(
    private val networkRepository: NetworkRepository
) {
    /**
     * Invoca el caso de uso para obtener la información de un cliente.
     *
     * Llama a la función `cliente` del [networkRepository] para realizar la petición.
     *
     * @param claveCliente El ID del cliente a buscar.
     * @param corredor El código del corredor asociado al cliente.
     * @return Un [Flow] de [Resource]<[ClienteResponse]> que emite el resultado de la operación (cargando, éxito o fallo) y la información del cliente en caso de éxito.
     */
    suspend operator fun invoke(claveCliente: String, corredor: String): Flow<Resource<ClienteResponse>> {
        return networkRepository.cliente(claveCliente = claveCliente, corredor = corredor)
    }
}