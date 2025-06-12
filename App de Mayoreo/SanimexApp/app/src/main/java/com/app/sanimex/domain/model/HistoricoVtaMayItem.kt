package com.app.sanimex.domain.model

data class HistoricoVtaMayItem(
    val codigo : String,
    val descripcion : String,
    val IMPORTE_ACTUAL : Double = 0.0,
    val CANTIDAD_ACTUAL : Int = 0
)
