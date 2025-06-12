package com.app.sanimex.domain.usecase

import com.app.sanimex.core.util.Resource
import com.app.sanimex.domain.repository.NetworkRepository
import kotlinx.coroutines.flow.Flow
import javax.inject.Inject

class AddToCartUseCase @Inject constructor(
    private val networkRepository: NetworkRepository
) {
    suspend operator fun invoke(
        productID:String,
        descripsion: String,
        corredor:String,
        precioFinal: Float,
        cantidad:Int,
        claveCliente : String,
        recoge:Boolean,
        contado: Boolean,
        tipoConsulta:Boolean
    ): Flow<Resource<Unit>>{
        return networkRepository.addToCart(
            productID = productID,
            descripsion = descripsion,
            corredor = corredor,
            precioFinal = precioFinal,
            cantidad = cantidad,
            claveCliente = claveCliente,
            recoge = recoge,
            contado = contado,
            tipoConsulta = tipoConsulta)
    }
}