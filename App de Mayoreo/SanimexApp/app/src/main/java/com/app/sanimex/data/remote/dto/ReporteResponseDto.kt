package com.app.sanimex.data.remote.dto

import com.app.sanimex.domain.model.HistoricoVtaMayItem

data class ReporteResponseDto(
    val `data`: Data,
    val status: String
) {
    data class Data(
        val product: List<Product>
    ) {
        data class Product(
            val codigo: String,
            val descripcion: String,
            val importE_ACTUAL: Double,
            val cantidaD_ACTUAL: Int,
        ) {
            fun toCommonProduct(): HistoricoVtaMayItem {
                return HistoricoVtaMayItem(
                    codigo = codigo,
                    descripcion = descripcion,
                    IMPORTE_ACTUAL = importE_ACTUAL,
                    CANTIDAD_ACTUAL = cantidaD_ACTUAL
                )
            }
        }
    }
}