package com.app.sanimex.domain.model

data class SingleProductInfo(
    val id: String = "",
    val codigo: String = "",
    val description: String = "",
    val images: List<String> = emptyList(),
    val weight: String = "",
    val precioProducto : Float =0.0f,
    val precioMetroProducto : Float = 0.0f,
    val color : String = "",
    val squareMeter : String = "",
    val classification : String = "",
    val proveedor : String = "",
    val isInCart: Boolean = false
)
