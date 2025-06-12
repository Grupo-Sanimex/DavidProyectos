package com.app.sanimex.domain.model.Producto

data class SingleClient(
    val nombreCliente : String = "",
    val clasificacion : String = "",
    val descuentoRecoje : String = "",
    val descuentoContado : String = "",
    val precioFinal : Float =0.0f,
    val actualimporte : Float =0.0f,
    val actualmetros : Float =0.0f
)
