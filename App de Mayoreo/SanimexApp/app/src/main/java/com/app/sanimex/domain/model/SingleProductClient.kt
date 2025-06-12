package com.app.sanimex.domain.model

data class SingleProductClient(
    val nombreCliente : String = "",
    val clasificacion : String = "",
    val descuentoRecoje : String = "",
    val descuentoContado : String = "",
    val precioFinal : Float =0.0f,
    val precioMetroFinal : Float =0.0f,
    val actualimporte : Float =0.0f,
    val actualmetros : Float =0.0f
)
