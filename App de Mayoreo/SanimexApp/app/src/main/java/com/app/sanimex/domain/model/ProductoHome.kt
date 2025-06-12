package com.app.sanimex.domain.model

data class ProductoHome(
    val Codigo: String,
    val Description: String,
    val image: String,
    val Weight: String,
    val PrecioProducto: Float = 0.0f,
    val Color: String?,
    val SquareMeter: String,
    val Classification: String,
    val Proveedor: String
)
