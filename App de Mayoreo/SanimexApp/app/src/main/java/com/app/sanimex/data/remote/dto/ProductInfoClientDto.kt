package com.app.sanimex.data.remote.dto

import com.app.sanimex.domain.model.Producto.ClientData
import com.app.sanimex.domain.model.SingleProductClient

data class ProductInfoClientDto(
    val client: Client,
    val status: String
) {
    data class Client(
        val nombreCliente: String,
        val clasificacion: String,
        val descuentoRecoje: String,
        val descuentoContado: String,
        val precioFinal: Float,
        val precioMetroFinal: Float,
        val actualimporte : Float,
        val actualmetros : Float
    ) {
        fun toSingleClientInfo(): SingleProductClient {
            return SingleProductClient(
                nombreCliente = nombreCliente,
                clasificacion = clasificacion,
                descuentoRecoje = descuentoRecoje,
                descuentoContado = descuentoContado,
                precioFinal = precioFinal,
                precioMetroFinal = precioMetroFinal,
                actualimporte = actualimporte,
                actualmetros = actualmetros
            )
        }
    }

   fun toClientData(): ClientData {
        return ClientData(
            clientInfo = client.toSingleClientInfo()
        )
    }

}