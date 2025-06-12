package com.app.sanimex.data.remote.dto

import com.app.sanimex.domain.model.BusquedaProducto

data class SearchResponseDto(
    val `data`: Data,
    val status: String
) {
    data class Data(
        val products: List<Product>
    ) {
        data class Product(
            val _id: String,
            val images: List<String>,
            val name: String,
            val description: String
        ) {
            fun toCommonProduct(): BusquedaProducto {
                val image = if (this.images.isNotEmpty()) this.images.first() else ""
                return BusquedaProducto(
                    name = this.name,
                    description = this.description,
                    image = image,
                    id = _id
                )
            }
        }
    }

}