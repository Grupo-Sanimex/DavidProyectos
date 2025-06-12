package com.app.sanimex.data.remote.dto

import com.app.sanimex.domain.model.Sucursal

data class WishlistResponseDto(
    val `data`: Data,
    val status: String,
    val idRol : Int
) {
    data class Data(
        val wishlists: List<Wishlists>
    ) {
        data class Wishlists(
            val __v: Int,
            val product: Product,
            val idRol: Int
        ) {
            data class Product(
                val _idSucursal: String,
                val idUsuario: String,
                val nombreSucursal: String,
                val idSAP: String,
                val idRol: Int
            )

            fun toCommonProduct(): Sucursal {
                return Sucursal(
                    idSucursal = product._idSucursal,
                    idUsuario = product.idUsuario,
                    nombreSucursal = product.nombreSucursal,
                    idSAP = product.idSAP,
                    idRol = idRol
                )
            }
        }
    }
}