package com.app.sanimex.domain.usecase.GetProducto

import com.app.sanimex.core.util.Resource
import com.app.sanimex.domain.model.Producto.ClientData
import com.app.sanimex.domain.repository.NetworkRepository
import kotlinx.coroutines.flow.Flow
import javax.inject.Inject

class GetSingleClientUseCase@Inject constructor(
    private val networkRepository: NetworkRepository
) {
    suspend operator fun invoke(
        productID: String,
        corredor: String,
        ClaveCliente : String,
        tipoEntrega : Boolean,
        tipoPago : Boolean,
        TipoConsulta : Boolean,
        idDireccion : Int
    ): Flow<Resource<ClientData>> {
        return networkRepository.getSingleClient(
            id = productID, corredor = corredor,
            ClaveCliente = ClaveCliente,
            tipoEntrega = tipoEntrega,
            tipoPago = tipoPago,
            TipoConsulta = TipoConsulta,
            idDireccion = idDireccion)
    }
}