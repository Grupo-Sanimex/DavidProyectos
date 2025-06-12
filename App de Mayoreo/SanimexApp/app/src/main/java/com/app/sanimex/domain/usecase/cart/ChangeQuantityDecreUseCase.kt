package com.app.sanimex.domain.usecase.cart

import com.app.sanimex.core.util.Resource
import com.app.sanimex.domain.repository.NetworkRepository
import kotlinx.coroutines.flow.Flow
import javax.inject.Inject

/**
 * Caso de uso para decrementar en uno la cantidad de un producto en el carrito de compras.
 *
 * Este caso de uso encapsula la lógica para llamar al repositorio de red y realizar la
 * operación de decremento de cantidad de un producto específico en el carrito.
 *
 * @property networkRepository El repositorio de red inyectado que proporciona el acceso a la API.
 * @author David Duarte
 * @version 1.0
 */
class ChangeQuantityDecreUseCase@Inject constructor(
    private val networkRepository: NetworkRepository
) {
    /**
     * Invoca el caso de uso para decrementar en uno la cantidad de un producto en el carrito.
     *
     * Llama a la función `removeUnoFromCart` del [networkRepository] para realizar la operación.
     *
     * @param productID El ID del producto cuya cantidad se va a decrementar.
     * @param corredor El código del corredor asociado al carrito.
     * @return Un [Flow] de [Resource]<[Unit]> que emite el resultado de la operación (cargando, éxito o fallo).
     */
    suspend operator fun invoke(productID: String, corredor: String): Flow<Resource<Unit>>
    {
        return networkRepository.removeUnoFromCart(productID = productID, corredor = corredor)
    }
}